using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApp.Services;
using Unity;
using Business.UseCase;

namespace WebApp.UseCase
{
    public class UseCaseAuthorizationFilter : IAuthorizationFilter
    {
        public bool AllowMultiple => false;

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var context = UnityConfig.Container.Resolve<IApiRequestContext>();

            UseCaseAttribute[] attributes;
            switch (context.UseCase)
            {
                case UseCases.Customer:
                    attributes = actionContext.ActionDescriptor.GetCustomAttributes<CustomerUseCasesAttribute>().Cast<UseCaseAttribute>().ToArray();
                    break;
                case UseCases.JobBoard:
                    attributes = actionContext.ActionDescriptor.GetCustomAttributes<JobBoardUseCaseAttribute>().Cast<UseCaseAttribute>().ToArray();
                    break;
                default:
                    attributes = Array.Empty<UseCaseAttribute>();
                    break;
            }

            if (attributes.Length > 0)
            {
                return continuation();
            }
            var response = actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Forbidden, "Invalid scope");
            actionContext.Response = response;
            return Task.FromResult(response);
        }
    }
}