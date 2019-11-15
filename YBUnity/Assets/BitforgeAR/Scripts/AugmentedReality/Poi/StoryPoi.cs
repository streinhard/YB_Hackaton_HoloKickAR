using DG.Tweening;
using UnityEngine;

namespace AugmentedReality.Poi
{
    public class StoryPoi : BasePoi
    {
        [SerializeField]
        private Vector3 shakeRotation = new Vector3(30, 30, 30);

        private Tween _showHideTween;
        private Tween _gazeOnOffTween;

        private Transform _thisTransform;
        private Transform _childTransform;

        private void Awake()
        {
            _thisTransform = transform;
            _childTransform = _thisTransform.GetChild(0);
        }

        public override void GazeOn()
        {
            _gazeOnOffTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Append(_childTransform.DOLocalMove(new Vector3(0, .1f, 0), 0.25f));
            sequence.Insert(0.15f, _childTransform.DOShakeRotation(0.3f, shakeRotation, 5, 0, false));
            _gazeOnOffTween = sequence;
        }

        public override void GazeOff()
        {
            _gazeOnOffTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Insert(0, _childTransform.DOLocalMove(Vector3.zero, 0.2f));
            sequence.Insert(0, _childTransform.DOLocalRotate(Vector3.zero, 0.2f));
            _gazeOnOffTween = sequence;
        }

        public void JustPunch()
        {
            _gazeOnOffTween?.Kill();

            var sequenceOn = DOTween.Sequence();
            sequenceOn.Append(_childTransform.DOLocalMove(new Vector3(0, .1f, 0), 0.25f));
            sequenceOn.Insert(0.15f, _childTransform.DOShakeRotation(0.3f, shakeRotation, 5, 0, false));

            var sequenceOff = DOTween.Sequence();
            sequenceOff.Insert(0, _childTransform.DOLocalMove(Vector3.zero, 0.2f));
            sequenceOff.Insert(0, _childTransform.DOLocalRotate(Vector3.zero, 0.2f));

            sequenceOn.Append(sequenceOff);
            _gazeOnOffTween = sequenceOn;
        }

        public override void Show()
        {
            // no show & hide at the moment
        }

        public override void Hide(bool immediate)
        {
            // no show & hide at the moment
        }

        public override void UpdateScale(float scale) { }

        [ContextMenu("TestShakeRotation")]
        private void TestShakeRotationTestShakeRotation()
        {
            _childTransform.DOShakeRotation(0.5f, shakeRotation, 5, 0, false);
        }
    }
}
