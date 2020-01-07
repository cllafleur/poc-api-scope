using Business.UseCase;
using Business.Vacancies;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Adapters;
using WebApp.Models;
using WebApp.Services;
using WebApp.Tools;

namespace WebApp.Controllers
{
    [Authorize]
    public class VacanciesController : ApiController
    {
        private readonly IVacancyService _vacancyService;

        public VacanciesController(IVacancyService vacancyService)
        {
            //_ = vacancyService ?? throw new ArgumentNullException(nameof(vacancyService));

            _vacancyService = vacancyService;
        }

        [CustomerUseCases]
        [Route("api/v1/vacancies")]
        [ResponseType(typeof(VacancyModel[]))]
        public IHttpActionResult GetVacancies()
        {
            var result = _vacancyService.GetVacancies();
            return this.CreateResponse(result.StatusCode, result.Result);
        }

        [CustomerUseCases]
        [Route("api/v1/vacancies/{id}")]
        [ResponseType(typeof(VacancyModel))]
        // GET api/<controller>/5
        public IHttpActionResult GetVacancy(int id)
        {
            var result = _vacancyService.GetVacancy(id);
            return this.CreateResponse(result.StatusCode, result.Result);
        }

        [CustomerUseCases]
        [Route("api/v1/vacancies")]
        [HttpPost]
        [ResponseType(typeof(VacancyModel))]
        // POST api/<controller>
        public IHttpActionResult CreateVacancy()
        {
            var result = _vacancyService.CreateVacancy(this.Request);
            return this.CreateResponse(result.StatusCode, result.Result);
        }

        [CustomerUseCases]
        [Route("api/v1/vacancies/{id}")]
        [HttpPut]
        [ResponseType(typeof(VacancyModel))]
        // PUT api/<controller>/5
        public IHttpActionResult CreateOrUpdateVacancy(int id)
        {
            return this.Ok("");
        }
    }
}