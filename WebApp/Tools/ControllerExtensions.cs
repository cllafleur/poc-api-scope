using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using WebApp.Models;

namespace WebApp.Tools
{
    public static class ControllerExtensions
    {
        public static IHttpActionResult CreateResponse(this ApiController controller, HttpStatusCode status, object content)
        {
            return new NegotiatedContentResult<object>(status, content, controller);
        }
    }
}