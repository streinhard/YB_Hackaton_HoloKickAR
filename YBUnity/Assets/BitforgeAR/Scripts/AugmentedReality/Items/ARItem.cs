using DG.Tweening;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace AugmentedReality.Items
{
    public abstract class ARItem : MonoBehaviour
    {
        public float[] scales = { 0.5f, 1f };

        protected Tween _showHideIndicatorTween;
        protected bool IsIndicator { get; private set; }

        private int _scaleIndex;
        protected float CurrentScale => scales[_scaleIndex];

        protected virtual void Start()
        {
            // init animation (null pointer prevention)
            _showHideIndicatorTween = DOTween.Sequence();

            InitAndHide();
        }

        /// <summary>
        ///     Setup all relevant objects and hide them, so nothing is visible. Can be used as reset function.
        /// </summary>
        public virtual void InitAndHide()
        {
            IsIndicator = true;
            _showHideIndicatorTween.Pause();
            _showHideIndicatorTween.Rewind();
            UpdateScale(scales[_scaleIndex]);
        }

        public virtual void ShowIndicator()
        {
            if (!IsIndicator) {
                return;
            }

            if (_showHideIndicatorTween.IsPlaying() && !_showHideIndicatorTween.isBackwards ||
                _showHideIndicatorTween.IsComplete()) { return; }

            // start animation forward
            _showHideIndicatorTween.Restart();
        }

        public virtual void HideIndicator()
        {
            if (_showHideIndicatorTween.isBackwards) { return; }

            // start animation forward
            _showHideIndicatorTween.PlayBackwards();
        }

        public virtual void ShowFullObject()
        {
            IsIndicator = false;
        }

        public void NextScale()
        {
            _scaleIndex = (_scaleIndex + 1) % scales.Length; 
           UpdateScale(CurrentScale);
        }

        protected abstract void UpdateScale(float scale);
    }
}
