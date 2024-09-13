using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Places
{
    public class ConstraintPlaceErrorException : Exception, IApplicationException
    {
        public ConstraintPlaceErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
