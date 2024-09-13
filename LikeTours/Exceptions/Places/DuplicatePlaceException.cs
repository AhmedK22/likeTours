using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Places
{
    public class DuplicatePlaceException:Exception,IApplicationException
    {
        public DuplicatePlaceException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => Message;
    }
}
