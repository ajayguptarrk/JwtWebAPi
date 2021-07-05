using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace JwtWebApi
{
    internal class AuthenticationFailureResult : IHttpActionResult
    {
        private string ReasonPhrase;
        private HttpRequestMessage Request;

        public AuthenticationFailureResult(string reasonPharse, HttpRequestMessage request)
        {
            this.ReasonPhrase = reasonPharse;
            this.Request = request;
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = ReasonPhrase;
            return responseMessage;

        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }
    }
}