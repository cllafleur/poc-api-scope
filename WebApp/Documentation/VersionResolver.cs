using System;
using System.Linq;
using System.Web.Http.Description;
using Business.UseCase;

namespace WebApp.Documentation
{
    public class VersionResolver
    {
        public bool ResolveVersionSupportByRouteConstraint(ApiDescription apiDesc, string targetApiVersion)
        {
            var targetUseCase = targetApiVersion.Substring(0, targetApiVersion.Length - 2);
            if (Enum.TryParse<UseCases>(targetUseCase, out var useCase))
            {
                var attributes = apiDesc.ActionDescriptor.GetCustomAttributes<UseCaseAttribute>();
                return attributes != null && attributes.Any(attr => attr.Value == useCase);
            }

            return false;
        }
    }
}