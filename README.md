# Pdd-Service

#### 介绍 2024-05-31
拼多多商戶打单系统(多商户)，疫情期间准备加入拼多多的开发平台，结果开发者身份认证一直通不过。
结果就完成了一半。

拼多多的订单通知采用WebSocket协议，当时只支持Java代码接入，所以写了一个中间件，对消息进行传输。
目前不知道什么方案，协议的具体内容也懒得去打探了。

## 多啰嗦几句
- 后端当时使用.Net Core 3.1开发，整理代码时候升级为.Net 7
- 前端忘记了
- 就这样
