#### Caching
数据缓存是比较重要的部分，用来存储一些热数据，目前分布式环境使用redis，单机可以直接使用
运行时缓存。
```c#
services.AddRuntimeCache(o =>
{
    o.CacheKey = "lindCache";
    o.ExpireMinutes = 5;
});
```