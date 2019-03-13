目前框架的NoSql部分由`redis和mongodb`组成，之所有选择这两种框架最大的原因就是它们覆盖了
NoSql所有的使用场景，像redis用来存储k/v键值对，支持5大数据结构；而mongodb用来存储文档
型数据，支持复杂的查询，嵌套查询等。
```c#
services.AddRedis(o =>
{
    o.Host = "localhost:6379";
    o.AuthPassword = "";
    o.IsSentinel = 1;
    o.ServiceName = "mymaster";
    o.Proxy = 0;
});
### redis相关总结
1. 按key获取value，如果需要查找value，那只能把它转换为key，额外再存储一个k/v
1. 按key获取value，时间复杂度为o(1)，与字典相同
1. 分布并发锁，多进程某过redis来达到排队的效果
1. session共享，多应用实现负载均衡时，共享session
1. 缓存拦截器，通过对方法的aop拦截，实现缓存策略的注入
#### 分布并发锁
```
//先申请锁定时间100ms,如果程序提前执行完,就马上释放锁
if (redisManager.Instance.GetDatabase().LockTake("锁键", "值没什么用", TimeSpan.FromMilliseconds(100)))
{
    try
    {
        Console.WriteLine("正在处理……");
        Thread.Sleep(1000);
    }
    catch (Exception)
    {
        throw;
    }
    finally
    {
        //处理结束后释放redis进程锁,否则还要阻塞100毫秒
        redisManager.Instance.GetDatabase().LockRelease("锁键", "值没什么用");
    }
}
```