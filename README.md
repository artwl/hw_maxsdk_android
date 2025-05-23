# hwsdk_android

本文档是Android版变现SDK，当前版本 `V9.8.18`， <b>  建议接SDK，就接最新的版本 </B>

## 下载地址

SDK 下载地址：[v.9.8.18](https://github.com/artwl/hw_maxsdk_android/releases)

## 接入文档

接入请参考：[SDK接入文档,飞书文档](https://hellowd.feishu.cn/docs/doccnJOWCBfsHiAGPmkFeNg3D2f)

## 需要帮助？

请先查看接入文档和常见问题，还有问题可联系对接人寻求技术支持

## 本版特性 (9.8.18 - 2025年4月)

详细内容请查看更新记录，有完整的更新内容列表。
- **9.8.18 新特性 (9.8.18 - 2025年4月22日)**
  - 1.更新第三方库版本
  - 2.添加完全境外用户判断逻辑

- **9.8.17 新特性 (9.8.17 - 2025年4月14日)**
  - 1.更新第三方库版本

- **9.8.15 新特性 (9.8.15 - 2025年2月26日)**
  - 1.更新第三方库版本
  - 2.添加SDK内打点

- **9.8.13 新特性 (9.8.13 - 2024年12月23日)**
  - 1.更新第三方库版本
  - 2.更新AppLovinQualityServiceGradlePlugin版本
    
- **9.8.11 新特性 (9.8.11 - 2024年10月22日)**
  - 1.新增打点
    
- **9.8.10 新特性 (9.8.10 - 2024年9月23日)**
  - 1.修复崩溃问题

- **9.8.9 新特性 (9.8.9 - 2024年9月23日)**
  - 1.更新第三方库版本，修复Google play后台提醒的崩溃问题

- **9.8.8 新特性 (9.8.8 - 2024年9月14日)**
  - 1.优化SDK功能
  - 2.adjust更新至V5版本，去掉AdjustSigSdk库

- **9.8.7 新特性 (9.8.7 - 2024年7月12日)**
  - 1.优化SDK功能
    
- **9.8.6 新特性 (9.8.6 - 2024年6月5日)**
  - 1.优化SDK功能
  - 2.新增SDK打点
    
- **9.8.5 新特性 (9.8.5 - 2024年5月29日)**
  - 1.优化SDK功能

- **9.8.3 新特性 (9.8.3 - 2024年5月16日)**
  - 1.添加adjust SDK签名功能

- **9.8.1 新特性 (9.8.1 - 2024年4月9日)**
  - 1.优化广告回调方法
    
- **9.8.0 新特性 (9.8.0 - 2024年4月7日)**
  - 1.更新第三方库版本
  - 2.新增bidmachine渠道
    
- **9.7.9 新特性 (9.7.9 - 2024年2月19日)**
  - 1.更新第三方库版本
  - 2.支持Google DMA

- **9.7.8 新特性 (9.7.8 - 2024年1月24日)**
  - 1.更新AppLovin版本至最新版本，更新其他第三方库版本
  - 2.支持Google CMP
    
- **9.7.0 新特性 (9.7.0 - 2022年10月10日)**
  - 1.更新AppLovin版本至11.5.2，更新其他第三方库版本
  - 2.okHttpClient请求时添加Android系统的判断，系统版本大于等于21
  - 3.新增V1LTV打点，删掉旧的LTV打点
  
- **9.6.0 新特性 (9.6.0 - 2022年9月20日)**
  - 1.添加判断区分Admob和Google Ad Manager，fyber和dt exchange的渠道名称
  - 2.去掉UAC， guidefinish等无用打点token值
  
- **9.5.4 新特性 (9.5.4 - 2022年7月27日)**
  - 1.更新了第三方SDK版本，注意build.gradle中的修改，unity-ads 4.0.1及以下版本不符合Google Play的用户数据政策，必须更新！！！
  - 2.添加Firebase Crashlytics
  - 3.添加内购是否打点的逻辑判断
  
- **9.5.3 新特性 (9.5.3 - 2022年7月12日)**
  - 1.新增设置用户ID接口。
  - 2.更新了第三方SDK版本，注意build.gradle中的修改，unity-ads 4.0.1及以下版本不符合Google Play的用户数据政策，必须更新！！！
  
- **9.5.2 新特性 (9.5.2 - 2022年6月21日)**
  - 1.新增Firebase打点ad_impression
  - 2.更新了第三方版本，注意build.gradle中的修改，unity-ads 4.0.1及以下版本不符合Google Play的用户数据政策，必须更新！！！
  
- **9.5.1 新特性 (9.5.1 - 2022年6月17日)**
  - 1.添加Banner展示Firebase的打点

- **9.5 新特性 (9.5 - 2022年6月9日)**
  - 1.新用户请求服务端失败后SDK会读取本地hw-services.json配置加载广告。
  - 2.请求间隔时间调整为15秒。
  - 3.服务端参数解析失败时SDK打点hwServiceErrorToken事件到Adjust。

- **9.4 新特性 (9.4 - 2022年5月25日)**
  - 1.升级facebook到6.11.0
  - 2.删除掉9.3中的另外一套聚合
  
- **9.3 新特性 (9.3 - 2022年4月24日)**
  - 1.banner由2套聚合控制，可以任意切换
  - 2.修复国内android12手机，没有授权网络权限第一次没有广告的bug

- **9.1.1 新特性 (9.1.1 - 2022年3月30日)**
  - 1.修复banner在某些场景下会中断的bug

- **9.1 新特性 (9.1 - 2022年3月28日)**
  - 1.内购LTV增加版本号

- **9.0.1 新特性 (9.0.1 - 2022年3月7日)**
  - 1.修复：多了下划线，导致服务端不好处理数据。
  - 2.内购加了版本号，方便排查数据

- **9.0 新特性 (9.0 - 2022年2月21日)**
  - 1.广告LTV打点，避免重复，支持去重
  - 2.渠道版本做了一次升级

- **8.6 新特性 (8.6 - 2022年2月16日)**
  - 1.针对内购，新增一种LTV打点的方式；和服务端一起让内购的LTV更准确

- **8.5 新特性 (8.5 - 2022年2月11日)**
  - 1.完善banner的ltv打点
  - 2.修复Android 12在vpn不稳定时，请求不到广告参数时，会有偶现的崩溃；
  - 3.LTV打点针对新的渠道，增加子渠道打点

- **8.4.2 新特性 (8.4.2 - 2022年1月26日)**
  - 1.针对初始化接口，仅仅初始化1次；修复多次初始化的bug

- **8.4.1 新特性 (8.4 - 2021年12月28日)**
  - 今天发现，8.4的版本，传的jar包是8.3的版本；今天重新传一个8.4的版本。

- **8.4 新特性 (8.4 - 2021年12月23日)**
  - 1.支持了内购打点到firebase
  - 2.去除了http的支持，注意xml中，删了一段代码
  - 3.去掉了adcolony的广告
  
- **8.3 新特性 (8.3 - 2021年12月15日)**
  - 1.这个版本主要针对banner的产品做了大的调整；
  - 2.返回banner的高度，默认是50dp或者90d
  - 3.获取Banner对象；通过获取这个banner对象，可以自由灵活控制banner显示的位置，以及对齐方式
  - 4.针对banner，增加了smaato渠道，注意build.gradle中修改了
  - 5.针对内购，SDK内部增加了一个参数，用于更精准的计算内购相关的模型

- **8.2 新特性 (8.2 - 2021年12月9日)**
  - 1.支持了内购打点到firebase
  - 2.去除了http的支持，注意xml中，删了一段代码
  - 3.去掉了adcolony的广告

- **8.1.1 新特性 (8.1.1 - 2021年12月3日)**
  - 1.针对内购，为了区分大R，中R，小R，增加了一个参数
  
- **8.1 新特性 (8.1 - 2021年11月24日)**
  - 1.针对内购，订阅进行了优化，调用二次验证的API增加了订单id的参数。

- **8.0.1 新特性 (8.0.1 - 2021年11月9日)**
  - 1.针对Facebook无法精准归因的问题，做了升级
  - 2.支持商业化方面的AB测试
  
- **7.4 新特性 (7.4 - 2021年10月14日)**
  - 1.针对Facebook升级了全部SDK的版本
  - 2.将插屏广告播放调整到主线程
  
- **7.3 新特性 (7.3 - 2021年9月28日)**
  - 1.针对Android12 增加了一个适配的权限
  
- **7.2 新特性 (7.2 - 2021年9月2日)**
  - 1.针对国内流量海外流量进行了区分
  - 2.facebook升级到了6.6.0
 
- **7.1 新特性 (7.1 - 2021年8月20日)**
  - 1.针对2家bidding渠道，升级小版本，bidding的效果更好。
  
- **7.0 新特性 (7.0 - 2021年8月10日)**
  - 1.初始化接口，增加了是否支持firebase的参数，支持传“yes”，不支持传“no”
  - 2.多支持一家bidding
  
- **6.7 新特性 (6.7 - 2021年7月22日)**
  - 1.更多的日志输出，方便查bug，
  - 2.支持用户级别证书抓包
  
- **6.6 新特性 (6.6 - 2021年7月20日)**
  - 1.删除facebook统计相关的代码，facebook统计，7月1号开始不让用了

- **6.5 新特性 (6.5 - 2021年7月5日)**
  - 1.修改买量端针对roas买量打点
  
- **6.4 新特性 (6.4 - 2021年6月1日)**
  - 1.统计ltv更精准
  - 2.新增一家广告渠道
  
- **6.3 新特性 (6.3 - 2021年5月19日)**
  - 1.升级Facebook的版本，针对新产品只有用这个版本才有facebook广告
  - 2.替换一家广告的下载地址，之前的旧地址已经无法下载
  
- **6.2 新特性 (6.2 - 2021年4月28日)**
  - 1.修改LTV打点的位置，使得LTV计算更加精准
  - 
- **6.0 新特性 (6.0 - 2021年3月24日)**
  - 1.支持开屏广告
  - 2.支持内购二次验证打点

- **5.7 新特性 (5.7 - 2021年3月9日)**
  - 1.SDK升级到最新版本

- **5.5 新特性 (5.5 - 2021年1月14日)**
  - 1.支持6家bidding
  
- **5.4 新特性 (5.4 - 2020年12月30日)**
  - 1.新增mintegral渠道；
  如果是从之前的版本升级，注意jar的替换，两个build.gradle中的修改

- **5.3 新特性 (5.3 - 2020年12月3日)**
  - 1.升级SDK版本，14.10
  
- **5.2 新特性 (5.2 - 2020年11月19日)**
  - 1.修复统计会话数不准的bug
  - 2.经过内部2个量级大的产品的验证，现在改成开放版

- **beta 5.1 新特性 (5.1 - 2020年11月10日)**
  - 1.增加服务端show，close打点
  
- **beta 5.0.1 新特性 (5.0.1 - 2020年10月19日 11月4日Update)**
  - 1.全新的SDK，较上一个版本，initSDK的参数从10个减少到7个，其他接口保持一致
  - 2.注意xml的配置
  - 3.注意gradle修改了很多，需要重新配置
  - 4.修改回调延时0.5秒的bug
  
- **3.5 新特性 (3.5 - 2020年11月2日)**
  - 在3.4的基础上修改代码，banner加载完，就自动显示
  
- **3.4 新特性 (3.4 - 2020年10月9日)**
  - facebook针对没有产生facebook收益的产品，9月28号一刀切，新应用需要升级到3.4才有facebook广告填充
  
- **3.3 新特性 (3.3 - 2020年9月30日)**
  - 针对推广AEO投放，增加功能支持，需要开发打对应的5个事件，调用方法见文档
  
- **3.2 新特性 (3.2 - 2020年9月2日)**
  - 删除一家被google play警告的SDK
  
- **3.1 新特性 (3.1 - 2020年8月5日)**
  - 升级Pangle版本
  
- **3.0 新特性 (3.0 - 2020年7月28日)**
  - 1.更精细化计算数据
  - 2.推广需要打几个点，用于更多维度买量
  - 3.和以前版本的SDK，取消了Application，初始化参数有8个token
  
- **2.3.1 新特性 (2.3.1 - 2020年7月23日)**
  - 升级csj版本，2.5.0.0之前的不让使用，现在升级到2.9.0.3版本，csj插屏可以使用
  
- **2.3 新特性 (2.3 - 2020年5月25日)**
  - 修正admob激励视频，中途关闭也给了奖励的bug
  
- **2.2 新特性 (2.2 - 2020年5月18日)**
  - 修正内部插屏打点逻辑，显示激励广告挪到了主线程
  
- **2.1 新特性 (2.1 - 2020年5月11日)**
  - 新增2家广告渠道，显示激励广告挪到了主线程
  
- **2.0 新特性 (2.0 - 2020年4月23日)**
  - 广告渠道版本升级到最新；减轻服务端压力
  
- **1.5 新特性 (1.5 - 2020年4月8日)**
  - 同步iOS 4.1版本的功能，将打点事件，1个拆分成3个，提高服务端拉取效率；
  
- **1.4 新特性 (1.4 - 2020年3月30日)**
  - 同步iOS 4.0版本的功能，增加banner加载成功的回调；
  
- **1.3 新特性 (1.3 - 2020年3月23日)**
  - 同步iOS 3.1版本的功能；
  
- **1.2 新特性 (1.2 - 2020年2月28日)**
  - 删除了一家渠道，渠道告知可能存在风险；

- **1.1 新特性 (1.1 - 2020年1月18日)**
  - 修复插屏控制逻辑不准的bug

- **1.0 新特性**
  - 支持海外广告
