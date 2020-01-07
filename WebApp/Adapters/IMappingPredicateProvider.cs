using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Adapters
{
    public interface IMappingPredicateProvider
    {
        Func<string, bool> GetPredicate<T>();
    }
}