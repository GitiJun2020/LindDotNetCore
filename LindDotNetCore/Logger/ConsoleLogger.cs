using System;

namespace LindDotNetCore.Logger
{
    /// <summary>
    /// 控制台日志
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        protected override void InputLogger(Level level, string message)
        {
            Console.WriteLine(DateTime.Now + level.ToString() + message);
        }
    }
}