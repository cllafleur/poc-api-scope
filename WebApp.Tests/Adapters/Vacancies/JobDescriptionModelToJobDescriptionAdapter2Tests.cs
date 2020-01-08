using Business.Services;
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
    public class JobDescriptionModelToJobDescriptionAdapter2Tests
    {
        private Mock<IAdapterFactoryService> _adapterFactoryService;
        private Mock<IMappingPredicateProvider> _mappingPredicateProvider;
        private Mock<IIdToCodeConverterService> _idConverterService;
        private JobDescriptionModelToJobDescriptionAdapter2 _adapterToTest;

        [SetUp]
        public void Setup()
        {
            _adapterFactoryService = new Mock<IAdapterFactoryService>();
            _mappingPredicateProvider = new Mock<IMappingPredicateProvider>();
            _idConverterService = new Mock<IIdToCodeConverterService>();
            _adapterToTest = new JobDescriptionModelToJobDescriptionAdapter2(_adapterFactoryService.Object, _mappingPredicateProvider.Object, _idConverterService.Object);
        }

        [Test]
        public void Fill_UseCaseCustomer_ReturnsModel()
        {
            var resultModel = new JobDescription();
            var origin = new JobDescriptionModel() { ContractCode = "1", Description = "desc", JobTitle = "title" };

            _mappingPredicateProvider.Setup(m => m.GetPredicate<JobDescriptionModel>()).Returns(FakeMappingPredicateProvider.GetPredicate<JobDescriptionModel>(Business.UseCase.UseCases.Customer));
            _idConverterService.Setup(m => m.GetId(It.IsAny<string>(), origin.ContractCode)).Returns(1);

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.ContractId, Is.EqualTo(1));
            Assert.That(resultModel.Description, Is.EqualTo(origin.Description));
            Assert.That(resultModel.JobTitle, Is.EqualTo(origin.JobTitle));
        }

        [Test]
        public void Fill_UseCaseCustomer_ContractThrowsException_ReturnsModel()
        {
            var resultModel = new JobDescription() { ContractId = 98 };
            var origin = new JobDescriptionModel() { ContractCode = "1", Description = "desc", JobTitle = "title" };

            _mappingPredicateProvider.Setup(m => m.GetPredicate<JobDescriptionModel>()).Returns(FakeMappingPredicateProvider.GetPredicate<JobDescriptionModel>(Business.UseCase.UseCases.Customer));
            _idConverterService.Setup(m => m.GetId(origin.ContractCode)).Throws<ArgumentNullException>();

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.ContractId, Is.EqualTo(98));
            Assert.That(resultModel.Description, Is.EqualTo(origin.Description));
            Assert.That(resultModel.JobTitle, Is.EqualTo(origin.JobTitle));
        }

        [Test]
        public void Fill_UseCaseJobBoard_ReturnsFullModel()
        {
            var resultModel = new JobDescription() { ContractId = 2 };
            var origin = new JobDescriptionModel() { ContractCode = "1", Description = "desc", JobTitle = "title" };

            _mappingPredicateProvider
                .Setup(m => m.GetPredicate<JobDescriptionModel>())
                .Returns(FakeMappingPredicateProvider.GetPredicate<JobDescriptionModel>(Business.UseCase.UseCases.JobBoard));
            _idConverterService.Setup(m => m.GetId(It.IsAny<string>(), origin.ContractCode)).Returns(1);

            _adapterToTest.Fill(resultModel, origin);

            Assert.That(resultModel.ContractId, Is.EqualTo(2));
            Assert.That(resultModel.Description, Is.EqualTo(origin.Description));
            Assert.That(resultModel.JobTitle, Is.EqualTo(origin.JobTitle));
        }
    }
}
