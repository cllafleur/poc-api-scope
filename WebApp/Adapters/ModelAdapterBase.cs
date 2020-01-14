using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public abstract class ModelAdapterBase<TModel, TSource>
        : AdapterBase<TModel, TSource>
        where TSource : SerializableModel
    {
        protected ModelAdapterBase(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        { }


        protected sealed override void ConditionalMap(Action action, string propertyName)
        {
            ConditionalMap(action, propertyName, _propertyMappingResolverProvider.GetPredicate<TSource>());
        }

        protected sealed override void ConditionalMap(Action action, string propertyName, Func<string, bool> predicate)
        {
            if (predicate(propertyName))
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Errors.Add(new ValidationResult(ex.Message, new[] { propertyName }));
                }
            }
        }

        protected sealed override TOut Fill<TOut, TNewSource>(TModel model, Func<TOut> property, TNewSource source)
        {
            var adapter = _adapterFactoryService.GetAdapter<TOut, TNewSource>();
            adapter.Fill(property(), source);
            return property();
        }
    }
}