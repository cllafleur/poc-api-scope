using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Tools
{
    public static class HttpContentExtensions
    {
        public static async Task Populate<T>(this HttpContent requestContent, T model) where T : class
        {
            _ = requestContent ?? throw new ArgumentNullException(nameof(requestContent));
            _ = model ?? throw new ArgumentNullException(nameof(model));

            Stream inputStream = await requestContent.ReadAsStreamAsync().ConfigureAwait(false);
            if (inputStream.CanSeek)
            {
                inputStream.Position = 0;
            }

            StreamReader inputStreamReader = new StreamReader(inputStream);
            var serializer = new JsonSerializer();
            serializer.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
            {
                IgnoreSerializableAttribute = true,
                IgnoreSerializableInterface = true
            };
            serializer.Populate(inputStreamReader, model);
        }
    }
}