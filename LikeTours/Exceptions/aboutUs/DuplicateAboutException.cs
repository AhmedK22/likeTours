using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.aboutUs
{
    public class DuplicateAboutException : Exception, IApplicationException
    {
        public DuplicateAboutException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => Message;
    }
}
