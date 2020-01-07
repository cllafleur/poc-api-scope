using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Adapters
{
    public abstract class ApiModelAdapterBase<TModel, TSource>
        : AdapterBase<TModel, TSource>
        where TModel : SerializableModel
    {

        protected ApiModelAdapterBase(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        { }


        protected sealed override void ConditionalMap(Action action, string propertyName)
        {
            ConditionalMap(action, propertyName, _propertyMappingResolverProvider.GetPredicate<TModel>());
        }

        protected sealed override void ConditionalMap(Action action, string propertyName, Func<string, bool> predicate)
        {
            if (predicate(propertyName))
            {
                action();
            }
        }

        protected sealed override TOut Fill<TOut, TNewSource>(TModel model, Func<TOut> property, TNewSource source)
        {
            var adapter = _adapterFactoryService.GetAdapter<TOut, TNewSource>();
            var newObject = (TOut)Activator.CreateInstance(typeof(TOut));
            adapter.Fill(newObject, source);
            return newObject;
        }
    }
}