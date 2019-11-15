using DG.Tweening;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer

namespace CameraHelper
{
    [RequireComponent(typeof(Camera))]
    public class BlurOnCollisionPostProcessing : MonoBehaviour
    {
        [SerializeField]
        private Material postprocessMaterial = null;

        [SerializeField]
        [Range(0.001f, 0.999f)]
        private float minResolutionFactor = 0.05f;

        [SerializeField]
        [Range(0.001f, 0.1f)]
        private float endBlurSize = 0.05f;

        private Tween _fadeAnimation;
        private float _tweenResolutionFactor;
        private static readonly int BlurSize = Shader.PropertyToID("_BlurSize");

        private void OnEnable()
        {
            _fadeAnimation?.Kill();
            _tweenResolutionFactor = 1;
            SetBlurSize(0);
            _fadeAnimation = DOVirtual.Float(0, 1, 0.25f, TweenBlur).SetEase(Ease.Linear);
        }

        private void OnDisable()
        {
            _fadeAnimation?.Kill();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //draws the pixels from the source texture to the destination texture
            var temporaryTexture = RenderTexture.GetTemporary(
                Mathf.RoundToInt(source.width * _tweenResolutionFactor),
                Mathf.RoundToInt(source.height * _tweenResolutionFactor)
            );
            Graphics.Blit(source, temporaryTexture, postprocessMaterial, 0);
            Graphics.Blit(temporaryTexture, destination, postprocessMaterial, 1);
            RenderTexture.ReleaseTemporary(temporaryTexture);
        }

        private void TweenBlur(float value)
        {
            _tweenResolutionFactor = minResolutionFactor + (1 - minResolutionFactor) * (1 - value);
            SetBlurSize(value * endBlurSize);
        }

        private void SetBlurSize(float blurSize)
        {
            if (postprocessMaterial != null) {
                postprocessMaterial.SetFloat(BlurSize, blurSize); //C#
            }
        }
    }
}
