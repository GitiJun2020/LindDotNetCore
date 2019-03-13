#### 服务总线
服务总线主要是用来解耦项目的层与层之间的调用，让程序员把关注点放在业务上，目前框架提供了IOC模式的事件，
基于简单内存字典存储的事件等。
```c#
services.AddIocBus();
services.AddInMemoryBus();
```