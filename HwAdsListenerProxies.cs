using UnityEngine;
using System;
using System.Threading;

#region ========== SDK 回调代理类 (AndroidJavaProxy) ==========

/// <summary>
/// 插屏广告回调代理 
/// 映射 Java 接口: com.hw.hwadssdk.HwAdsInterstitialListener
/// </summary>
public class HwAdsInterstitialProxy : AndroidJavaProxy
{
    public Action OnLoaded;
    public Action OnFailed;
    public Action OnShown;
    public Action OnClicked;
    public Action<bool> OnDismissed;

    // 获取当前 Unity 主线程的上下文
    private SynchronizationContext mainThreadContext;

    public HwAdsInterstitialProxy() : base("com.hw.hwadssdk.HwAdsInterstitialListener") 
    {
        // 极其重要：由于实例化往往发生在主线程，我们在构造时抓取主线程 Context
        mainThreadContext = SynchronizationContext.Current; 
    }

    private void RunOnMainThread(Action action)
    {
        if (mainThreadContext != null)
        {
            mainThreadContext.Post(_ => action?.Invoke(), null);
        }
        else
        {
            action?.Invoke(); // 兜底：如果抓不到上下文就硬着头皮调
        }
    }

    // 下面方法的命名和参数列表必须与 Java 里的接口方法完全一致
    void onInterstitialLoaded() => RunOnMainThread(() => OnLoaded?.Invoke());
    void onInterstitialFailed() => RunOnMainThread(() => OnFailed?.Invoke());
    void onInterstitialShown() => RunOnMainThread(() => OnShown?.Invoke());
    void onInterstitialClicked() => RunOnMainThread(() => OnClicked?.Invoke());
    void onInterstitialDismissed(bool isFborAdmob) => RunOnMainThread(() => OnDismissed?.Invoke(isFborAdmob));
}

/// <summary>
/// 激励视频广告回调代理
/// 映射 Java 接口: com.hw.hwadssdk.HwAdsRewardVideoListener
/// </summary>
public class HwAdsRewardedVideoProxy : AndroidJavaProxy
{
    public Action OnLoadSuccess;
    public Action OnLoadFailure;
    public Action OnStarted;
    public Action OnPlaybackError;
    public Action OnClicked;
    public Action OnClosed;
    public Action OnCompleted; 
    
    private SynchronizationContext mainThreadContext;

    public HwAdsRewardedVideoProxy() : base("com.hw.hwadssdk.HwAdsRewardVideoListener") 
    {
        mainThreadContext = SynchronizationContext.Current;
    }

    private void RunOnMainThread(Action action)
    {
        if (mainThreadContext != null)
        {
            mainThreadContext.Post(_ => action?.Invoke(), null);
        }
        else
        {
            action?.Invoke();
        }
    }

    void onRewardedVideoLoadSuccess() => RunOnMainThread(() => OnLoadSuccess?.Invoke());
    void onRewardedVideoLoadFailure() => RunOnMainThread(() => OnLoadFailure?.Invoke());
    void onRewardedVideoStarted() => RunOnMainThread(() => OnStarted?.Invoke());
    void onRewardedVideoPlaybackError() => RunOnMainThread(() => OnPlaybackError?.Invoke());
    void onRewardedVideoClicked() => RunOnMainThread(() => OnClicked?.Invoke());
    void onRewardedVideoClosed() => RunOnMainThread(() => OnClosed?.Invoke());
    void onRewardedVideoCompleted() => RunOnMainThread(() => OnCompleted?.Invoke());
}

#endregion