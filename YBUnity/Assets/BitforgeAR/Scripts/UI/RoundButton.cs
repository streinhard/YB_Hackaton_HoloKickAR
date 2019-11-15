using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class RoundButton : MonoBehaviour
    {
        private const float ANIMATION_TIME = 0.2f;
        
        [SerializeField]
        private Button button = null;
        
        [SerializeField]
        private Image shadow = null;

        private Sequence _animation;

        public Button.ButtonClickedEvent OnClick => button.onClick;

        private void Awake()
        {
            Hide(true);
        }

        public void Show(float delay = 0f)
        {
            _animation?.Kill();

            _animation = DOTween.Sequence();
            _animation.Insert(0, transform.DOScale(Vector3.one, ANIMATION_TIME).SetEase(Ease.OutCubic));
            _animation.Insert(0, shadow.transform.DOScale(Vector3.one, ANIMATION_TIME).SetEase(Ease.OutCubic));
            
            if (delay > 0) { _animation.SetDelay(delay); }
        }

        public void Hide(bool immediate)
        {
            _animation?.Kill();

            if (immediate) {
                transform.localScale = Vector3.zero;
                shadow.transform.localScale = Vector3.zero;
            }
            else {
                _animation = DOTween.Sequence();
                _animation.Insert(0, transform.DOScale(Vector3.zero, ANIMATION_TIME).SetEase(Ease.OutCubic));
                _animation.Insert(0, shadow.transform.DOScale(Vector3.zero, ANIMATION_TIME).SetEase(Ease.OutCubic));
            }
        }
    }
}
