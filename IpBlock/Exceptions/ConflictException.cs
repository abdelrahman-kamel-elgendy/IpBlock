using System.Net;

namespace IpBlock.Exceptions
{
    public class ConflictException : AppException
    {
        public ConflictException(string message)
            : base(message, HttpStatusCode.Conflict) { }
    }
}
