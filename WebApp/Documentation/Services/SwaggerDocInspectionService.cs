using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Swagger;
using WebApp.Documentation.Contracts;

namespace WebApp.Documentation.Services
{
    public class SwaggerDocInspectionService :ISwaggerDocInspectionService
    {
        public HashSet<string> GetRootTypes(SwaggerDocument swaggerDoc)
        {
            var rootReferences = new HashSet<string>();
            var done = new List<object>();
            foreach (var swaggerDocPath in swaggerDoc.paths.Select(p => p.Value))
            {
                LoadReferences(swaggerDocPath, rootReferences, done);
            }

            return rootReferences;
        }

        public string LoadType(object schema)
        {
            var refs = new HashSet<string>();
            var d = new List<object>();
            LoadReferences(schema, refs, d);
            return refs.Any() ? refs.First() : null;
        }

        private void LoadReferences(object obj, HashSet<string> definitions, List<object> done)
        {
            if (obj == null) return;
            Type objType = obj.GetType();

            // in order to avoid circular references issues
            if (done.Contains(obj))
                return;
            done.Add(obj);

            foreach (var field in objType.GetFields())
            {
                var propValue = field.GetValue(obj);
                if (field.Name == "ref")
                {
                    if (propValue != null && !definitions.Contains(propValue?.ToString().Split('/').Last()))
                        definitions.Add(propValue?.ToString().Split('/').Last());
                }

                var elems = propValue as IList;
                var elemDic = propValue as IDictionary;
                if (elems != null)
                {
                    foreach (var item in elems)
                    {
                        LoadReferences(item, definitions, done);
                    }
                }
                else if (elemDic != null)
                {
                    foreach (var item in elemDic.Keys)
                    {
                        LoadReferences(item, definitions, done);
                    }

                    foreach (var item in elemDic.Values)
                    {
                        LoadReferences(item, definitions, done);
                    }
                }
                else
                {
                    if (field.FieldType.Assembly == objType.Assembly)
                    {
                        LoadReferences(propValue, definitions, done);
                    }
                }
            }
        }
    }
}