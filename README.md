```markdown
# HwAds SDK Unity 接入说明 (版本 9.8.59)

本文档说明如何在 Unity 环境中接入 HwAds SDK 的 Android 原生广告和打点功能。
本版本使用了最新的 `AndroidJavaProxy` 代理模式，**无需在场景中挂载任何特殊的 GameObject，且内部已自动处理主线程同步回调**，研发同学可直接在代码里进行无缝对接及 UI 操作。

## 1. 导入库文件

1. 将原生的 Android 插件（如 `.jar` 或 `.aar` 及其依赖）放置在 Unity 工程的 `Assets/Plugins/Android` 目录下。
2. 将提供的 `HwAdsInterface.cs` 和 `HwAdsListenerProxies.cs` 放入工程的 `Assets/Scripts` 目录下即可。

---

## 2. API 使用说明

所有 API 均位于 `HwAdsInterface` 静态类中。已内置平台检测，在 Windows / Mac 编辑器及 iOS 环境下调用会自动 return，不会报错。

### 1. 初始化前：数数 (ThinkingAnalytics) 与 Adjust 关联打点 (强制顺序)

如果你的项目接入了数数平台 (ThinkingAnalytics)，为了让数数能成功关联 Adjust 数据，**必须严格按照以下顺序执行，必须在 HwAds SDK 初始化之前完成，否则会关联失败！**

```csharp
// 【第一步】：初始化数数 SDK
TDConfig config = new TDConfig("APPID", "SERVER");
TDAnalytics.Init(config);

// 【第二步】：获取 TE(数数) 的访客 ID，对应 TE 中的 #distinct_id
var distinctId = TDAnalytics.GetDistinctId();

// 【第三步】：将访客 ID 设置到 adjust 采集事件中
// 参数1: ta_distinct_id
// 参数2: ta_account_id （如果没有获取到 accountId，可以直接传空字符串 ""）
HwAdsInterface.HwAdjustAddTa_distinct_id(distinctId, ""); 

// ---------- 此后才能进行第四步：初始化 HwAds SDK ----------
```

### 2. 初始化 HwAds SDK (必接)

在完成上述数数关联（如果有）之后，在游戏启动的首个场景（`Awake` 或 `Start` 中）尽早调用本 SDK 的初始化：

```csharp
// gameBrainId: 游戏ID
// appToken: adjust平台 Token
// isFirebaseOK: "yes" / "no"
// isABTestOpen: "yes" / "no"
// isMerge: "yes" / "no"

HwAdsInterface.InitSDK("your_game_brain_id", "your_adjust_Apptoken", "yes", "no", "no");

// 或者带 Banner 背景色的初始化
// HwAdsInterface.InitSDK("your_game_brain_id", "your_token", "yes", 0xFFFFFF, "no", "no");
```

### 3. 激励视频广告回调设置 (极其重要)

由于使用了最新的代理机制，监听广告生命周期只需实例化 `HwAdsRewardedVideoProxy` 并挂载事件即可。
**重要：事件内部已自动切回 Unity 主线程，支持直接操作 UI。**

为防止奖励弹窗与广告关闭动画重叠导致卡顿，**强烈建议采用“完成时标记，关闭时发奖”的最佳实践**。

```csharp
// 标记位：记录玩家是否真正看完了视频
private bool isRewardEarned = false;

