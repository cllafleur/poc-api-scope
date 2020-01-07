using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApp;
using WebApp.Services;
using Unity;

namespace Business.UseCase
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class UseCaseAttribute : Attribute
    {
        private static Type[] _useCaseAttributes;

        public static Type[] UseCaseAttributes
        {
            get
            {
                if (_useCaseAttributes == null)
                {
                    _useCaseAttributes = typeof(UseCaseAttribute).Assembly.GetTypes().Where(t => typeof(UseCaseAttribute).IsAssignableFrom(t)).ToArray();
                }
                return _useCaseAttributes;
            }
        }

        public UseCases Value { get; private set; }

        protected UseCaseAttribute(UseCases usecase)
        {
            Value = usecase;
        }
    }
}
