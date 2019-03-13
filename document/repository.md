#### 仓储
仓储主要简化数据持久化的操作，对外提供简单的CURD操作接口，使用者直接调用即可，不需要干预SQL语句，
从这点上来说，开发效率确实提升了不少。目前大叔框架里集成了`ef,dapper,mongodb,redis,elastic`等仓储，其中
EF和Dapper可以操作sqlserver,mysql,sqllite等数据库。
```c#
services.UseDapper(o =>
{
    o.ConnString = $"Data Source={Directory.GetCurrentDirectory()}/intergratetest.db";
    o.DbType = Lind.DotNetCore.Repository.DbType.SqlLite;
});