using Business.Services;
using Business.Vacancies;
using System;

using Unity;
using Unity.AspNet.Mvc;
using WebApp.Adapters;
using WebApp.Documentation.Contracts;
using WebApp.Documentation.Services;
using WebApp.Services;

namespace WebApp
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;

        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType(typeof(IVacancyRepository), typeof(VacancyRepository));
            container.RegisterType(typeof(IIdToCodeConverterService), typeof(IdToCodeConverterService));
            container.RegisterType(typeof(IAdapterFactoryService), typeof(AdapterFactoryService));
            container.RegisterType(typeof(IApiRequestContext), typeof(ApiRequestContext), new PerRequestLifetimeManager());
            container.RegisterType(typeof(IVacancyService), typeof(VacancyService));
            container.RegisterType(typeof(IMappingPredicateProvider), typeof(MappingPredicateProvider));

            container.RegisterType(typeof(IUseCaseExtractorService), typeof(UseCaseExtractorService));
            container.RegisterType(typeof(IPropertiesInfoProvider), typeof(PropertiesInfoProvider));
            container.RegisterType(typeof(ISwaggerDocInspectionService), typeof(SwaggerDocInspectionService));
        }
    }
}