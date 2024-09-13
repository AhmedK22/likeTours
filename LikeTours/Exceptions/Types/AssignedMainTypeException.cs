using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Types
{
    public class AssignedMainTypeException: Exception, IApplicationException
    {
        public AssignedMainTypeException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
