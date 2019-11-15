using System.Collections.Generic;
using CameraHelper;
using DG.Tweening;
using UnityEngine;

// ReSharper disable ConvertToAutoProperty
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable InconsistentNaming

namespace AugmentedReality.Items
{
    public class ARItemBuilder : ARItemDefault
    {
        [SerializeField]
        private GameObject[] buildPrefabs = null;

        [SerializeField]
        private Transform partsAnchor = null;

        [SerializeField]
        private Material deleteMaterial = null;

        [SerializeField]
        private GameObject basePlate = null;

        public bool CanPlaceObject { get; private set; }
        public bool CanDeleteObject { get; private set; }
        public bool IsInPlaceMode { get; private set; } = true;

        public GameObject BasePlate => basePlate;

        private Transform _cameraTransform;
        private LayerMask _basePlateLayerMask;
        private LayerMask _brickLayerMask;
        private ClipPlaneOptimizer _clipPlaneOptimizer;

        private int _buildPrefabIndex;
        private Transform _currentBuildPrefab;
        private Transform _brickToDelete;

        private readonly Dictionary<Renderer, Material> _originalMaterialDictionary =
            new Dictionary<Renderer, Material>();

        protected override void Awake()
        {
            base.Awake();

            _basePlateLayerMask = LayerMask.GetMask("POI");
            _brickLayerMask = LayerMask.GetMask("ShadowCaster");
        }

        protected override void Start()
        {
            base.Start();
            _clipPlaneOptimizer = FindObjectOfType<ClipPlaneOptimizer>();
            _cameraTransform = ARControllerStackLoader.GetArCamera().transform;
        }

        public override void InitAndHide()
        {
            base.InitAndHide();
            DeleteBrick(_currentBuildPrefab);
            _currentBuildPrefab = null;
        }

        public void SwitchMode()
        {
            // delete temporary brick 
            if (IsInPlaceMode) {
                DeleteBrick(_currentBuildPrefab);
                _currentBuildPrefab = null;
            }
            else {
                if (_brickToDelete != null) {
                    SetOriginalMaterial(_brickToDelete);
                    _brickToDelete = null;
                }
            }

            IsInPlaceMode = !IsInPlaceMode;
        }

        public void PlaceBrick()
        {
            if (CanPlaceObject && !ReferenceEquals(_currentBuildPrefab, null)) {
                _currentBuildPrefab.parent = partsAnchor;

                // remove brick from renderers (all renderers in items), used if scene gets reset
                AddRenderer(_currentBuildPrefab);

                _currentBuildPrefab = null;
                CanPlaceObject = false;

                SetShadowProjectorsDirty();
            }
        }

        public void DeleteBrick()
        {
            if (CanDeleteObject) {
                DeleteBrick(_brickToDelete);
                _brickToDelete = null;
            }
        }

        public void NextBrick()
        {
            // update index
            _buildPrefabIndex++;
            if (_buildPrefabIndex < 0) { _buildPrefabIndex = 0; }

            while (_buildPrefabIndex >= buildPrefabs.Length) { _buildPrefabIndex -= buildPrefabs.Length; }

            UpdateTemporaryBrick();
        }

