using UnityEngine;
using UnityEngine.XR.ARFoundation;

// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public class ARFoundationPlaneHit : ARPlaneHit
    {
        public ARRaycastHit Hit { get; }

        public ARFoundationPlaneHit(ARRaycastHit arRaycastHit, Camera camera) : base(arRaycastHit.pose, camera)
        {
            Hit = arRaycastHit;
        }
    }
}
