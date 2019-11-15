using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality.Items
{
    public class ARItemDefault : ARItem
    {
        private const int floorLayerMask = 9;
        private const float showHideAnimationDuration = 0.25f;
        private const float showFullAnimationDuration = 0.6f;

        [SerializeField]
        private Transform itemAnchor = null;

        [SerializeField]
        private Transform indicatorAnchor = null;

        [SerializeField]
        private Material itemTransparentMaterial = null;

        [SerializeField]
        private Material itemTransparentSkinnedMaterial = null;


        private List<Renderer> _itemRenderers;
        private List<Material[]> _itemOriginalMaterials;
        private List<Material[]> _itemTransparentMaterials;
        private CanvasGroup _indicatorCanvasGroup;
        private ShadowProjectorController[] _shadowProjectorControllers;
        private MeshRenderer _itemFloorRenderer;

        protected virtual void Awake()
        {
            _indicatorCanvasGroup = indicatorAnchor.GetComponentInChildren<CanvasGroup>();
            _shadowProjectorControllers = GetComponentsInChildren<ShadowProjectorController>();
            _itemRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>());

            // cache the original materials
            if (_itemTransparentMaterials == null || _itemOriginalMaterials == null) {

                var initRenderers = GetComponentsInChildren<Renderer>();

                _itemRenderers = new List<Renderer>(initRenderers.Length);
                _itemOriginalMaterials = new List<Material[]>(initRenderers.Length);
                _itemTransparentMaterials = new List<Material[]>(initRenderers.Length);

                foreach (var r in initRenderers) {
                    AddRenderer(r);

                    if (r.gameObject.layer == floorLayerMask && r is MeshRenderer mr) {
                        _itemFloorRenderer = mr;
                    }
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            // setup indicator animation
            var sequence = DOTween.Sequence().SetAutoKill(false).Pause();

            sequence.Insert(0, _indicatorCanvasGroup.DOFade(1, showHideAnimationDuration).SetEase(Ease.InCubic));
            sequence.Insert(0, indicatorAnchor.DOScale(1, showHideAnimationDuration).SetEase(Ease.InCubic));

            sequence.Insert(0, itemAnchor.DOScale(1, showHideAnimationDuration).SetEase(Ease.InCubic));
            sequence.Insert(0, itemAnchor.DOLocalMoveY(0.1f, showHideAnimationDuration).SetEase(Ease.InCubic));

            _showHideIndicatorTween = sequence;
        }

        public override void InitAndHide()
        {
            indicatorAnchor.gameObject.SetActive(true);
            indicatorAnchor.localScale = Vector3.zero;
            _indicatorCanvasGroup.alpha = 0;

            itemAnchor.localScale = Vector3.zero;
            itemAnchor.localPosition = Vector3.zero;

            // reset materials to transparent
            //SetMaterialsOnRenderers(_itemTransparentMaterials, _itemRenderers);
            //ToggleShadowProjectors(false);

            if(_itemFloorRenderer!=null)
            _itemFloorRenderer.enabled = false;

            base.InitAndHide();
        }

        protected void AddRenderer(Transform t)
        {
            var renderers = t.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) {
                AddRenderer(r);
            }
        }

        private void AddRenderer(Renderer r)
        {
            if (!_itemRenderers.Contains(r)) {
                _itemRenderers.Add(r);

                var isSkinnedRenderer = r is SkinnedMeshRenderer;
                var sharedMaterials = r.sharedMaterials;
                var originalMaterials = new Material[sharedMaterials.Length];
                var transparentMaterials = new Material[sharedMaterials.Length];

                for (var i = 0; i < sharedMaterials.Length; i++) {
                    originalMaterials[i] = sharedMaterials[i];

                    // use skinned material effect for all human, so they look the same as the meshrenderr
                    transparentMaterials[i] = isSkinnedRenderer ? itemTransparentSkinnedMaterial : itemTransparentMaterial;
                }

                _itemOriginalMaterials.Add(originalMaterials);
                _itemTransparentMaterials.Add(transparentMaterials);
            }
        }

        protected void RemoveRenderer(Transform t)
        {
            var renderers = t.GetComponentsInChildren<Renderer>();
            foreach (var r in renderers) {
                RemoveRenderer(r);
            }
        }

        private void RemoveRenderer(Renderer r)
        {
            var index = _itemRenderers.IndexOf(r);
            if (index >= 0) {
                _itemRenderers.RemoveAt(index);
                _itemOriginalMaterials.RemoveAt(index);
                _itemTransparentMaterials.RemoveAt(index);
            }
        }

        public override void ShowFullObject()
        {
            base.ShowFullObject();

            DOTween.Kill(itemAnchor);
            DOTween.Kill(_indicatorCanvasGroup);
            DOTween.Kill(indicatorAnchor);

            var sequenceDurationHalf = showFullAnimationDuration / 2;
            var sequence = DOTween.Sequence();

            // place down
            sequence.Insert(0, itemAnchor.DOLocalMoveY(0f, sequenceDurationHalf).SetEase(Ease.InCubic));

            // scale to 0
            sequence.Insert(
                sequenceDurationHalf,
                _indicatorCanvasGroup.DOFade(0, sequenceDurationHalf).SetEase(Ease.InCubic)
            );
            sequence.Insert(
                sequenceDurationHalf,
                indicatorAnchor.DOScale(0, sequenceDurationHalf).SetEase(Ease.InCubic)
            );

            // change texture and turn on shadows
            sequence.InsertCallback(
                sequenceDurationHalf,
                () =>
                {
                    SetMaterialsOnRenderers(_itemOriginalMaterials, _itemRenderers);
                    
                    if(_itemFloorRenderer!=null)
                    _itemFloorRenderer.enabled = true;
                    ToggleShadowProjectors(true);
                }
            );

            sequence.InsertCallback(
                showFullAnimationDuration,
                () =>
                {
                    indicatorAnchor.gameObject.SetActive(false);
                }
            );
        }

        protected override void UpdateScale(float scale)
        {
            transform.localScale = new Vector3(scale, scale, scale);
        }

        protected void SetShadowProjectorsDirty()
        {
            if (_shadowProjectorControllers != null && _shadowProjectorControllers.Length > 0) {
                foreach (var p in _shadowProjectorControllers) { p.SetDirty(); }
            }
        }

        private void ToggleShadowProjectors(bool active)
        {
            if (_shadowProjectorControllers != null && _shadowProjectorControllers.Length > 0) {
                foreach (var p in _shadowProjectorControllers) { p.gameObject.SetActive(active); }
            }
        }

        private static void SetMaterialsOnRenderers(
            IReadOnlyList<Material[]> materials,
            IReadOnlyList<Renderer> renderers
        )
        {
            for (var r = 0; r < renderers.Count; r++) {
                var singleRenderer = renderers[r];
                singleRenderer.sharedMaterials = materials[r];
            }
        }
    }
}
