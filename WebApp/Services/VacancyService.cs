using Business.Vacancies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp.Adapters;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.Tools;

namespace WebApp.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IApiRequestContext _requestContext;
        private readonly IAdapterFactoryService _adapterFactoryService;
        private readonly IVacancyRepository _vacancyRepo;

        public VacancyService(IApiRequestContext requestContext, IAdapterFactoryService adapterFactoryService, IVacancyRepository vacancyRepo)
        {
            _requestContext = requestContext;
            _adapterFactoryService = adapterFactoryService;
            _vacancyRepo = vacancyRepo;
        }

        public ApiResult<VacancyModel> CreateVacancy(HttpRequestMessage request)
        {
            var model = new VacancyModel();
            var vacancy = new Vacancy() { JobDescription = new JobDescription() };
            var adapter = _adapterFactoryService.GetAdapter<VacancyModel, Vacancy>();
            adapter.Fill(model, vacancy);
            request.Content.Populate(model).GetAwaiter().GetResult();
            var adapter2 = _adapterFactoryService.GetAdapter<Vacancy, VacancyModel>();
            adapter2.Fill(vacancy, model);

            var updatedModel = new VacancyModel();
            adapter.Fill(updatedModel, vacancy);
            return new ApiResult<VacancyModel>(System.Net.HttpStatusCode.Created, updatedModel);
        }

        public ApiResult<IList<VacancyModel>> GetVacancies()
        {
            var objs = _vacancyRepo.GetVacancies();
            var adapter = _adapterFactoryService.GetAdapter<VacancyModel, Vacancy>();
            var models = new List<VacancyModel>();
            adapter.Fill(models, objs, () => new VacancyModel());
            return new ApiResult<IList<VacancyModel>>(HttpStatusCode.OK, models);

        }

        public ApiResult<VacancyModel> GetVacancy(int id)
        {
            var obj = _vacancyRepo.GetVacancy(id);
            if (obj == null)
                return new ApiResult<VacancyModel>(HttpStatusCode.NotFound, null);
            var adapter = _adapterFactoryService.GetAdapter<VacancyModel, Vacancy>();
            var model = new VacancyModel() { };
            adapter.Fill(model, obj);
            return new ApiResult<VacancyModel>(HttpStatusCode.OK, model);

        }
    }
}