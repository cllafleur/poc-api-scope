using Business.Vacancies;
using Microsoft.CSharp.RuntimeBinder;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Adapters;
using WebApp.Models;

namespace WebApp.Tests.Adapters
{
    public class VacancyToVacancyModelAdapterTests
    {
        private Mock<IAdapterFactoryService> _adapterFactoryService;
        private Mock<IMappingPredicateProvider> _mappingPredicateProvider;
        private Mock<IAdapter<JobDescriptionModel, JobDescription>> _jobDescriptionAdapter;
        private VacancyToVacancyModelAdapter _adapterToTest;

        [SetUp]
        public void SetUp()
        {
            _adapterFactoryService = new Mock<IAdapterFactoryService>();
            _mappingPredicateProvider = new Mock<IMappingPredicateProvider>();
            _jobDescriptionAdapter = new Mock<IAdapter<JobDescriptionModel, JobDescription>>();
            _adapterToTest = new VacancyToVacancyModelAdapter(_adapterFactoryService.Object, _mappingPredicateProvider.Object);
        }

        [Test]
        public void Fill_UseCaseCustomer_ReturnsModel()
        {
            var resultModel = new VacancyModel() { JobDescription = new JobDescriptionModel() };
            var origin = new Vacancy() { Reference = "ref-1", CreationDate = DateTime.Now, AgencyComment = "A comment", JobDescription = new JobDescription() };

            _adapterFactoryService.Setup(m => m.GetAdapter<JobDescriptionModel, JobDescription>()).Returns(_jobDescriptionAdapter.Object);
            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<VacancyModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<VacancyModel>(Business.UseCase.UseCases.Customer));

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.Reference, Is.EqualTo(origin.Reference));
            Assert.That(resultModel.CreationDate, Is.EqualTo(origin.CreationDate));
            Assert.That(resultModel.AgencyComment, Is.EqualTo(origin.AgencyComment));
            Assert.That(resultModel.JobDescription, Is.Not.Null);
        }

        [Test]
        public void Fill_UseCaseJobBoard_ReturnsModelWithoutAgencyCommentField()
        {
            var resultModel = new VacancyModel();
            var origin = new Vacancy() { Reference = "ref-1", CreationDate = DateTime.Now, AgencyComment = "A comment", JobDescription = new JobDescription() };

            _adapterFactoryService.Setup(m => m.GetAdapter<JobDescriptionModel, JobDescription>()).Returns(_jobDescriptionAdapter.Object);
            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<VacancyModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<VacancyModel>(Business.UseCase.UseCases.JobBoard));

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.Reference, Is.EqualTo(origin.Reference));
            Assert.That(resultModel.CreationDate, Is.EqualTo(origin.CreationDate));
            Assert.Throws<RuntimeBinderException>(() => { _ = resultModel.AgencyComment; });
            Assert.That(resultModel.JobDescription, Is.Not.Null);
        }
    }
}
