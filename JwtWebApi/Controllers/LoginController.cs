using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace JwtWebApi
{
    public class LoginController : ApiController
    {
        [HttpGet]        
        public HttpResponseMessage Get(string userName, string password)
        {
            if (userName == "admin" && password == "admin")
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, TokenManager.GenerateToken(userName));
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadGateway, "userName or Password is not valid.");
            }
        }
    }
}
