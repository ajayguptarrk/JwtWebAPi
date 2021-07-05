using JwtWebApi.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JwtWebApi
{
   // [CustomAuthantication]
    public class TestDataController : ApiController
    {

        JwtApiDBEntities dbcontext = new JwtApiDBEntities();
        // GET api/values
         
        public HttpResponseMessage Get()
        {
            List<TestData> testDataList = new List<TestData>();
            using (JwtApiDBEntities dc = new JwtApiDBEntities())
            {
                testDataList = dc.TestDatas.OrderBy(a => a.Name).ToList();
                HttpResponseMessage response;
                response = Request.CreateResponse(HttpStatusCode.OK, testDataList);
                return response;
            }
        }


    }
}
