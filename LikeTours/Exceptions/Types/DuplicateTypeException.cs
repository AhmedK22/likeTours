using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Types
{
    public class DuplicateTypeException: Exception,IApplicationException
    {
        public DuplicateTypeException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => Message;
    }
}
