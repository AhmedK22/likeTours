using LikeTours.Contracts.Exceptions;
using System.Net;

namespace LikeTours.Exceptions.Places
{
    public class PlaceNotFoundException : Exception, IApplicationException
    {
        public PlaceNotFoundException(string errorMessage) : base(errorMessage)
        {

        }
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }
}
