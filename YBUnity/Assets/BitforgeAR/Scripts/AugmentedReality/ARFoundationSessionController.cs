//#define DEBUG_AR
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public class ARFoundationSessionController : MonoBehaviour, IARSessionController
    {
        [SerializeField]
        private ARSessionOrigin arSessionOrigin = null;

        [SerializeField]
        private ARPointCloudManager arPointCloudManager = null;

        [SerializeField]
        private ARPlaneManager arPlaneManager = null;

        [SerializeField]
        private ARReferencePointManager arReferencePointManager = null;

        [SerializeField]
        private ARRaycastManager _raycastManager;

        [SerializeField]
        private Camera arCamera = null;

        [SerializeField]
        private TrackableType trackableType = TrackableType.PlaneWithinPolygon;

        [SerializeField]
        private float scanningHitAngle = 45;

        private readonly List<ARRaycastHit> _arRaycastHits = new List<ARRaycastHit>();
        private readonly List<ARPlane> _arPlanes = new List<ARPlane>();
        private Material _planeMaterial;
        private Tween _planeTween;
        private ARReferencePoint _referencePoint;

        private Material PlaneMaterial
        {
            get
            {
                if (_planeMaterial == null) {
                    _planeMaterial = arPlaneManager.planePrefab.GetComponent<MeshRenderer>().sharedMaterial;
                }

                return _planeMaterial;
            }
        }

        public Camera ARCamera => arCamera;

        private void Awake()
        {
            Debug.Log("ARFoundationSessionController instantiated");
            arPlaneManager.planesChanged += ArPlaneManagerOnPlanesChanged;
            
            
            #if DEBUG_AR
            Debug.Log("Initial AR System State: " + ARSession.state);
            ARSession.stateChanged += args => { Debug.Log("New AR System State: " + args.state); };
            #endif
        }

        public ARSessionState GetARSystemState()
        {
            return ARSession.state;
        }

        public bool HasDetectedPlaneInsight()
        {
            if (arPlaneManager.trackables.count > 0) {
                var referenceDot = ARMathHelper.GetDotProductForAngle(scanningHitAngle);
                var cameraTransform = arCamera.transform;
                var cameraForward = cameraTransform.forward;
                var cameraPosition = cameraTransform.position;

                foreach (var plane in arPlaneManager.trackables) {
                    if (plane.trackingState == TrackingState.Tracking) {
                        var toPlaneForward = (plane.transform.position - cameraPosition).normalized;
                        var dotProduct = Vector3.Dot(toPlaneForward, cameraForward);
                        return dotProduct > referenceDot;
                    }
                }
            }

            return false;
        }

        public void ShowPointCloud()
        {
            if (arPointCloudManager != null) {
                if (arPointCloudManager != null) {
                    //arPointCloudManager.gameObject.SetActive(true);
                }

                arPointCloudManager.enabled = true;
            }
        }

        public void HidePointCloud()
        {
            if (arPointCloudManager != null) {
                if (arPointCloudManager != null) {
                    //arPointCloudManager.gameObject.SetActive(false);
                }

                arPointCloudManager.enabled = false;
            }
        }

        public void ShowPlanes()
        {
            // kill old animation
            _planeTween?.Kill();

            // enable plane manager
            arPlaneManager.enabled = true;

            // fade in material
            _planeTween = PlaneMaterial.DOFade(Mathf.Clamp01(1), 0.25f);
        }

        public void HidePlanes()
        {
            // fade out material
            _planeTween?.Kill();

            var sequence = DOTween.Sequence();
            sequence.Append(PlaneMaterial.DOFade(Mathf.Clamp01(0), 0.25f));
            sequence.AppendCallback(
                () => { arPlaneManager.enabled = true; }
            );
            _planeTween = sequence;
        }

        public bool TryGetArPlaneHit(out IARPlaneHit arPlaneHit)
        {
            var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.4f));
            if (_raycastManager.Raycast(screenCenter, _arRaycastHits, trackableType) && _arRaycastHits.Count > 0) {
                arPlaneHit = new ARFoundationPlaneHit(_arRaycastHits[0], arCamera);
                return true;
            }

            arPlaneHit = null;
            return false;
        }

        public void AttachToReferencePoint(IARPlaneHit arPlaneHit, Transform targetTransform)
        {
            // remove reference point if there is one
            if (_referencePoint != null) {
                DetachFromReferencePoint();
                _referencePoint = null;
            }

            // try to attach hit point to plane
            if (arPlaneHit is ARFoundationPlaneHit arFoundationPlaneHit) {
                var arPlane = arPlaneManager.GetPlane(arFoundationPlaneHit.Hit.trackableId);
                if (arPlane != null) {
                    _referencePoint = arReferencePointManager.AttachReferencePoint(
                        arPlane,
                        arFoundationPlaneHit.HitPose
                    );

                    if (_referencePoint != null) { Debug.Log("Attached object to plane (reference point)"); }
                }
            }

            // try to attach hit point to ar system if plane resolution didn't work
            if (_referencePoint == null) {
                _referencePoint = arReferencePointManager.AddReferencePoint(arPlaneHit.HitPose);

                if (_referencePoint != null) { Debug.Log("Attached object to reference point"); }
            }

            // attach game object to anchor
            if (_referencePoint != null) {
                targetTransform.parent = _referencePoint.transform;
                targetTransform.localRotation = Quaternion.identity;
                targetTransform.localPosition = Vector3.zero;
            }
            else {
                Debug.Log(
                    "Object could not be attached to reference point, and stays under ArSessionOrigin.trackablesParent"
                );

                // simple leave object as underneath trackablesParent
                MakeChildOf(targetTransform, arSessionOrigin.trackablesParent);
            }
        }

        public void DetachFromReferencePoint()
        {
            if (_referencePoint != null) {
                // remove all children under this anchor
                foreach (Transform child in _referencePoint.transform) {
                    MakeChildOf(child, arSessionOrigin.trackablesParent);
                }

                if (_referencePoint != null && arReferencePointManager.RemoveReferencePoint(_referencePoint)) {
                    // destroy anchor
                    var go = _referencePoint.gameObject;
                    go.SetActive(false);
                    Destroy(go);
                }
            }
        }

        public void MoveTransformToPose(Transform content, IARPlaneHit arPlaneHit, bool alignWidthCamera)
        {
            if (alignWidthCamera) { arPlaneHit.AlignHitRotationWithCamera(); }

            // make content child of ar session origin trackables if it isn't
            MakeChildOf(content, arSessionOrigin.trackablesParent);

            content.position = Vector3.Lerp(content.position, arPlaneHit.HitPose.position, 0.5f);
            content.rotation = Quaternion.Lerp(content.rotation, arPlaneHit.HitPose.rotation, 0.5f);
        }

        private static void MakeChildOf(Transform child, Transform parent)
        {
            if (child.parent != parent) { child.SetParent(parent, true); }
        }



        private static void ArPlaneManagerOnPlanesChanged(ARPlanesChangedEventArgs args)
        {
            foreach(var plane in args.removed){
                if (plane != null) {
                    var go = plane.gameObject;
                    if (go != null) {
                        go.SetActive(false);
                        Destroy(go);
                    }
                }
            }
        }
    }
}
