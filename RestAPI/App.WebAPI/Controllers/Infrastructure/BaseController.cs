using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace App.WebAPI.Controllers.Infrastructure
{
    public class BaseController : ApiController
    {
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.OK;
            return response;
        }
    }
}
