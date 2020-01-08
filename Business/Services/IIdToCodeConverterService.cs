using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IIdToCodeConverterService
    {
        int? GetId(string code);

        int? GetId<T>(string code);

        int? GetId(string tableName, string code);

        string GetCode(int? id);
    }
}
