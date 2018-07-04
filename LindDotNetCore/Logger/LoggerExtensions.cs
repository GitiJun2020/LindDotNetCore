using LindDotNetCore.Logger;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
     /// <summary>
    /// 日志扩展
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 使用文件日志
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLoggerFile(
            this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger), typeof(LindLogger));
            return services;
        }

        /// <summary>
        /// 使用api日志
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLoggerConsole(
            this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger), typeof(ConsoleLogger));
            return services;
        }
    }
}