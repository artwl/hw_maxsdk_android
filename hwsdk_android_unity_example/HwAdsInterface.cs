using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// HwAds SDK Unity 桥接接口
/// 对应 Android 端的 com.hw.hwadssdk.HwAdsInterface
/// </summary>
public class HwAdsInterface
{
    private const string JAVA_CLASS_NAME = "com.hw.hwadssdk.HwAdsInterface";
    private static AndroidJavaClass _hwAdsClass;

    // --- 静态常量值 ---
    public const string SDKVersion = "9.8.61";

    private static AndroidJavaClass HwAdsClass
    {
        get
        {
            if (_hwAdsClass == null)
            {
                _hwAdsClass = new AndroidJavaClass(JAVA_CLASS_NAME);
            }
            return _hwAdsClass;
        }
    }

    /// <summary>
    /// 获取当前 Unity Activity (Context)
    /// </summary>
    private static AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }

    #region ========== SDK 初始化 ==========

    public static void InitSDK(string gameBrainId, string appToken, string isFirebaseOK, string isABTestOpen, string isMerge)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("initSDK", GetUnityActivity(), gameBrainId, appToken, isFirebaseOK, isABTestOpen, isMerge);
    }

    public static void InitSDK(string gameBrainId, string appToken, string isFirebaseOK, int bannerBgColor, string isABTestOpen, string isMerge)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("initSDK", GetUnityActivity(), gameBrainId, appToken, isFirebaseOK, bannerBgColor, isABTestOpen, isMerge);
    }

    #endregion

    #region ========== Banner 广告 ==========

    public static void ShowBanner()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("showBanner");
    }

    public static void HideBanner()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("hideBanner");
    }

    public static bool IsBannerLoad()
    {
        if (Application.platform != RuntimePlatform.Android) return false;
        return HwAdsClass.CallStatic<bool>("isBannerLoad");
    }

    public static int GetBannerHeightPx()
    {
        if (Application.platform != RuntimePlatform.Android) return 0;
        return HwAdsClass.CallStatic<int>("getBannerHeightPx");
    }

    #endregion

    #region ========== 插屏广告 ==========

    public static void ShowInter()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("showInter");
    }

    public static bool IsInterLoad()
    {
        if (Application.platform != RuntimePlatform.Android) return false;
        return HwAdsClass.CallStatic<bool>("isInterLoad");
    }

    public static bool IsFacebookInter()
    {
        if (Application.platform != RuntimePlatform.Android) return false;
        return HwAdsClass.CallStatic<bool>("isFacebookInter");
    }

    #endregion

    #region ========== 激励视频广告 ==========

    public static void ShowReward(string rewardTag)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("showReward", rewardTag);
    }

    public static bool IsRewardLoad()
    {
        if (Application.platform != RuntimePlatform.Android) return false;
        return HwAdsClass.CallStatic<bool>("isRewardLoad");
    }

    public static void TrackRewardButtonClick(string adPointName)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("trackRewardButtonClick", adPointName);
    }

    #endregion

    #region ========== 打点与归因 (Analytics) ==========

    public static void HwAdjustAddTa_distinct_id(string ta_distinct_id, string ta_account_id)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("hwAdjustAddTa_distinct_id", ta_distinct_id, ta_account_id);
    }

    public static void HwAnalyticsPurchaseSecondVerify(string category, string number, string currency, string purchaseToken, string productId, int purchaseType, string orderId, string adjustDifferentPurchaseToken)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwAnalyticsPurchaseSecondVerify", category, number, currency, purchaseToken, productId, purchaseType, orderId, adjustDifferentPurchaseToken);
    }

    public static void HwAnalyticsUserFBValue(string fbEmail, string fb_login_id)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwAnalyticsUserFBValue", fbEmail, fb_login_id);
    }

    public static void HwAdjustTrackWithFBValue(string eventToken, string fbEmail, string fb_login_id)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwAdjustTrackWithFBValue", eventToken, fbEmail, fb_login_id);
    }

    public static void HwSplashSceenAdAnalytic()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwSplashSceenAdAnalytic");
    }

    public static void HwAnalyticsUserNew(string actionToken, string category, string action, string label)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwAnalyticsUserNew", actionToken, category, action, label);
    }

    public static void HwFacebookAnalytic(string category, string action, string label)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwFacebookAnalytic", category, action, label);
    }

    // AppLovin 高级事件打点对接 (支持电商参数字典)
    public static void TrackAppLovinEvent(string eventName, Dictionary<string, string> parameters)
    {
        if (Application.platform != RuntimePlatform.Android) return;

        // 将 C# 的 Dictionary 转换为 Java 的 HashMap
        using (AndroidJavaObject hashMap = new AndroidJavaObject("java.util.HashMap"))
        {
            System.IntPtr putMethod = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            object[] putArgs = new object[2];

            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    using (AndroidJavaObject key = new AndroidJavaObject("java.lang.String", kvp.Key))
                    using (AndroidJavaObject val = new AndroidJavaObject("java.lang.String", kvp.Value))
                    {
                        putArgs[0] = key;
                        putArgs[1] = val;
                        AndroidJNI.CallObjectMethod(hashMap.GetRawObject(), putMethod, AndroidJNIHelper.CreateJNIArgArray(putArgs));
                    }
                }
            }
            HwAdsClass.CallStatic("trackAppLovinEvent", eventName, hashMap);
        }
    }

    #endregion

    #region ========== AB测试及状态 ==========

    public static string HwABTestValue()
    {
        if (Application.platform != RuntimePlatform.Android) return "";
        return HwAdsClass.CallStatic<string>("HwABTestValue");
    }

    public static string HwFirebaseABTestValue()
    {
        if (Application.platform != RuntimePlatform.Android) return "";
        return HwAdsClass.CallStatic<string>("HwFirebaseABTestValue");
    }

    public static void HwAnalyticsUserABTestState(string userABTestState)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("HwAnalyticsUserABTestState", userABTestState);
    }

    public static string HwAttributionValue()
    {
        if (Application.platform != RuntimePlatform.Android) return "0"; // 默认为0
        return HwAdsClass.CallStatic<string>("HwAttributionValue");
    }

    public static void SetABTestValue(string externalABTestValue)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("setABTestValue", externalABTestValue);
    }

    #endregion

    #region ========== GDPR / UMP 及其他 ==========

    public static void UMPDebugSetting(string testDeviceIdentifiers)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("UMPDebugSetting", testDeviceIdentifiers);
    }

    public static void RequestCMP()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("requestCMP");
    }

    public static bool IsPrivacySettingsButtonEnabled()
    {
        if (Application.platform != RuntimePlatform.Android) return false;
        return HwAdsClass.CallStatic<bool>("isPrivacySettingsButtonEnabled");
    }

    public static void PresentCMPForm()
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("presentCMPForm");
    }

    public static void SetRemoveAdsStatus(bool hasRemoveAds)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("setRemoveAdsStatus", hasRemoveAds);
    }

    public static void SetHotfix_version(string hotfix_version)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        HwAdsClass.CallStatic("setHotfix_version", hotfix_version);
    }

    #endregion
    
    #region ========== 回调监听器 (Callback Listeners) ==========

    // 缓存持有的代理，防止被C#垃圾回收掉
    private static HwAdsInterstitialProxy _interstitialProxy;
    private static HwAdsRewardedVideoProxy _rewardedVideoProxy;

    /// <summary>
    /// 设置插屏广告监听
    /// </summary>
    public static void SetHwAdsInterstitialListener(HwAdsInterstitialProxy listenerProxy)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        
        _interstitialProxy = listenerProxy; // 缓存起来以免被 GC

        // 将 Proxy 注册给 Android
        HwAdsClass.CallStatic("setHwAdsInterstitialListener", _interstitialProxy);
    }

    /// <summary>
    /// 设置激励视频广告监听
    /// </summary>
    public static void SetHwAdsRewardedVideoListener(HwAdsRewardedVideoProxy listenerProxy)
    {
        if (Application.platform != RuntimePlatform.Android) return;
        
        _rewardedVideoProxy = listenerProxy; // 缓存起来以免被 GC

        // 将 Proxy 注册给 Android
        HwAdsClass.CallStatic("setHwAdsRewardedVideoListener", _rewardedVideoProxy);
    }

    #endregion

}