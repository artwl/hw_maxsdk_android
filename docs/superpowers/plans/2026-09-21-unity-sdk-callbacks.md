# Unity Android SDK 回调更新实施计划

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**目标：** 将对外 Unity 示例的版本号同步到 Android SDK `9.8.78`，并补齐激励视频与插屏新增回调，同时保持旧激励失败事件兼容。

**架构：** 沿用现有 `AndroidJavaProxy` 和 `SynchronizationContext` 主线程转发机制，只在两个代理类增加事件和 Java 方法映射。带错误详情的激励失败回调同时触发新旧两个 C# 事件，避免已有接入方行为改变。

**技术栈：** Unity C#、AndroidJavaProxy、Python 标准库 `unittest` 源码契约测试。

---

### 任务 1：建立源码契约测试并确认 RED

**文件：**
- 新建：`tests/test_unity_sdk_contract.py`
- 检查：`hwsdk_android_unity_example/HwAdsInterface.cs`
- 检查：`hwsdk_android_unity_example/HwAdsListenerProxies.cs`
- 检查：`hwsdk_android_unity_example/README.md`

- [ ] **步骤 1：写失败测试**

测试读取 Unity 示例源码，断言版本号、README 版本、两个收益事件、激励详情失败事件、Java 回调方法，以及详情失败同时触发旧事件。

```python
import pathlib
import re
import unittest


ROOT = pathlib.Path(__file__).resolve().parents[1]
INTERFACE = (ROOT / "hwsdk_android_unity_example/HwAdsInterface.cs").read_text(encoding="utf-8")
PROXIES = (ROOT / "hwsdk_android_unity_example/HwAdsListenerProxies.cs").read_text(encoding="utf-8")
README = (ROOT / "hwsdk_android_unity_example/README.md").read_text(encoding="utf-8")


class UnitySdkContractTests(unittest.TestCase):
    def test_version_is_97878_in_bridge_and_readme(self):
        self.assertRegex(INTERFACE, r'public const string SDKVersion = "9\.8\.78";')
        self.assertIn("版本 9.8.78", README)

    def test_interstitial_exposes_revenue_callback(self):
        interstitial = PROXIES.split("public class HwAdsRewardedVideoProxy", 1)[0]
        self.assertIn("public Action<double> OnAdRevenuePaid;", interstitial)
        self.assertRegex(interstitial, r"void onAdRevenuePaid\(double adRevenue\)")
        self.assertIn("OnAdRevenuePaid?.Invoke(adRevenue)", interstitial)

    def test_rewarded_exposes_revenue_and_detailed_failure_callbacks(self):
        rewarded = PROXIES.split("public class HwAdsRewardedVideoProxy", 1)[1]
        self.assertIn("public Action<double> OnAdRevenuePaid;", rewarded)
        self.assertIn("public Action<int, string> OnLoadFailureWithError;", rewarded)
        self.assertRegex(rewarded, r"void onAdRevenuePaid\(double adRevenue\)")
        self.assertRegex(rewarded, r"void onRewardedVideoLoadFailure\(int errorCode, string errorMessage\)")
        detailed_failure = re.search(
            r"void onRewardedVideoLoadFailure\(int errorCode, string errorMessage\).*?\n    ",
            rewarded,
            re.S,
        )
        self.assertIsNotNone(detailed_failure)
        self.assertIn("OnLoadFailureWithError?.Invoke(errorCode, errorMessage)", detailed_failure.group(0))
        self.assertIn("OnLoadFailure?.Invoke()", detailed_failure.group(0))


if __name__ == "__main__":
    unittest.main()
```

- [ ] **步骤 2：运行测试确认失败**

运行：`python3 -m unittest tests/test_unity_sdk_contract.py -v`

预期：失败，原因是当前桥接仍为 `9.8.61`、README 仍为 `9.8.59`，且代理尚未声明新增回调。

- [ ] **步骤 3：提交 RED 测试**

```bash
git add tests/test_unity_sdk_contract.py
git commit -m "test: define Unity SDK callback contract"
```

### 任务 2：同步版本号并补充示例文档

**文件：**
- 修改：`hwsdk_android_unity_example/HwAdsInterface.cs:14`
- 修改：`hwsdk_android_unity_example/README.md:1`

- [ ] **步骤 1：只更新确认范围内的版本号**

将 `SDKVersion` 改为 `"9.8.78"`，将 README 标题中的版本改为 `9.8.78`；不修改 README 中其他 API 说明。

- [ ] **步骤 2：补充新增回调示例**

在激励和插屏回调章节分别增加 `OnAdRevenuePaid` 订阅示例，在激励回调附近增加：

```csharp
rewardProxy.OnLoadFailureWithError += (errorCode, errorMessage) =>
    Debug.LogError($"激励视频加载失败: {errorCode}, {errorMessage}");
```

并说明原有 `OnLoadFailure` 仍会在同一次详情失败回调中触发。

### 任务 3：实现代理回调的最小兼容映射

**文件：**
- 修改：`hwsdk_android_unity_example/HwAdsListenerProxies.cs:13-88`

- [ ] **步骤 1：补充插屏收益事件和 Java 方法**

在 `HwAdsInterstitialProxy` 中加入：

```csharp
public Action<double> OnAdRevenuePaid;
void onAdRevenuePaid(double adRevenue) => RunOnMainThread(() => OnAdRevenuePaid?.Invoke(adRevenue));
```

- [ ] **步骤 2：补充激励收益事件和详情失败事件**

在 `HwAdsRewardedVideoProxy` 中加入：

```csharp
public Action<double> OnAdRevenuePaid;
public Action<int, string> OnLoadFailureWithError;

void onRewardedVideoLoadFailure(int errorCode, string errorMessage) => RunOnMainThread(() =>
{
    OnLoadFailureWithError?.Invoke(errorCode, errorMessage);
    OnLoadFailure?.Invoke();
});

void onAdRevenuePaid(double adRevenue) => RunOnMainThread(() => OnAdRevenuePaid?.Invoke(adRevenue));
```

旧的 `void onRewardedVideoLoadFailure()` 保持不变。

- [ ] **步骤 3：运行 RED 测试确认变为 GREEN**

运行：`python3 -m unittest tests/test_unity_sdk_contract.py -v`

预期：3 个契约测试全部通过。

- [ ] **步骤 4：提交实现**

```bash
git add hwsdk_android_unity_example/HwAdsInterface.cs hwsdk_android_unity_example/HwAdsListenerProxies.cs hwsdk_android_unity_example/README.md
git commit -m "feat: update Unity SDK callbacks to 9.8.78"
```

### 任务 4：完成验证

**文件：** 无新增修改。

- [ ] **步骤 1：运行完整源码契约测试**

运行：`python3 -m unittest discover -s tests -v`

预期：所有测试通过，退出码为 `0`。

- [ ] **步骤 2：检查语法和差异**

运行：`git diff --check` 和 `git show --stat --oneline HEAD`。

预期：无空白错误；当前实现提交只包含本次 Unity 示例文件和对应的契约测试。

- [ ] **步骤 3：核对工作区**

运行：`git status --short --branch`。

预期：工作区干净，且没有生成的构建产物或缓存文件。
