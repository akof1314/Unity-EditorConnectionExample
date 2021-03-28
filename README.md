## 目的
实现与 Unity Profiler 一样可以连接手机进行通信，来接收 App 发送的信息或发送消息给 App，而 Unity 有开放 EditorConnection 和 PlayerConnection 接口，可以使用它们来实现。

## 测试
Unity 2018 发布 App，记得选中 Development Build 以及 Autoconnect Profiler 选项。

![](https://img-blog.csdnimg.cn/20210328191744541.png)

自动连接成功，此时可以点击按钮进行发送消息。

![](https://img-blog.csdnimg.cn/20210328191745248.jpg)

手机上收到消息，显示在界面上。点击按钮可以发消息给编辑器。

![](https://img-blog.csdnimg.cn/20210328191744599.png)

编辑器也收到消息打印在控制台。
## 问题
目前发现的问题：
- 不能反注册连接和断开连接的事件
- App 上的连接和断开连接事件不一定会响应
- 如果断开后再连接的话，App 可以发送数据到编辑器，接收不到编辑器发送的数据

## 源码
测试工程地址：[https://github.com/akof1314/Unity-EditorConnectionExample](https://github.com/akof1314/Unity-EditorConnectionExample)