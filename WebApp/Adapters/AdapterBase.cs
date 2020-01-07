using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public abstract class AdapterBase<TModel, TSource> : IAdapter<TModel, TSource>
    {
        protected readonly IAdapterFactoryService _adapterFactoryService;
        protected readonly IMappingPredicateProvider _propertyMappingResolverProvider;

        protected AdapterBase(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
        {
            _adapterFactoryService = adapterFactoryService;
            _propertyMappingResolverProvider = propertyMappingResolverProvider;
        }

        public ICollection<ValidationResult> Errors => new List<ValidationResult>();

        public abstract void Fill(TModel model, TSource source);

        public virtual void Fill(ICollection<TModel> models, ICollection<TSource> sources, Func<TModel> modelBuilder)
        {
            foreach (var source in sources)
            {
                var model = modelBuilder();
                this.Fill(model, source);
                models.Add(model);
            }
        }

        protected abstract void ConditionalMap(Action action, string propertyName);

        protected abstract void ConditionalMap(Action action, string propertyName, Func<string, bool> predicate);

        protected abstract TOut Fill<TOut, TNewSource>(TModel model, Func<TOut> property, TNewSource source) where TNewSource : class;
    }
}