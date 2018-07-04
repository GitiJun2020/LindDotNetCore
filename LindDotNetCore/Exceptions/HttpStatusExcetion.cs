using System;
using System.Net;

namespace LindDotNetCore.Exceptions
{
    /// <summary>
    /// 关于Http的异常抽象
    /// </summary>
    public abstract class HttpStatusExcetion : Exception
    {
        public HttpStatusExcetion(string message) : base(message)
        {
        }

        public virtual HttpStatusCode HttpStatusCode => HttpStatusCode.OK;
    }
}