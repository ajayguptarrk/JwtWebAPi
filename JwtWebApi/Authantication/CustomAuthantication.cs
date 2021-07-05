using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace JwtWebApi
{
    public class CustomAuthantication : AuthorizeAttribute, IAuthenticationFilter
    {

        public bool AllowMultipleObject
        {
            get { return false; }
        }


        async Task IAuthenticationFilter.AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParameter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            string[] tokenandUser = null;
            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPharse: "Missing AuthorizationHeader", request);
                return;
            }
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPharse: "Invalid Authorization Schema", request);
                return;
            }

            tokenandUser = authorization.Parameter.Split(':');


            string token = tokenandUser[0];
            string user = "";
            if (tokenandUser.Length > 1)
            {
                user = tokenandUser[1];
            }

            if (string.IsNullOrEmpty(token))
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPharse: "Missing token", request);
                return;
            }
            string validusernName = TokenManager.ValidateToken(token);
            if (user != validusernName)
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPharse: "Invalid token for user", request);
                return;
            }


            context.Principal = TokenManager.GetPrincipal(token);
        }

        async Task IAuthenticationFilter.ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(scheme: "Basic", parameter: "realm=localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }
    }
}