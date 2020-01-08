using System;
using Business.UseCase;
using Swashbuckle.Swagger;
using WebApp.Documentation.Contracts;

namespace WebApp.Documentation.Services
{
    public class UseCaseExtractorService : IUseCaseExtractorService
    {
        public UseCases ExtractUseCaseFromSwaggerVersion(SwaggerDocument swaggerDoc)
        {
            if (swaggerDoc?.info?.version == null || swaggerDoc.info.version.Length < 3)
            {
                return UseCases.None;
            }

            if (Enum.TryParse(swaggerDoc.info.version.Substring(0, swaggerDoc.info.version.Length - 2),
                out UseCases useCase))
            {
                return useCase;
            }

            return UseCases.None;
        }
    }
}