using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Review
{
    public class ReviewNotFoundException : Exception, IApplicationException
    {
        public ReviewNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
