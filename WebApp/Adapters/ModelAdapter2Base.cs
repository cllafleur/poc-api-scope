using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Adapters
{
    public abstract class ModelAdapter2Base<TModel, TSource>
        : AdapterBase<TModel, TSource>
        where TSource : SerializableModel
    {
        private static readonly ConcurrentDictionary<string, string> _tableNameCache = new ConcurrentDictionary<string, string>();
        private string _currentPropertyName = string.Empty;

        protected ModelAdapter2Base(IAdapterFactoryService adapterFactoryService, IMappingPredicateProvider propertyMappingResolverProvider)
            : base(adapterFactoryService, propertyMappingResolverProvider)
        { }


        protected sealed override void ConditionalMap(Action action, string propertyName)
        {
            ConditionalMap(action, propertyName, _propertyMappingResolverProvider.GetPredicate<TSource>());
        }

        protected sealed override void ConditionalMap(Action action, string propertyName, Func<string, bool> predicate)
        {
            _currentPropertyName = propertyName;
            try
            {
                if (predicate(propertyName))
                {
                    action();
                }
            }
            finally
            {
                _currentPropertyName = string.Empty;
            }
        }

        protected TOut GetValue<TIn, TOut>(Func<string, TIn, TOut> valueProvider, TIn argument, Expression<Func<TOut>> propertyPredicate)
        {
            var tableName = GetPropertyTableName(propertyPredicate);
            var resultValue = valueProvider(tableName, argument);
            if (resultValue == null && argument != null)
            {
                Errors.Add(new ValidationResult("lookup failed for ", new[] { _currentPropertyName }));
            }
            return resultValue;
        }

        private string GetPropertyTableName<TOut>(Expression<Func<TOut>> propertyPredicate)
        {
            var key = propertyPredicate.Body.ToString();
            var value = _tableNameCache.GetOrAdd(key, k =>
            {
                var expression = (MemberExpression)propertyPredicate.Body;
                var referencedType = expression.Member.GetCustomAttributes(false).Where(a => a.GetType().GUID == typeof(SerializableAttribute).GUID).Cast<SerializableAttribute>().FirstOrDefault();
                if (referencedType == null)
                {
                    return null;
                }
                return "random Name";
            });
            return value;

        }

        protected sealed override TOut Fill<TOut, TNewSource>(TModel model, Func<TOut> property, TNewSource source)
        {
            var adapter = _adapterFactoryService.GetAdapter<TOut, TNewSource>();
            adapter.Fill(property(), source);
            return property();
        }
    }
}