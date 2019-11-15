using UnityEngine;

public static class TransformExtension
{
    public static void SetPose(this Transform transform, Pose pose)
    {
        transform.SetPositionAndRotation(pose.position, pose.rotation);    
    }

    public static void SetPoseLerp(this Transform transform, Pose pose, float t)
    {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, pose.position, t), Quaternion.Lerp(transform.rotation, pose.rotation, t));    
    }  
    
    public static void SetPoseSlerp(this Transform transform, Pose pose, float t)
    {
        transform.SetPositionAndRotation(Vector3.Slerp(transform.position, pose.position, t), Quaternion.Slerp(transform.rotation, pose.rotation, t));    
    }  
}



