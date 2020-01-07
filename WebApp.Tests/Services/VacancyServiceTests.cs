using Business.UseCase;
using Business.Vacancies;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Unity.Lifetime;
using WebApp.Adapters;
using WebApp.Controllers;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Tests.Services
{
    public class VacancyServiceTests
    {
        private Mock<IApiRequestContext> _apiRequestContextMock;
        private Mock<IVacancyRepository> _vacancyRepositoryMock;
        private Mock<IAdapterFactoryService> _adapterFactoryServiceMock;
        private VacancyService _serviceToTest;

        [SetUp]
        public void Setup()
        {
            _apiRequestContextMock = new Mock<IApiRequestContext>();
            _vacancyRepositoryMock = new Mock<IVacancyRepository>();
            _adapterFactoryServiceMock = new Mock<IAdapterFactoryService>();
            UnityConfig.Container.RegisterInstance(typeof(IApiRequestContext), "context", _apiRequestContextMock.Object, new SingletonLifetimeManager());
            _serviceToTest = new VacancyService(_apiRequestContextMock.Object, _adapterFactoryServiceMock.Object, _vacancyRepositoryMock.Object);
        }

        [Test]
        public void UseCaseCustomer_GetVacancies_ReturnsFullModel()
        {
            _vacancyRepositoryMock.Setup(m => m.GetVacancies()).Returns(
                new[] {
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-1", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 1", Description="Come !!!" } },
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-2", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 2", Description="Come !!!" } },
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-3", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 3", Description="Come !!!" } },
                });

            _apiRequestContextMock.Setup(m => m.UseCase).Returns(UseCases.Customer);
            var adapterMock = new Mock<IAdapter<VacancyModel, Vacancy>>();
            adapterMock
                .Setup(m => m.Fill(It.IsAny<ICollection<VacancyModel>>(), It.IsAny<ICollection<Vacancy>>(), It.IsAny<Func<VacancyModel>>()))
                .Callback<ICollection<VacancyModel>, ICollection<Vacancy>, Func<VacancyModel>>((m, s, b) => m.Add(new VacancyModel() { Reference = "ref-1"}));
            _adapterFactoryServiceMock.Setup(m => m.GetAdapter<VacancyModel, Vacancy>()).Returns(adapterMock.Object);

            var result = _serviceToTest.GetVacancies();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var jsonResult = JsonConvert.SerializeObject(result.Result);
            Assert.That(jsonResult, Is.EqualTo("[{\"Reference\":\"ref-1\"}]"));
        }

        [Test]
        public void UseCaseCustomer_GetVacanciesWithSubObjects_ReturnsFail()
        {
            _vacancyRepositoryMock.Setup(m => m.GetVacancies()).Returns(
                new[] {
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-1", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 1", Description="Come !!!" } },
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-2", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 2", Description="Come !!!" } },
                    new Vacancy(){CreationDate = DateTime.Now, Reference = "2019-3", AgencyComment= "Super comment", JobDescription = new JobDescription(){ ContractId=1, JobTitle="Offer 3", Description="Come !!!" } },
                });

            _apiRequestContextMock.Setup(m => m.UseCase).Returns(UseCases.Customer);
            var adapterMock = new Mock<IAdapter<VacancyModel, Vacancy>>();
            adapterMock
                .Setup(m => m.Fill(It.IsAny<ICollection<VacancyModel>>(), It.IsAny<ICollection<Vacancy>>(), It.IsAny<Func<VacancyModel>>()))
                .Callback<ICollection<VacancyModel>, ICollection<Vacancy>, Func<VacancyModel>>((m, s, b) => m.Add(new VacancyModel() { Reference = "ref-1", JobDescription = new JobDescriptionModel() }));
            _adapterFactoryServiceMock.Setup(m => m.GetAdapter<VacancyModel, Vacancy>()).Returns(adapterMock.Object);

            var result = _serviceToTest.GetVacancies();
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var jsonResult = JsonConvert.SerializeObject(result.Result);
            Assert.That(jsonResult, Is.EqualTo("[{\"Reference\":\"ref-1\",\"JobDescription\":{}}]"));
        }
    }
}
