using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;
using Unity;
using WebApp.Documentation.Contracts;

namespace WebApp.Documentation
{
    public class SwaggerFilter : IDocumentFilter
    {
        private readonly IUseCaseExtractorService _useCaseExtractorService;

        private readonly IPropertiesInfoProvider _propertiesInfoProvider;

        private readonly ISwaggerDocInspectionService _swaggerDocInspectionService;

        public SwaggerFilter()
        {
            this._useCaseExtractorService = UnityConfig.Container.Resolve<IUseCaseExtractorService>();
            this._propertiesInfoProvider = UnityConfig.Container.Resolve<IPropertiesInfoProvider>();
            this._swaggerDocInspectionService = UnityConfig.Container.Resolve<ISwaggerDocInspectionService>();
        }

        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            if (swaggerDoc == null || schemaRegistry == null)
            {
                return;
            }

            var useCase = _useCaseExtractorService.ExtractUseCaseFromSwaggerVersion(swaggerDoc);
            var types = new HashSet<string>();

            foreach (var schemaRegistryDefinition in schemaRegistry.Definitions)
            {
                var props = _propertiesInfoProvider.GetPropertyInfos(schemaRegistryDefinition.Key);
                foreach (var prop in props)
                {
                    if (prop.Value.All(at => at.Value != useCase))
                    {
                        schemaRegistryDefinition.Value.properties.Remove(prop.Key);
                    }
                    else
                    {
                        var type = _swaggerDocInspectionService.LoadType(schemaRegistryDefinition.Value.properties[prop.Key]);
                        if (type != null && !types.Contains(type))
                        {
                            types.Add(type);
                        }
                    }
                }
            }

            types.UnionWith(_swaggerDocInspectionService.GetRootTypes(swaggerDoc));
            
            RemoveNotUsedDefinitions(schemaRegistry, types);
        }

        private void RemoveNotUsedDefinitions(SchemaRegistry schemaRegistry, HashSet<string> types)
        {
            var notReferencedTypes = new List<string>();
            foreach (var schemaRegistryDefinition in schemaRegistry.Definitions)
            {
                if (!types.Contains(schemaRegistryDefinition.Key))
                {
                    notReferencedTypes.Add(schemaRegistryDefinition.Key);
                }
            }

            foreach (var notUsedType in notReferencedTypes)
            {
                schemaRegistry.Definitions.Remove(notUsedType);
            }
        }
    }
}