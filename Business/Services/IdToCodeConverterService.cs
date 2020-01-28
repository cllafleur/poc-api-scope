using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public class IdToCodeConverterService : IIdToCodeConverterService
    {
        public string GetCode(int? id)
        {
            return Guid.NewGuid().ToString();
        }

        public int? GetId(string code)
        {
            return new Random().Next();
        }

        public int? GetId(string tableName, string code)
        {
            return new Random().Next();
        }

        public int? GetId<T>(string code)
        {
            return new Random().Next();
        }
    }
}