        private void Update()
        {
            if (IsIndicator) { return; }

            if (IsInPlaceMode) {
                // place mode
                if (GetPoseHitOnFloor(out var newPose)) {
                    // check if there is already a build prefab
                    if (ReferenceEquals(_currentBuildPrefab, null)) { UpdateTemporaryBrick(); }

                    // update position
                    _currentBuildPrefab.position = Vector3.Lerp(_currentBuildPrefab.position, newPose.position, 0.5f);
                    _currentBuildPrefab.rotation = Quaternion.Lerp(
                        _currentBuildPrefab.rotation,
                        newPose.rotation,
                        0.5f
                    );

                    // update visible state
                    if (!CanPlaceObject) {
                        CanPlaceObject = true;
                        DOTween.Kill(_currentBuildPrefab);
                        _currentBuildPrefab.DOScale(1, 0.25f);
                    }
                }
                else {
                    if (CanPlaceObject && !ReferenceEquals(_currentBuildPrefab, null)) {
                        // scale it down if there is no base plane hit and object is still visible
                        CanPlaceObject = false;
                        DOTween.Kill(_currentBuildPrefab);
                        _currentBuildPrefab.DOScale(0, 0.25f);
                    }
                }
            }
            else {
                // delete mode
                if (GetBrickHit(out var hitBrick)) {
                    if (_brickToDelete == null) {
                        // no brick 2 delete is set
                        _brickToDelete = hitBrick;
                        SetDeleteMaterial(_brickToDelete);
                    }
                    else if (!ReferenceEquals(_brickToDelete, hitBrick)) {
                        // another selected brick is not the same anymore
                        SetOriginalMaterial(_brickToDelete);

                        // replace with new one
                        _brickToDelete = hitBrick;
                        SetDeleteMaterial(_brickToDelete);
                    }

                    CanDeleteObject = true;
                }
                else {
                    if (!ReferenceEquals(_brickToDelete, null)) {
                        SetOriginalMaterial(_brickToDelete);
                        _brickToDelete = null;
                    }

                    CanDeleteObject = false;
                }
            }
        }

        private void SetDeleteMaterial(Transform brick)
        {
            brick = GetBrickParent(brick);
            var brickRenderers = brick.GetComponentsInChildren<Renderer>(true);
            foreach (var r in brickRenderers) {
                if (!_originalMaterialDictionary.ContainsKey(r)) {
                    _originalMaterialDictionary.Add(r, r.sharedMaterial);
                }

                r.material = deleteMaterial;
            }
        }

        private void SetOriginalMaterial(Transform brick)
        {
            brick = GetBrickParent(brick);
            var brickRenderers = brick.GetComponentsInChildren<Renderer>(true);
            foreach (var r in brickRenderers) {
                if (_originalMaterialDictionary.TryGetValue(r, out var originalMaterial)) {
                    r.sharedMaterial = originalMaterial;
                }
            }
        }

        private void UpdateTemporaryBrick()
        {
            // destroy old prefab if there is any
            DeleteBrick(_currentBuildPrefab);
            _currentBuildPrefab = null;

            // instantiate new element 
            var newBrick = Instantiate(buildPrefabs[_buildPrefabIndex], partsAnchor);
            _currentBuildPrefab = newBrick.transform;
            _clipPlaneOptimizer.TryToAdd(_currentBuildPrefab);
            _currentBuildPrefab.localScale = Vector3.zero;
            CanPlaceObject = false;
        }

        private bool GetPoseHitOnFloor(out Pose pose, float distance = 10)
        {
            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            if (Physics.Raycast(ray, out var hitInfo, distance, _basePlateLayerMask)) {
                // create rotation based on camera but always up
                var forward = _cameraTransform.forward;
                forward.y = 0;
                pose = new Pose(hitInfo.point, Quaternion.LookRotation(forward, Vector3.up));
                return true;
            }

            pose = new Pose();
            return false;
        }

        private bool GetBrickHit(out Transform hitBrick, float distance = 10)
        {
            var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            if (Physics.Raycast(ray, out var hitInfo, distance, _brickLayerMask)) {
                // create rotation based on camera but always up
                hitBrick = hitInfo.transform;
                return true;
            }

            hitBrick = null;
            return false;
        }

        private void DeleteBrick(Transform brick)
        {
            if (brick != null) {
                // get topmost item
                brick = GetBrickParent(brick);

                // remove brick from renderers (all renderers in items)
                RemoveRenderer(brick);

                // set layer to 0 on all children to disable collision
                var brickTransforms = brick.GetComponentsInChildren<Transform>(true);
                foreach (var brickTransform in brickTransforms) { brickTransform.gameObject.layer = 0; }

                DOTween.Kill(brick);
                brick.DOScale(0, 0.25f).SetEase(Ease.OutCubic).OnComplete(
                    () =>
                    {
                        _clipPlaneOptimizer.TryToRemove(brick);
                        SetShadowProjectorsDirty();
                        Destroy(brick.gameObject);
                    }
                );
            }
        }

        private Transform GetBrickParent(Transform brick)
        {
            while (brick.parent != null && brick.parent != partsAnchor) { brick = brick.parent; }

            return brick;
        }
    }
}
