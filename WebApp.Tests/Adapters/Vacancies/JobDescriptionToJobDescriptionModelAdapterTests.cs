using Business.Services;
using Business.Vacancies;
using Microsoft.CSharp.RuntimeBinder;
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
    public class JobDescriptionToJobDescriptionModelAdapterTests
    {
        private Mock<IAdapterFactoryService> _adapterFactoryService;
        private Mock<IMappingPredicateProvider> _mappingPredicateProvider;
        private Mock<IIdToCodeConverterService> _idConverterService;
        private JobDescriptionToJobDescriptionModelAdapter _adapterToTest;

        [SetUp]
        public void SetUp()
        {
            _adapterFactoryService = new Mock<IAdapterFactoryService>();
            _mappingPredicateProvider = new Mock<IMappingPredicateProvider>();
            _idConverterService = new Mock<IIdToCodeConverterService>();
            _adapterToTest = new JobDescriptionToJobDescriptionModelAdapter(_adapterFactoryService.Object, _mappingPredicateProvider.Object, _idConverterService.Object);
        }

        [Test]
        public void Fill_UseCaseCustomer_ReturnsFullModel()
        {
            var resultModel = new JobDescriptionModel();
            var origin = new JobDescription() { ContractId = 1, Description = "desc", JobTitle = "title" };

            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<JobDescriptionModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<JobDescriptionModel>(Business.UseCase.UseCases.Customer));
            _idConverterService.Setup(m => m.GetCode(origin.ContractId)).Returns("1");

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.ContractCode, Is.EqualTo("1"));
            Assert.That(resultModel.Description, Is.EqualTo(origin.Description));
            Assert.That(resultModel.JobTitle, Is.EqualTo(origin.JobTitle));
        }

        [Test]
        public void Fill_UseCaseJobBoard_ReturnsModelWithoutAgencyCommentField()
        {
            var resultModel = new JobDescriptionModel();
            var origin = new JobDescription() { ContractId = 1, Description = "desc", JobTitle = "title" };

            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<JobDescriptionModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<JobDescriptionModel>(Business.UseCase.UseCases.JobBoard));

            _adapterToTest.Fill(resultModel, origin);

            Assert.Throws<RuntimeBinderException>(() => { _ = resultModel.ContractCode; });
            Assert.That(resultModel.Description, Is.EqualTo(origin.Description));
            Assert.That(resultModel.JobTitle, Is.EqualTo(origin.JobTitle));
        }
    }
}
