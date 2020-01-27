using Business.UseCase;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Adapters
{
    public class ModelDescriptor
    {
        public ModelDescriptor(Type modelType)
        {
            Dictionary<UseCases, HashSet<string>> propertiesByUseCases = new Dictionary<UseCases, HashSet<string>>();
            Dictionary<string, string> serializedNames = new Dictionary<string, string>();
            InitDictionary(propertiesByUseCases);
            foreach (var property in modelType.GetProperties())
            {
                var attributes = property.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    switch (attribute)
                    {
                        case UseCaseAttribute usecaseAttribute:
                            propertiesByUseCases[usecaseAttribute.Value].Add(property.Name);
                            break;
                        case JsonPropertyAttribute propertyAttribute:
                            serializedNames.Add(property.Name, propertyAttribute.PropertyName);
                            break;
                    }

                }
            }
            this.PropertiesByUseCase = propertiesByUseCases;
            this.PropertiesSerializedName = serializedNames;
        }

        public Dictionary<UseCases, HashSet<string>> PropertiesByUseCase { get; private set; }

        public Dictionary<string, string> PropertiesSerializedName { get; private set; }

        private void InitDictionary(Dictionary<UseCases, HashSet<string>> dic)
        {
            foreach (var usecase in Enum.GetNames(typeof(UseCases)))
            {
                dic.Add((UseCases)Enum.Parse(typeof(UseCases), usecase), new HashSet<string>());
            }
        }
    }
}