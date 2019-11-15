using UnityEngine;
// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public interface IARPlaneHit
    {
        Pose CameraPose { get; }
        Pose HitPose { get; }
        void AlignHitRotationWithCamera();
    }
}
