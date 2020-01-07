using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public interface IIdToCodeConverterService
    {
        int? GetId(string code);

        string GetCode(int? id);
    }
}
