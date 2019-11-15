using UnityEngine;

public static class QualityController
{
    public static void SlowDownUnity()
    {
        Debug.Log("SlowDown Unity");

        #if !UNITY_EDITOR
        Application.targetFrameRate = 6;    // 166ms // set target framerate to 6
        QualitySettings.vSyncCount = 0;     // don't use vsync
        #endif

        Time.fixedDeltaTime = 1f;           // 1 fps slow down physic update
    }

    public static void SpeedUpUnity()
    {
        Debug.Log("SpeedUp Unity");

        #if !UNITY_EDITOR
        Application.targetFrameRate = 60;   // set target framerate to 1
        QualitySettings.vSyncCount = 0;     // don't use vsync
        #endif

        Time.fixedDeltaTime = 0.05f;        // 20 fps // speed up physic update (needed by unity ui)
    }
}
