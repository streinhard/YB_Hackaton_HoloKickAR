using DG.Tweening;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable InconsistentNaming

namespace AugmentedReality.Items
{
    public class ARItemMauren : ARItemDefault
    {
        private const float FLOODING_ANIMATION_TIME = 2f;
        private const string BLENDSHAPE_NAME = "Normal";

        [SerializeField]
        private SkinnedMeshRenderer _waterMesh = null;

        [SerializeField]
        private Transform _yourPositionJumpAnchor = null;

        [SerializeField]
        private Transform _railwayBridgeJumpAnchor = null;

        private int _blendshapeIndex = -1;

        private int BlendshapeIndex
        {
            get
            {
                if (_blendshapeIndex < 0) {
                    var mesh = _waterMesh.sharedMesh;
                    _blendshapeIndex = mesh.GetBlendShapeIndex(BLENDSHAPE_NAME);
                }

                return _blendshapeIndex;
            }
        }

        public int ExperienceStep
        {
            get => _experienceStep;
            set => SetExperienceStep(value);
        }

        private int _experienceStep = -1;
        private Tween _tween;
        private Transform _cameraTransform;
        private Transform _yourPosition;
        private Transform _railwayBridge;

        protected override void Awake()
        {
            _yourPosition = _yourPositionJumpAnchor.parent;
            _railwayBridge = _railwayBridgeJumpAnchor.parent;

            base.Awake();
        }

        protected override void Start()
        {
            _cameraTransform = ARControllerStackLoader.GetArCamera().transform;
            base.Start();
        }

        public override void InitAndHide()
        {
            SetExperienceStep(0, true);
            base.InitAndHide();
        }

        public void ToggleExperienceStep()
        {
            SetExperienceStep(ExperienceStep == 0 ? 1 : 0);
        }

        private void Update()
        {
            // make marker look in direction of camera if no animation is running and 
            // scene is in showing mode
            if (!IsIndicator) {
                var targetPosition = _cameraTransform.position;
                targetPosition.y = _yourPosition.position.y;
                _yourPosition.LookAt(targetPosition);

                targetPosition.y = _railwayBridge.position.y;
                _railwayBridge.LookAt(targetPosition);
            }
        }

        private void SetExperienceStep(int newStep, bool immediate = false)
        {
            newStep = Mathf.Clamp(newStep, 0, 1);

            if (newStep != _experienceStep) {
                _experienceStep = newStep;

                switch (_experienceStep) {
                    // railway bridge
                    case 0: {
                        ShowHideFlooding(false, immediate);
                        break;
                    }

                    // flooding
                    case 1: {
                        ShowHideFlooding(true, immediate);
                        break;
                    }
                }
            }
        }

        private void ShowHideFlooding(bool isShow, bool immediate = false)
        {
            _tween?.Kill();

            var actualParent = isShow ? _railwayBridge : _yourPosition;
            var nextParent = isShow ? _yourPosition : _railwayBridge;
            var nextJumpAnchor = isShow ? _yourPositionJumpAnchor : _railwayBridgeJumpAnchor;

            if (immediate) {
                actualParent.localScale = Vector3.zero;
                nextParent.localScale = Vector3.one;

                SetInverseBlendWeight01(isShow ? 100 : 0);
            }
            else {
                var actualWeight = GetInverseBlendWeight01();
                var floodAnimationTime = isShow ? 1 - actualWeight : actualWeight;
                if (floodAnimationTime < 1E-6f) { return; }

                floodAnimationTime *= FLOODING_ANIMATION_TIME;

                // fasten animation when backwards
                if (!isShow) { floodAnimationTime /= 2f; }


                _yourPositionJumpAnchor.localRotation = Quaternion.identity;

                var sequence = DOTween.Sequence();
                sequence.Insert(
                    0,
                    DOVirtual.Float(actualWeight, isShow ? 1 : 0, floodAnimationTime, SetInverseBlendWeight01)
                        .SetEase(Ease.Linear)
                );
                sequence.Insert(0, actualParent.DOScale(0, 0.25f));
                sequence.Insert(floodAnimationTime - 0.5f, nextParent.DOScale(1, 0.25f));
                sequence.Insert(
                    floodAnimationTime - 0.3f,
                    nextJumpAnchor.DOJump(nextParent.position, CurrentScale * 0.005f, 1, 0.25f)
                );
                sequence.Insert(
                    floodAnimationTime - 0.3f,
                    nextJumpAnchor.DOLocalRotate(new Vector3(0, 360, 0), 0.25f, RotateMode.FastBeyond360)
                );

                _tween = sequence;
                _tween.OnKill(() => { _tween = null; });
            }
        }

        private void SetInverseBlendWeight01(float weight)
        {
            // disable mesh if super low value near to zero
            _waterMesh.enabled = !(weight <= 1E-06f);

            // make it inverse (blendshape is inverse)
            weight = Mathf.Clamp01(weight);
            weight = 1f - weight;

            // correct curve to match rubens blendshape animation start - end = 70 - 0
            weight *= 0.7f; // delay end to match the 70

            _waterMesh.SetBlendShapeWeight(BlendshapeIndex, 100f * weight);
        }

        private float GetInverseBlendWeight01()
        {
            var weight = _waterMesh.GetBlendShapeWeight(BlendshapeIndex) / 100f;
            weight = 1f - weight;
            return Mathf.Clamp01(weight);
        }
    }
}
