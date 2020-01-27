using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebApp.Adapters;

namespace WebApp.Models
{
    [Serializable]
    public class SerializableModel : ISerializable
    {
        protected dynamic _internalObject = new ExpandoObject();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var descriptor = new ModelDescriptor(this.GetType());

            var dic = (IDictionary<string, object>)_internalObject;
            foreach (var pair in dic)
            {
                string key = pair.Key;
                if (descriptor.PropertiesSerializedName.ContainsKey(key))
                {
                    key = descriptor.PropertiesSerializedName[key];
                }
                info.AddValue(key, pair.Value, pair.Value == null ? typeof(string) : pair.Value.GetType());
            }
        }
    }
}