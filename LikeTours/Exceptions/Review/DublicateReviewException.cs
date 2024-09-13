using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Review
{
    public class DublicateReviewException : Exception, IApplicationException
    {
        public DublicateReviewException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
