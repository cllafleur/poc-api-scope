using System.Collections.Generic;
using Business.UseCase;

namespace WebApp.Documentation.Contracts
{
    public interface IPropertiesInfoProvider
    {
        Dictionary<string, IEnumerable<UseCaseAttribute>> GetPropertyInfos(string modelName);
    }
}