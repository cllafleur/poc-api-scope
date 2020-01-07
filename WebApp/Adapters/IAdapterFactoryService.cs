using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Adapters
{
    public interface IAdapterFactoryService
    {
        IAdapter<TModel, TSource> GetAdapter<TModel, TSource>();

        object GetAdapter(Type typeModel, Type typeSource);
    }
}