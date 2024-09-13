using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.aboutUs
{
    public class AssignedMainAboutException : Exception, IApplicationException
    {
        public AssignedMainAboutException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
