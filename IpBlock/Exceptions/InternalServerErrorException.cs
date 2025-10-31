using System.Net;

namespace IpBlock.Exceptions
{
    public class InternalServerErrorException : AppException
    {
        public InternalServerErrorException(string message)
            : base(message, HttpStatusCode.InternalServerError) { }
    }
}
