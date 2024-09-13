using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Payment
{
    public class AssignedMainPaymentException : Exception, IApplicationException
    {
        public AssignedMainPaymentException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
