using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Places
{
    public class AssignedMainPlaceException : Exception, IApplicationException
    {
        public AssignedMainPlaceException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
