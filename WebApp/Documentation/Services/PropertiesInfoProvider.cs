using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Business.UseCase;
using WebApp.Documentation.Contracts;

namespace WebApp.Documentation.Services
{
    public class PropertiesInfoProvider : IPropertiesInfoProvider
    {
        public Dictionary<string, IEnumerable<UseCaseAttribute>> GetPropertyInfos(string modelName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var type = assembly.GetTypes().Single(t => t.Name == modelName);
            return type.GetProperties().ToDictionary(m => m.Name, m => m.GetCustomAttributes<UseCaseAttribute>());
        }
    }
}