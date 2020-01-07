using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business.UseCase;


namespace WebApp.Services
{
    public interface IApiRequestContext
    {

        string UserName { get; }

        UseCases UseCase { get; }

        string ClientId { get; }

    }
}