using System.Net;

namespace LindDotNetCore.Exceptions
{
    /// <summary>
    /// 无权访问异常
    /// </summary>
    public class ForbiddenException : HttpStatusExcetion
    {
        public ForbiddenException(string message) : base(message)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.Forbidden;
    }
}