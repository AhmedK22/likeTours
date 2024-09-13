using LikeTours.Contracts.Exceptions;
using Renci.SshNet.Messages;
using System.Net;

namespace LikeTours.Exceptions.Questions
{
    public class DuplicateQuestionException : Exception, IApplicationException
    {
        public DuplicateQuestionException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
