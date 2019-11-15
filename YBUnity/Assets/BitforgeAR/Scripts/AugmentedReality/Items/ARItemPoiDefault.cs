using System;
using AugmentedReality.Poi;
using UnityEngine;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable InconsistentNaming

namespace AugmentedReality.Items
{
    public class ARItemPoiDefault : ARItemDefault
    {
        private BasePoi[] pois;
        private float raycastOffset;
        private int _poiLayerMask;
        private readonly RaycastHit[] _raycastHits = new RaycastHit[6];

        protected override void Awake()
        {
            base.Awake();

            // limit layer mask, for gazing physic calculation
            _poiLayerMask = LayerMask.GetMask("POI");

            // find all interaction points inside
            pois = GetComponentsInChildren<BasePoi>();

            // calculate sphere collider radius
            if (pois.Length > 0) {
                var sphereCollider = pois[0].GetComponent<SphereCollider>();
                if (sphereCollider != null) { raycastOffset = sphereCollider.radius; }
            }
        }

        

        public override void InitAndHide()
        {
            HidePois(true);
            base.InitAndHide();
        }

        public override void ShowFullObject()
        {
            ShowPois();
            base.ShowFullObject();
        }

        public void ShowPois()
        {
            foreach (var poi in pois) { poi.Show(); }
        }

        public void HidePois(bool immediate)
        {
            foreach (var poi in pois) { poi.Hide(immediate); }
        }

        public void TogglePoiVisibility()
        {
            if (pois.Length > 0) {
                if (pois[0].enabled) { HidePois(false); }
                else { ShowPois(); }
            }
        }
        

        public BasePoi RaycastPoi(Ray ray, float distance = 10)
        {
            // shift backwards the origin the compensate the collider radius
            ray.origin -= raycastOffset * transform.lossyScale.x * ray.direction;

            //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            var resultCount = Physics.RaycastNonAlloc(ray, _raycastHits, distance, _poiLayerMask);

            if (resultCount == 1) {
                // take the one found when it fits
                var poi = _raycastHits[0].transform.GetComponent<BasePoi>();
                if (!ReferenceEquals(poi, null)) { return poi; }
            }

            if (resultCount > 1) {
                // try to find the collision poi in the center when there are more than one
                BasePoi centeredPoi = null;

                // find the hit with the lowest
                var maxDot = float.MinValue;
                foreach (var hit in _raycastHits) {
                    if (hit.transform != null) {
                        var poi = hit.transform.GetComponent<BasePoi>();
                        if (!ReferenceEquals(poi, null)) {
                            if (centeredPoi == null) {
                                centeredPoi = poi;
                                maxDot = CalculateNormalizedDot(hit, ray);
                            }
                            else {
                                var newMaxDot = CalculateNormalizedDot(hit, ray);
                                if (newMaxDot > maxDot) {
                                    centeredPoi = poi;
                                    maxDot = newMaxDot;
                                }
                            }
                        }
                    }
                }

                return centeredPoi;
            }

            return null;
        }

        protected override void UpdateScale(float scale)
        {
            base.UpdateScale(scale);
            foreach (var poi in pois) { poi.UpdateScale(scale); }
        }

        private static float CalculateNormalizedDot(RaycastHit raycastHit, Ray ray)
        {
            var raycastForward = raycastHit.transform.position - ray.origin;
            raycastForward.Normalize();
            return Vector3.Dot(ray.direction, raycastForward);
        }
    }
}
