using System;
using UnityEngine;

namespace CameraHelper
{
    [RequireComponent(typeof(Collider))]
    public class BlurTrigger : MonoBehaviour
    {
        public BlurOnCollisionPostProcessing blurOnCollisionPostProcessing;

        private const string VALID_TAG = "blurcollider";
        private int _activeTriggerCount;
        private bool _triggerStay;
        private bool _blurEnabled;

        private void OnTriggerEnter(Collider other)
        {
            if (ColliderIsValid(other)) { _activeTriggerCount++; }
        }

        private void OnTriggerStay(Collider other)
        {
            _triggerStay |= ColliderIsValid(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (ColliderIsValid(other)) { _activeTriggerCount--; }
        }

        private bool ColliderIsValid(Collider colliderToCheck)
        {
            return colliderToCheck != null && colliderToCheck.tag.Equals(VALID_TAG, StringComparison.OrdinalIgnoreCase);
        }

        private void Update()
        {
            var newBlurEnabled = _triggerStay || _activeTriggerCount > 0;
            if (newBlurEnabled != _blurEnabled) {
                _blurEnabled = newBlurEnabled;
                blurOnCollisionPostProcessing.enabled = _blurEnabled;
            }

            _triggerStay = false;
        }
    }
}
