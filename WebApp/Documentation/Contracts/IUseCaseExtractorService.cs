using Business.UseCase;
using Swashbuckle.Swagger;

namespace WebApp.Documentation.Contracts
{
    public interface IUseCaseExtractorService
    {
        UseCases ExtractUseCaseFromSwaggerVersion(SwaggerDocument swaggerDoc);
    }
}
