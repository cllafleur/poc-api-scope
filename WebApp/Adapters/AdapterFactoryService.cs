using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Unity;
using Unity.Lifetime;

namespace WebApp.Adapters
{
    public class AdapterFactoryService : IAdapterFactoryService
    {
        private readonly IUnityContainer _container;

        static AdapterFactoryService()
        {
            var container = UnityConfig.Container;

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var interfaces = type.FindInterfaces((t, o) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IAdapter<,>), null);
                if (interfaces.Length > 0)
                {
                    container.RegisterType(interfaces[0], type, new TransientLifetimeManager());
                }
            }
        }

        public AdapterFactoryService(IUnityContainer container)
        {
            _container = container;
        }

        public IAdapter<TModel, TSource> GetAdapter<TModel, TSource>()
        {
            return this.GetAdapterInternal<TModel, TSource>();
        }

        public object GetAdapter(Type typeModel, Type typeSource)
        {
            var adapterType = typeof(IAdapter<,>).MakeGenericType(typeModel, typeSource);
            return _container.Resolve(adapterType);
        }

        private IAdapter<Tmodel, Tsource> GetAdapterInternal<Tmodel, Tsource>()
        {
            return _container.Resolve<IAdapter<Tmodel, Tsource>>();
        }
    }
}