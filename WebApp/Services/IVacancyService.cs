using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.Tools;

namespace WebApp.Services
{
    public interface IVacancyService
    {
        ApiResult<IList<VacancyModel>> GetVacancies();
        ApiResult<VacancyModel> GetVacancy(int id);
        ApiResult<VacancyModel> CreateVacancy(HttpRequestMessage request);
    }
}