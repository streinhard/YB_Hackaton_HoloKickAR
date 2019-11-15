using UnityEngine;
using UnityEngine.XR.ARFoundation;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public class ARDebugSessionController : MonoBehaviour, IARSessionController
    {
        [SerializeField]
        private Camera debugCamera = null;

        [SerializeField]
        private GameObject debugPlane = null;

        public Camera ARCamera => debugCamera;

        private LayerMask floorLayerMask;

        private void Awake()
        {
            Debug.Log("ARDebugSessionController instantiated");
            floorLayerMask = LayerMask.GetMask("Water");
        }

        public ARSessionState GetARSystemState()
        {
            return ARSessionState.Ready;
        }

        public bool HasDetectedPlaneInsight()
        {
            if (debugCamera == null) return false;

            var referenceDot = ARMathHelper.GetDotProductForAngle(30);
            var cameraTransform = debugCamera.transform;
            var cameraForward = cameraTransform.forward;
            var toPlaneForward = (debugPlane.transform.position - cameraTransform.position).normalized;
            var dotProduct = Vector3.Dot(toPlaneForward, cameraForward);

            //Debug.Log(referenceDot + " vs. " + dotProduct + " = "  + (dotProduct > referenceDot));

            return dotProduct > referenceDot;
        }

        public void ShowPointCloud()
        {
            Debug.Log("ShowPointCloud (not implemented)");
        }

        public void HidePointCloud()
        {
            Debug.Log("HidePointCloud (not implemented)");
        }

        public void ShowPlanes()
        {
            debugPlane.GetComponent<MeshRenderer>().enabled = true;
        }

        public void HidePlanes()
        {
            debugPlane.GetComponent<MeshRenderer>().enabled = false;
        }

        public bool TryGetArPlaneHit(out IARPlaneHit arPlaneHit)
        {
            var cameraTransform = debugCamera.transform;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hitInfo, 100f, floorLayerMask)) {
                arPlaneHit = new ARPlaneHit(new Pose(hitInfo.point, hitInfo.transform.rotation), debugCamera);
                return true;
            }

            arPlaneHit = null;
            return false;
        }

        public void AttachToReferencePoint(IARPlaneHit arPlaneHit, Transform targetTransform)
        {
            // create anchor
            var anchorPointTransform = new GameObject("DebugAnchor").transform;
            anchorPointTransform.SetPose(arPlaneHit.HitPose);

            // attach game object to anchor
            targetTransform.parent = anchorPointTransform;
            targetTransform.localRotation = Quaternion.identity;
            targetTransform.localPosition = Vector3.zero;
        }

        public void DetachFromReferencePoint() { }

        public void MoveTransformToPose(Transform content, IARPlaneHit arPlaneHit, bool alignWidthCamera)
        {
            if (alignWidthCamera) { arPlaneHit.AlignHitRotationWithCamera(); }

            content.position = arPlaneHit.HitPose.position;
            content.rotation = arPlaneHit.HitPose.rotation;
        }
    }
}
