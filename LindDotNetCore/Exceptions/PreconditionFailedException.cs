using System.Net;

namespace LindDotNetCore.Exceptions
{
    /// <summary>
    /// 客户端输入问题
    /// </summary>
    public class PreconditionFailedException : HttpStatusExcetion
    {
        public PreconditionFailedException(string message) : base(message)
        {
        }

        public override HttpStatusCode HttpStatusCode => HttpStatusCode.PreconditionFailed;
    }
}