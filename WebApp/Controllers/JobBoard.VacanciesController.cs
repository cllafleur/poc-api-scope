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
    public class JobBoardVacanciesController : ApiController
    {
        private readonly IVacancyService _vacancyService;

        public JobBoardVacanciesController(IVacancyService vacancyService)
        {
            //_ = vacancyService ?? throw new ArgumentNullException(nameof(vacancyService));

            _vacancyService = vacancyService;
        }

        [JobBoardUseCase]
        [Route("api/jobboard/v1/vacancies")]
        [ResponseType(typeof(VacancyModel[]))]
        public IHttpActionResult GetVacanciesAsJobBoard()
        {
            var result = _vacancyService.GetVacancies();
            return this.CreateResponse(result.StatusCode, result.Result);
        }

        [JobBoardUseCase]
        [Route("api/jobboard/v1/vacancies/{id}")]
        [ResponseType(typeof(VacancyModel))]
        // GET api/<controller>/5
        public IHttpActionResult GetVacancyAsJobBoard(int id)
        {
            var result = _vacancyService.GetVacancy(id);
            return this.CreateResponse(result.StatusCode, result.Result);
        }

        [JobBoardUseCase]
        [Route("api/jobboard/v1/vacancies")]
        [HttpPost]
        [ResponseType(typeof(VacancyModel))]
        // POST api/<controller>
        public IHttpActionResult CreateVacancyAsJobBoard()
        {
            var result = _vacancyService.CreateVacancy(this.Request);
            return this.CreateResponse(result.StatusCode, result.Result);
        }
    }
}