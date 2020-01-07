using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class SerializableModel : ISerializable
    {
        protected dynamic _internalObject = new ExpandoObject();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var dic = (IDictionary<string, object>)_internalObject;
            foreach (var pair in dic)
            {
                info.AddValue(pair.Key, pair.Value, pair.Value == null ? typeof(string) : pair.Value.GetType());
            }
        }
    }
}