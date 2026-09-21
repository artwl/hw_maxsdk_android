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
            r"void onRewardedVideoLoadFailure\(int errorCode, string errorMessage\).*?"
            r"(?=\n    void onRewardedVideoStarted)",
            rewarded,
            re.S,
        )
        self.assertIsNotNone(detailed_failure)
        self.assertIn("OnLoadFailureWithError?.Invoke(errorCode, errorMessage)", detailed_failure.group(0))
        self.assertIn("OnLoadFailure?.Invoke()", detailed_failure.group(0))


if __name__ == "__main__":
    unittest.main()
