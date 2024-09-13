using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Payment
{
    public class ConstraintPaymentErrorException : Exception, IApplicationException
    {
        public ConstraintPaymentErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
