using LindDotNetCore.Repository;
using LindDotNetCore.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 功能描述    ：RepositoryOptionsExtension
    /// 创 建 者    ：lind
    /// 创建日期    ：2017/10/11 11:08:10
    /// 最后修改者  ：lind
    /// 最后修改日期：2017/10/11 11:08:10
    /// </summary>
    public static class RepositoryOptionsExtensions
    {
        /// <summary>
        /// 使用EF持久化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddEf(this IServiceCollection services, Action<EFConfig> configure)
        {
            var options = new EFConfig();
            //装饰
            configure?.Invoke(options);
            //优先级控制
            ObjectMapper.MapperTo(options, ConfigFileHelper.Get<EFConfig>());
            //ef相关配置
            services.AddSingleton(options);
            //注册通用DbContext上下文，主要为通用的EFRepository仓储提供数据对象，单个数据上下文时不需要定义自己的仓储
            services.AddTransient(typeof(DbContext), options.DbContextType);
            //注册通用数据仓储
            services.AddTransient(typeof(IRepository<>), typeof(EFRepository<>));
            //注册当前数据上下文，主要为当前业务仓储提交数据对象，而业务仓储的注册在业务项目里，实例模式
            services.AddTransient(options.DbContextType);
            return services;
        }

        /// <summary>
        /// 使用Dapper持久化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddDapper(this IServiceCollection services, Action<RepositoryConfig> configure)
        {
            var mysqlOptions = new RepositoryConfig();
            configure(mysqlOptions);
            services.AddSingleton(mysqlOptions);
            services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
            return services;
        }
    }
}