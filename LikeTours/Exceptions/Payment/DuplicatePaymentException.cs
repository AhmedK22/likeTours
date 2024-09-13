using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Payment
{
    public class DuplicatePaymentException : Exception, IApplicationException
    {
        public DuplicatePaymentException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
