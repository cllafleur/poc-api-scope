using Business.Vacancies;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Adapters;
using WebApp.Models;

namespace WebApp.Tests.Adapters
{
    public class VacancyModelToVacancyAdapterTests
    {
        private Mock<IAdapterFactoryService> _adapterFactoryService;
        private Mock<IMappingPredicateProvider> _mappingPredicateProvider;
        private Mock<IAdapter<JobDescription, JobDescriptionModel>> _jobDescriptionAdapter;
        private VacancyModelToVacancyAdapter _adapterToTest;

        [SetUp]
        public void SetUp()
        {
            _adapterFactoryService = new Mock<IAdapterFactoryService>();
            _mappingPredicateProvider = new Mock<IMappingPredicateProvider>();
            _jobDescriptionAdapter = new Mock<IAdapter<JobDescription, JobDescriptionModel>>();
            _adapterToTest = new VacancyModelToVacancyAdapter(_adapterFactoryService.Object, _mappingPredicateProvider.Object);
        }

        [Test]
        public void Fill_UseCaseCustomer_ReturnsFullModel()
        {
            var resultModel = new Vacancy() { JobDescription = new JobDescription() };
            var origin = new VacancyModel() { Reference = "ref-1", CreationDate = DateTime.Now, AgencyComment = "A comment", JobDescription = new JobDescriptionModel() };

            _adapterFactoryService.Setup(m => m.GetAdapter<JobDescription, JobDescriptionModel>()).Returns(_jobDescriptionAdapter.Object);
            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<VacancyModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<VacancyModel>(Business.UseCase.UseCases.Customer));

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.Reference, Is.EqualTo(origin.Reference));
            Assert.That(resultModel.CreationDate, Is.Not.EqualTo(origin.CreationDate));
            Assert.That(resultModel.AgencyComment, Is.EqualTo(origin.AgencyComment));
        }

        [Test]
        public void Fill_UseCaseJobBoard_ReturnsModel()
        {
            var resultModel = new Vacancy() { AgencyComment = "previous", JobDescription = new JobDescription() };
            var origin = new VacancyModel() { Reference = "ref-1", CreationDate = DateTime.Now, AgencyComment = "A comment", JobDescription = new JobDescriptionModel() };

            _adapterFactoryService.Setup(m => m.GetAdapter<JobDescription, JobDescriptionModel>()).Returns(_jobDescriptionAdapter.Object);
            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<VacancyModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<VacancyModel>(Business.UseCase.UseCases.JobBoard));

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.Reference, Is.EqualTo(origin.Reference));
            Assert.That(resultModel.CreationDate, Is.Not.EqualTo(origin.CreationDate));
            Assert.That(resultModel.AgencyComment, Is.EqualTo("previous"));
        }
    }
}
