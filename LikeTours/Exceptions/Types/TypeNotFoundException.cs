using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Types
{
    public class TypeNotFoundException : Exception, IApplicationException
    {
        public TypeNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
