using DG.Tweening;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality.Poi
{
    public class InteractionPoint : BasePoi
    {
        [SerializeField]
        public string imageUrl = null;

        [SerializeField]
        public Texture imageOverride;

        [SerializeField]
        private float activeScale = 0.05f;

        [SerializeField]
        private float passiveScale = 0.02f;

        [SerializeField]
        private Color activeColor = new Color(23f / 255, 93f / 255, 168f / 255);

        [SerializeField]
        private Color passiveColor = Color.white;

        [SerializeField]
        private LineRenderer lineRenderer = null;

        [SerializeField]
        private float lineWidth = 0.01f;

        [SerializeField]
        private bool hideLine = false;

        private Sequence _showHideAnimation;
        private Tween _gazeAnimation;
        private Vector3 _originalLocalPosition;

        private Transform _thisTransform;
        private Transform _visualTransform;
        private Renderer _visualRenderer;
        private float _actualScale;

        private void Awake()
        {
            _thisTransform = transform;
            _visualTransform = _thisTransform.GetChild(0);
            _visualRenderer = _visualTransform.GetComponent<MeshRenderer>();
            _originalLocalPosition = _thisTransform.localPosition;

            if (!hideLine) {
                lineRenderer.startWidth = lineWidth;
                lineRenderer.endWidth = lineWidth;
            }
        }

        private void OnEnable()
        {
            UpdateLineRenderer();
            lineRenderer.enabled = !hideLine;
        }

        private void OnDisable()
        {
            lineRenderer.enabled = false;
        }

        public override void GazeOn()
        {
            _gazeAnimation?.Kill();
            
            _gazeAnimation = _visualTransform.DOScale(activeScale, 0.1f).SetEase(Ease.InCubic);
            _visualRenderer.material.color = activeColor;
            UpdateLineWidth(true);
            
            lineRenderer.endColor = activeColor;
        }

        public override void GazeOff()
        {
            _gazeAnimation?.Kill();
            
            _gazeAnimation = _visualTransform.DOScale(passiveScale, 0.1f).SetEase(Ease.InCubic);
            UpdateLineWidth();
            _visualRenderer.material.color = passiveColor;
            
            lineRenderer.endColor = passiveColor;
        }

        public override void Show()
        {
            _showHideAnimation?.Kill();

            if (!enabled) {
                // restore init state if no animation is running
                Hide(true);
                enabled = true;
            }

            var thisTransform = transform;
            _showHideAnimation = DOTween.Sequence();
            _showHideAnimation.Insert(
                0,
                thisTransform.DOLocalMove(_originalLocalPosition, 0.25f).SetEase(Ease.InCubic)
            );
            _showHideAnimation.Insert(0, thisTransform.DOScale(1, 0.25f).SetEase(Ease.InCubic));
        }

        public override void Hide(bool immediate)
        {
            _showHideAnimation?.Kill();

            var thisTransform = transform;

            if (immediate) {
                thisTransform.localPosition = Vector3.zero;
                thisTransform.localScale = Vector3.zero;
                enabled = false;
            }
            else {
                _showHideAnimation = DOTween.Sequence();
                _showHideAnimation.Insert(0, thisTransform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutCubic));
                _showHideAnimation.Insert(0, thisTransform.DOScale(0, 0.25f).SetEase(Ease.OutCubic));
                _showHideAnimation.AppendCallback(() => enabled = false);
            }
        }

        private void Update()
        {
            UpdateLineRenderer();
        }

        public override void UpdateScale(float scale)
        {
            _actualScale = scale;
            UpdateLineWidth();
        }

        [ContextMenu("UpdateLineRenderer - Positions")]
        private void UpdateLineRenderer()
        {
            #if !UNITY_EDITOR
            if (!hideLine)
            #endif
            {
                UpdateSingleLinePosition(0, Vector3.zero);
                UpdateSingleLinePosition(1, transform.localPosition);
            }
        }

        [ContextMenu("UpdateLineRenderer - Width")]
        private void UpdateLineWidth(bool selected = false)
        {
            #if !UNITY_EDITOR
            if (!hideLine)
            #endif
            {
                lineRenderer.startWidth = _actualScale * lineWidth * (selected ? 0.5f : 0.75f);
                lineRenderer.endWidth = _actualScale * lineWidth * (selected ? 2f : 1f);
            }
        }

        private void UpdateSingleLinePosition(int index, Vector3 position)
        {
            #if !UNITY_EDITOR
            if (!hideLine)
            #endif
            {
                if (lineRenderer.GetPosition(index) != position) { lineRenderer.SetPosition(index, position); } 
            }
        }
    }
}
