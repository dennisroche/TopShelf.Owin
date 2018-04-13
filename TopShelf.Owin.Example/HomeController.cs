using System;
using System.Web.Http;

namespace ExampleSeflHostOwin
{
    public class HomeController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Ping()
        {
            Console.WriteLine("Ping - Ok!");
            return Ok();
        }
    }
}
