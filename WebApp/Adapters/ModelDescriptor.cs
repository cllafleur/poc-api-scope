using Business.UseCase;
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
            InitDictionary(propertiesByUseCases);
            foreach (var property in modelType.GetProperties())
            {
                var attributes = property.GetCustomAttributes(false);
                foreach (var attribute in attributes)
                {
                    var usecaseAttribute = attribute as UseCaseAttribute;
                    if (usecaseAttribute != null)
                    {
                        propertiesByUseCases[usecaseAttribute.Value].Add(property.Name);
                    }
                }
            }
            this.PropertiesByUseCase = propertiesByUseCases;
        }

        public Dictionary<UseCases, HashSet<string>> PropertiesByUseCase { get; private set; }

        private void InitDictionary(Dictionary<UseCases, HashSet<string>> dic)
        {
            foreach (var usecase in Enum.GetNames(typeof(UseCases)))
            {
                dic.Add((UseCases)Enum.Parse(typeof(UseCases), usecase), new HashSet<string>());
            }
        }
    }
}