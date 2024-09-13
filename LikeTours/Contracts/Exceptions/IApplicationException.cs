using System.Net;

namespace LikeTours.Contracts.Exceptions
{
    public interface IApplicationException
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }
    }
}
