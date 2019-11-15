
using UnityEngine;

public static class StoreHelper
{
    // TODO: Replace with actual PlayStore id
    private const string ANDROID_STORE_URL = "https://play.google.com/store/apps/details?id=li.listory.app";
    // TODO: Replace with actual AppStore id
    private const string IOS_STORE_URL = "https://itunes.apple.com/us/app/liechtenstein-tourismus/id1462266258";

    public static void AppRate()
    {
        #if UNITY_ANDROID
        Application.OpenURL("market://details?id=" + Application.identifier);
        #elif UNITY_IOS
        Application.OpenURL(IOS_STORE_URL);
        #endif
    }

    public static void AppRecommend()
    {
        var storeUrl = Application.platform == RuntimePlatform.Android ? ANDROID_STORE_URL : IOS_STORE_URL;

        // TODO: Translate to english
        new NativeShare()
            .SetSubject(Localization.GetText("unity_recommend_title"))
            .SetText(Localization.GetText("unity_recommend_text") + "\n" + storeUrl)
            .Share();
    }
}

