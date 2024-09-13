using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.aboutUs
{
    public class ConstraintAboutErrorException : Exception, IApplicationException
    {
        public ConstraintAboutErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
