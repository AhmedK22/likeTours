using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Questions
{
    public class ConstraintQuestionErrorException : Exception, IApplicationException
    {
        public ConstraintQuestionErrorException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
