using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Questions
{
    public class AssignedMainQuestionException : Exception, IApplicationException
    {
        public AssignedMainQuestionException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public string ErrorMessage => Message;
    }
}
