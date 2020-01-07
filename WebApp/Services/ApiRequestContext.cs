using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Business.UseCase;

namespace WebApp.Services
{
    public class ApiRequestContext : IApiRequestContext
    {
        public string UserName { get; private set; }

        public UseCases UseCase { get; private set; }

        public string ClientId { get; private set; }

        public ApiRequestContext()
        {
            var authentication = HttpContext.Current.GetOwinContext().Authentication;
            UserName = authentication.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            ClientId = authentication.User.Claims.FirstOrDefault(c => c.Type == "client_id")?.Value;
            var scope = authentication.User.Claims.FirstOrDefault(c => c.Type == "useCase")?.Value;
            if (Enum.TryParse<UseCases>(scope, out UseCases useCase))
            {
                UseCase = useCase;
            }
        }
    }
}