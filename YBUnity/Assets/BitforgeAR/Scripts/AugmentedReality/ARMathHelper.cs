using UnityEngine;

namespace AugmentedReality
{
    // ReSharper disable once InconsistentNaming
    public static class ARMathHelper
    {
        public static float GetDotProductForAngle(float angleDegrees)
        {
            return Vector3.Dot(Vector3.forward, Quaternion.Euler(0, angleDegrees, 0)* Vector3.forward);
        }    
        
    }
}
