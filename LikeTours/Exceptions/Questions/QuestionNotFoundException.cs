using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Questions
{
    public class QuestionNotFoundException : Exception, IApplicationException
    {
        public QuestionNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
