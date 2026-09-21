# Unity Android SDK 回调更新设计

## 范围

仅针对本次确认的内容，将对外发布的 Unity 示例同步到本地 Android SDK
`9.8.78`。其他已存在但未确认的 API 差异保持不变。

## 变更内容

- 将 `HwAdsInterface.SDKVersion` 从 `9.8.61` 改为 `9.8.78`。
- 将 Unity 示例 README 中的版本号从 `9.8.59` 改为 `9.8.78`。
- 在插屏代理中新增 `OnAdRevenuePaid(double revenue)`。
- 在激励视频代理中新增 `OnAdRevenuePaid(double revenue)`。
- 在激励视频代理中新增 `OnLoadFailureWithError(int errorCode, string errorMessage)`，
  对应 Android SDK 新增的带错误详情的失败回调重载。
- 收到带错误详情的激励加载失败回调时，同时触发新的详情事件和原有的
  `OnLoadFailure` 事件。这样既暴露错误信息，也保持现有 Unity 项目的兼容性。
- 在 Unity 示例 README 中补充这几个回调的用法说明。

## 回调流程

Android SDK 调用 `HwAdsInterstitialListener` 或 `HwAdsRewardVideoListener` Java
代理，C# 的 `AndroidJavaProxy` 通过现有代理构造时捕获的
`SynchronizationContext` 转发回调，因此事件订阅者仍在 Unity 主线程执行。

收益回调直接转发 Java 的 `double` 值。激励视频的带详情失败回调转发 Java
的 `int` 和 `String` 参数，并额外触发无参数的旧失败事件。

## 兼容性边界

本次不新增 Banner 监听代理、用户邮箱接口、用户分层接口、微单位内购接口，
也不处理其他 Android 原生 API 差异。现有事件名称和无参数的激励失败事件保持有效。

## 验证方式

- 增加源码契约测试，检查版本号、README 版本号、代理回调方法、事件声明和旧失败事件的兼容转发。
- 按测试先行流程先验证测试失败，再实现代码并验证通过。
- 在当前非完整 Unity 工程中运行可用的仓库检查，并检查最终 diff，确保没有无关改动。
