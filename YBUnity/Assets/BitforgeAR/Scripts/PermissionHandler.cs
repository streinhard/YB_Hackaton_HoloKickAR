#if PLATFORM_ANDROID
using UnityEngine.Android;

#endif

public static class PermissionHandler
{
    public static void RequestLocationPermission()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) { Permission.RequestUserPermission(Permission.FineLocation); }
        #endif
    }

    public static void RequestCameraPermission()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) { Permission.RequestUserPermission(Permission.Camera); }
        #endif
    }

    public static bool HasCameraPermission()
    {
        #if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.Camera);
        #else
        return true;
        #endif
    }
}
