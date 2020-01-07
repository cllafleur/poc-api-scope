using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApp.Tools
{
    public class ApiResult<T>
    {
        public HttpStatusCode StatusCode { get; private set; }

        public T Result { get; private set; }

        public ApiResult(HttpStatusCode statusCode, T result)
        {
            this.StatusCode = statusCode;
            this.Result = result;
        }
    }
}