using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Services;

namespace WebApp.Adapters
{
    public class MappingPredicateProvider : IMappingPredicateProvider
    {
        private static readonly ConcurrentDictionary<Guid, Lazy<ModelDescriptor>> _modelTypeDescriptor = new ConcurrentDictionary<Guid, Lazy<ModelDescriptor>>();
        private readonly IApiRequestContext _context;

        public MappingPredicateProvider(IApiRequestContext context)
        {
            _context = context;
        }

        public Func<string, bool> GetPredicate<T>()
        {
            var descriptor = _modelTypeDescriptor.GetOrAdd(typeof(T).GUID, new Lazy<ModelDescriptor>(() => new ModelDescriptor(typeof(T)))).Value;

            return (p) => descriptor.PropertiesByUseCase[_context.UseCase].Contains(p);
        }
    }
}