using System.Net;

namespace Lind.DotNetCore.Exceptions
{
    /// <summary>
    /// 客户端输入问题
    /// </summary>
    public class NoFoundException : HttpStatusExcetion
    {
        public NoFoundException(string message) : base(message)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.NotFound;
    }
}