void Start()
{
    // 1. 创建激励视频代理
    HwAdsRewardedVideoProxy rewardProxy = new HwAdsRewardedVideoProxy();

    // --- 绑定核心事件 ---
    
    rewardProxy.OnLoadSuccess += () => {
        Debug.Log("[回调] 激励视频加载成功");
        // 建议：通知 UI 层点亮对应标签的“免费获取”按钮
    };

    rewardProxy.OnStarted += () => {
        Debug.Log("[回调] 激励视频开始播放展示");
        // 【强制要求】：在此处通知游戏暂停画面及背景音乐
        // Time.timeScale = 0; AudioListener.pause = true; 等
        isRewardEarned = false; // 每次开始前重置标记
    };

    rewardProxy.OnCompleted += () => {
        Debug.Log("[核心回调] 用户已完整看完激励视频，达成奖励条件");
        // 最佳实践：在此处仅设置标记位，待在 OnClosed 中统一发奖
        isRewardEarned = true; 
    };

    rewardProxy.OnClosed += () => {
        Debug.Log("[回调] 激励视频页面被关闭（无论是否看完都会触发）");
        
        // 【核心逻辑处理】：
        // 1. 通知游戏恢复画面及背景音乐
        // Time.timeScale = 1; AudioListener.pause = false; 

        // 2. 检查 OnCompleted 中设置的标记位，若为 true，则通知引擎层发放奖励
        if (isRewardEarned) 
        {
            Debug.Log(">>> 发放金币奖励，播放特效！ <<<");
            // ExecuteRewardLogic();
            isRewardEarned = false; // 清空标记
        }
    };
    
    // --- 其他辅助事件 ---
    rewardProxy.OnLoadFailure += () => Debug.Log("[回调] 激励视频加载失败");
    rewardProxy.OnPlaybackError += () => Debug.Log("[回调] 激励播放过程中出现错误");
    rewardProxy.OnClicked += () => Debug.Log("[回调] 广告被用户点击");

    // 3. 注册给系统 (只需注册一次)
    HwAdsInterface.SetHwAdsRewardedVideoListener(rewardProxy);
}
```

### 4. 激励视频展示及点击打点 (展示入口)

当玩家点击了 UI 上的“看视频获取奖励”按钮时，**务必先调用 `TrackRewardButtonClick` 记录点位，再请求展示广告。**

```csharp
public void ClickShowRewardButton()
{
    string tag = "double"; // 激励点位标签，例如："double", "revive" 等
    
    // 第一步：记录激励按钮点击事件（必调，无需等待加载结果）
    HwAdsInterface.TrackRewardButtonClick(tag);

    // 第二步：请求展示激励视频广告，参数需与上方标签保持一致
    // 建议在展示前可判断下有没有缓存：
    if(HwAdsInterface.IsRewardLoad()) {
        HwAdsInterface.ShowReward(tag); 
    } else {
        Debug.LogWarning("视频还未准备好，请稍后");
    }
}
```

### 5. 插屏广告回调与展示

插屏通常用于过关/暂停等节点，逻辑与激励视频相似，只需绑定 `HwAdsInterstitialProxy`：

```csharp
void Start()
{
    HwAdsInterstitialProxy interProxy = new HwAdsInterstitialProxy();
    
    interProxy.OnDismissed += (isFbOrAdmob) => {
        Debug.Log("插屏广告被关闭，isFbOrAdmob = " + isFbOrAdmob);
        // 可在这里继续游戏逻辑（恢复音乐/画面）
    };

    // 其他事件：OnLoaded, OnFailed, OnShown, OnClicked 可酌情订阅

    HwAdsInterface.SetHwAdsInterstitialListener(interProxy);
}

public void ShowInterstitial()
{
    if (HwAdsInterface.IsInterLoad())
    {
        HwAdsInterface.ShowInter();
    }
}
```

### 6. 关键打点接入与内购验证

#### 6.1 基础与高级打点
```csharp
// 基础事件打点
HwAdsInterface.HwAnalyticsUserNew("actionToken", "category", "action", "label");

// AppLovin 高级事件打点（支持多级字典）
var paramMap = new Dictionary<string, string> { { "item_id", "1001" }, { "price", "0.99" } };
HwAdsInterface.TrackAppLovinEvent("purchase_item", paramMap);
```

#### 6.2 核心：内购打点与二次验证 (IAP)
当玩家发起内购成功后，务必调用此接口完成验证与打点。**请严格遵循参数说明格式化金额。**

**接口调用：**
```csharp
HwAdsInterface.HwAnalyticsPurchaseSecondVerify(
    category, numberStr, currency, purchaseToken, productId, purchaseType, orderId, adjustDifferentPurchaseToken);
```
**参数详解与避坑指南：**
*   `category`: 购买 key 值。**必须固定传 `"HwPurchase"`**。
*   `number`: 内购本地化金额（`string` 类型）。
    *   **获取要求**：请使用内购返回的 `priceAmountMicros`（微分单位值），转换成标准金额：`priceAmountMicros / 1000000.0`。
    *    **极其重要：在将浮点金额 `ToString()` 传递时，必须使用 `CultureInfo.InvariantCulture`，以避免在欧盟等地区（使用逗号 `,` 代替小数点）发生严重解析和校验失败！**
    *   写法示例：`price.ToString(System.Globalization.CultureInfo.InvariantCulture)`。
*   `currency`: 本地化单位（例如："USD", "EUR"）。
*   `purchaseToken`: Google / Apple 订单购买完成后返回的购买校验 Token。
*   `productId`: 你在后台配置的商品 SKU ID。
*   `purchaseType`: 内购商品类型。`1` 是订阅(Subscription)，`0` 是普通消费商品(Consumable)。
*   `orderId`: 订单凭证流水号 ID。
*   `adjustDifferentPurchaseToken`: 直接传入空字符串 `""` 即可。

### 7. 欧盟隐私弹窗 (GDPR / UMP)

如果需要为欧洲地区玩家展示隐私合规弹窗或入口：
```csharp
// 判断当前环境是否需要显示隐私设置按钮
bool needPrivacyBtn = HwAdsInterface.IsPrivacySettingsButtonEnabled();
if (needPrivacyBtn)
{
    HwAdsInterface.PresentCMPForm();
}
```

---
## 联系支持
如果有任何接口调用、逻辑疑问或打包报错，请及时联系对接人员。
