using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Contacts
{
    public class ContactNotFoundException : Exception, IApplicationException
    {
        public ContactNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    
    }
}
