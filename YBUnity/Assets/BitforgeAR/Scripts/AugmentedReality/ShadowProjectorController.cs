using System;
using DynamicShadowProjector;
using UnityEngine;

// ReSharper disable RedundantDefaultMemberInitializer

namespace AugmentedReality
{
    [RequireComponent(typeof(Projector), typeof(DrawTargetObject))]
    public class ShadowProjectorController : MonoBehaviour
    {
        [SerializeField]
        private float projectOverFloor = 0.1f;

        private Transform _thisTransform;
        private Projector _projector;
        private DrawTargetObject _drawTargetObject;
        private float _lastLossyScaleZ = 1f;
        private bool _isDirty = false;

        private void Awake()
        {
            _thisTransform = transform;
            _projector = GetComponent<Projector>();
            _drawTargetObject = GetComponent<DrawTargetObject>();

            // set near clip to 0, make it easier to calculate all other stuff
            _projector.nearClipPlane = 0;
        }

        private void Update()
        {
            UpdateJustScale();

            if (_isDirty) {
                _drawTargetObject.SetCommandBufferDirty();
                _isDirty = false;
            }
        }

        [ContextMenu("UpdateJustScale")]
        private void UpdateJustScale()
        {
            #if UNITY_EDITOR
            _thisTransform = transform;
            _projector = GetComponent<Projector>();
            #endif

            // update size based on scale
            var lossyScaleY = _thisTransform.lossyScale.y;
            if (Math.Abs(lossyScaleY - _lastLossyScaleZ) > 1E-6) {
                var factor = lossyScaleY / _lastLossyScaleZ;
                _projector.orthographicSize *= factor;
                _lastLossyScaleZ = lossyScaleY;
            }

            // update far clip pane
            _projector.farClipPlane = 1.05f * projectOverFloor * lossyScaleY;

            // update height based on value
            var newPosition = _thisTransform.localPosition;
            newPosition.y = projectOverFloor;
            _thisTransform.localPosition = newPosition;
        }

        [ContextMenu("SetDirty")]
        public void SetDirty()
        {
            _isDirty = true;
        }
    }
}
