using UnityEngine;

// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public class ARPlaneHit : IARPlaneHit
    {
        public Pose CameraPose { get; }

        public Pose HitPose { get; private set; }

        public ARPlaneHit(Pose hitPose, Camera camera)
        {
            HitPose = hitPose;
            var cameraTransform = camera.transform;
            CameraPose = new Pose(cameraTransform.position, cameraTransform.rotation);
        }

        public void AlignHitRotationWithCamera()
        {
            var cameraForward = CameraPose.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            HitPose = new Pose(HitPose.position, Quaternion.LookRotation(cameraBearing));
        }
    }
}
