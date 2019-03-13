#### 日志
日志框架与之前的Lind框架里日志差别不大，只是把对象的生命周期移到了DI容器去统一管理，都采用单例方式，目前日志框架提供了
对log4net的支持，同时轻量级日志可以使用lindlogger来实现。
```c#
services.AddLog4Logger(o =>
{
    o.Log4ConfigFileName = "log4.config";
    o.ProjectName = "test";
});
```