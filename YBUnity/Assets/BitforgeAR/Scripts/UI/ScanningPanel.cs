using AugmentedReality;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

// ReSharper disable RedundantDefaultMemberInitializer

namespace UI
{
    public class ScanningPanel : GenericPanel
    {
        [SerializeField]
        private Text titleText = null;

        [SerializeField]
        private Image movingIcon = null;

        [SerializeField]
        private RectTransform tipTextRect = null;

        private Text _tipText;

        private Tween _tipCarouselTween;
        private Tween _tipShowHideTween;
        private Tween _phoneMoveTween;
        private Tween _phoneFadeTween;

        private readonly float _tipTextYOffset = -Screen.height;

        private Tween _sessionCheckTween;

        protected override void Awake()
        {
            base.Awake();
            _tipText = tipTextRect.GetComponentInChildren<Text>();
        }

        public override void Show()
        {
            base.Show();
            StartMovingIconAnimation();
            StartInfoTextAnimation();
        }

        public override void Hide()
        {
            base.Hide();
            StopMovingIconAnimation(true);
            StopInfoTextAnimation(true);
            StopSessionCheck();
        }

        private void StartMovingIconAnimation()
        {
            _phoneMoveTween?.Kill();
            _phoneFadeTween?.Kill();

            if (movingIcon != null) {
                // stop all running animation on the icon
                DOTween.Kill(movingIcon);

                // start animation
                const float totalAnimationTime = 4;
                const float extendX = 100;
                const float extendY = -20;
                const float rotationX = 15;
                const float rotationY = 45;
                var oneForth = totalAnimationTime / 4f;
                var rectTransform = movingIcon.transform;

                var sequence = DOTween.Sequence();

                sequence.Insert(0, rectTransform.DOLocalMoveX(extendX, oneForth).SetEase(Ease.Linear));
                sequence.Insert(0, rectTransform.DOLocalMoveY(extendY, oneForth).SetEase(Ease.InCubic));
                sequence.Insert(
                    0,
                    rectTransform.DOLocalRotate(new Vector3(rotationX, rotationY, 0), oneForth).SetEase(Ease.Linear)
                );

                sequence.Insert(oneForth, rectTransform.DOLocalMoveX(0, oneForth).SetEase(Ease.Linear));
                sequence.Insert(oneForth, rectTransform.DOLocalMoveY(0, oneForth).SetEase(Ease.OutCubic));
                sequence.Insert(oneForth, rectTransform.DOLocalRotate(Vector3.zero, oneForth).SetEase(Ease.Linear));

                sequence.Insert(2 * oneForth, rectTransform.DOLocalMoveX(-extendX, oneForth).SetEase(Ease.Linear));
                sequence.Insert(2 * oneForth, rectTransform.DOLocalMoveY(extendY, oneForth).SetEase(Ease.InCubic));
                sequence.Insert(
                    2 * oneForth,
                    rectTransform.DOLocalRotate(new Vector3(rotationX, -rotationY, 0), oneForth).SetEase(Ease.Linear)
                );

                sequence.Insert(3 * oneForth, rectTransform.DOLocalMoveX(0, oneForth).SetEase(Ease.Linear));
                sequence.Insert(3 * oneForth, rectTransform.DOLocalMoveY(0, oneForth).SetEase(Ease.OutCubic));
                sequence.Insert(3 * oneForth, rectTransform.DOLocalRotate(Vector3.zero, oneForth).SetEase(Ease.Linear));

                sequence.SetLoops(-1, LoopType.Restart);
                sequence.Play();

                _phoneMoveTween = sequence;

                // start fade in as second animation
                _phoneFadeTween = movingIcon.DOFade(1, oneForth / 2).SetEase(Ease.InQuad);
            }
        }

        private void StopMovingIconAnimation(bool immediate)
        {
            DOTween.Kill(movingIcon);
            _phoneMoveTween?.Kill();
            _phoneFadeTween?.Kill();

            if (immediate) { movingIcon.color = new Color(1, 1, 1, 0); }
            else { movingIcon.DOFade(0, 0.25f); }
        }

        private void StartInfoTextAnimation()
        {
            const float showDelay = 3f;
            const float tipShowTime = 5f;
            const float tipCarouselSwitchTimeHalf = 0.3f;

            _tipCarouselTween?.Kill();
            _tipShowHideTween?.Kill();

            // vertical show animation
            _tipShowHideTween = DOVirtual.DelayedCall(
                showDelay,
                () =>

                {
                    SetTipTextDown();

                    // tip carousel
                    var sequence = DOTween.Sequence();

                    sequence.AppendCallback(
                        () =>
                        {
                            _tipText.text = Localization.GetText("unity_scanning_tip_0");
                            _tipText.SetLayoutDirty();
                        }
                    );
                    sequence.Append(tipTextRect.DOAnchorPosY(0, tipCarouselSwitchTimeHalf));

                    sequence.AppendInterval(tipShowTime);

                    sequence.Append(tipTextRect.DOAnchorPosY(_tipTextYOffset, tipCarouselSwitchTimeHalf));
                    sequence.AppendCallback(
                        () =>
                        {
                            _tipText.text = Localization.GetText("unity_scanning_tip_1");
                            _tipText.SetLayoutDirty();
                        }
                    );
                    sequence.Append(tipTextRect.DOAnchorPosY(0, 0.5f));

                    sequence.AppendInterval(tipShowTime);

                    sequence.Append(tipTextRect.DOAnchorPosY(_tipTextYOffset, tipCarouselSwitchTimeHalf));
                    sequence.AppendCallback(
                        () =>
                        {
                            _tipText.text = Localization.GetText("unity_scanning_tip_2");
                            _tipText.SetLayoutDirty();
                        }
                    );
                    sequence.Append(tipTextRect.DOAnchorPosY(0, 0.5f));

                    sequence.AppendInterval(tipShowTime);

                    sequence.Append(tipTextRect.DOAnchorPosY(_tipTextYOffset, tipCarouselSwitchTimeHalf));

                    sequence.SetLoops(-1, LoopType.Restart);

                    _tipCarouselTween = sequence;
                }
            );
        }

        private void StopInfoTextAnimation(bool immediate)
        {
            _tipCarouselTween?.Kill();
            _tipShowHideTween?.Kill();

            SetTipTextDown();
        }

        private void SetTipTextDown()
        {
            var anchorPosition = tipTextRect.anchoredPosition;
            anchorPosition.y = _tipTextYOffset;
            tipTextRect.anchoredPosition = anchorPosition;
        }

        #region ar working check

        public void StartSessionCheck(IARSessionController arSessionController)
        {
            // stop
            _sessionCheckTween?.Kill();

            if (arSessionController.GetARSystemState() == ARSessionState.SessionInitializing) {
                var sequence = DOTween.Sequence();
                sequence.AppendInterval(15);
                sequence.AppendCallback(
                    () =>
                    {
                        if (arSessionController.GetARSystemState() == ARSessionState.SessionInitializing) {
                            titleText.alignment = TextAnchor.MiddleCenter;
                            titleText.verticalOverflow = VerticalWrapMode.Overflow;
                            titleText.rectTransform.anchorMin = Vector2.zero;
                            titleText.text = Localization.GetText("unity_ar_initialization_failed");
                            StopMovingIconAnimation(false);
                            StopInfoTextAnimation(false);
                        }
                    }
                );
                _sessionCheckTween = sequence;
            }
        }

        private void StopSessionCheck()
        {
            // stop
            _sessionCheckTween?.Kill();
            _sessionCheckTween = null;
        }
        #endregion
    }
}
