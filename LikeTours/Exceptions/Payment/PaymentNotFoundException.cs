using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Payment
{
    public class PaymentNotFoundException : Exception, IApplicationException
    {
        public PaymentNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
