using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Review
{
    public class ConstraintReviewErrorException : Exception, IApplicationException
    {
        public ConstraintReviewErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
