using UnityEngine;

public static class OrientationController
{
    public static void UnlockOrientations()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = true;
    }

    public static void LockOrientations()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = false;
    }

    public static void UsePortrait()
    {
        Debug.Log("SHOW PORTRAIT");
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public static void UseLandscape()
    {
        Debug.Log("SHOW LANDSCAPE");
        Screen.orientation = ScreenOrientation.Landscape;
    }

}
