using System.Collections.Generic;
using Swashbuckle.Swagger;

namespace WebApp.Documentation.Contracts
{
    public interface ISwaggerDocInspectionService
    {
        HashSet<string> GetRootTypes(SwaggerDocument swaggerDoc);

        string LoadType(object schema);
    }
}