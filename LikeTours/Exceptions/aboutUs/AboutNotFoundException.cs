using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.aboutUs
{
    public class AboutNotFoundException : Exception, IApplicationException
    {
        public AboutNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
