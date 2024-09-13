using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Places
{
    public class ConstraintTypeErrorException : Exception, IApplicationException
    {
        public ConstraintTypeErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
