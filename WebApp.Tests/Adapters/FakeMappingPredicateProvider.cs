using Business.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Adapters;

namespace WebApp.Tests.Adapters
{
    public class FakeMappingPredicateProvider
    {
        public static Func<string, bool> GetPredicate<T>(UseCases useCase)
        {
            var descriptor = new ModelDescriptor(typeof(T));

            return (p) => descriptor.PropertiesByUseCase[useCase].Contains(p);
        }
    }
}
