using UnityEngine;
using UnityEngine.XR.ARFoundation;
// ReSharper disable InconsistentNaming

namespace AugmentedReality
{
    public interface IARSessionController
    {
        Camera ARCamera { get; }
        
        ARSessionState GetARSystemState();
        bool HasDetectedPlaneInsight();
        
        void ShowPointCloud();
        void HidePointCloud();

        void ShowPlanes();
        void HidePlanes();

        bool TryGetArPlaneHit(out IARPlaneHit arPlaneHit);
        
        void AttachToReferencePoint(IARPlaneHit arPlaneHit, Transform targetTransform);
        void DetachFromReferencePoint();
        void MoveTransformToPose(Transform content, IARPlaneHit arPlaneHit, bool alignWidthCamera);
    }
}
