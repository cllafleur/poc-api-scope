using Business.UseCase;
using NUnit.Framework;
using Swashbuckle.Swagger;
using WebApp.Documentation.Services;

namespace WebApp.Tests.Documentation.Services
{
    public class UseCaseExtractorServiceTests
    {
        private UseCaseExtractorService _service;

        [SetUp]
        public void SetUp()
        {
            this._service = new UseCaseExtractorService();
        }

        [TestCase(UseCases.JobBoard)]
        [TestCase(UseCases.Customer)]
        public void ExtractUseCaseFromSwaggerVersion_WhenCalledWithValidVersion_ShouldReturnUseCase(UseCases useCase)
        {
            // Arrange
            var last2Chars = "01";
            var swaggerDocument = new SwaggerDocument { info = new Info { version = useCase+last2Chars }};

            // Act
            var res = this._service.ExtractUseCaseFromSwaggerVersion(swaggerDocument);

            // Assert
            Assert.AreEqual(useCase, res);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("1")]
        [TestCase("12")]
        [TestCase("notAUseCase01")]
        [TestCase("JobBoard")]
        [TestCase("JobBoard000")]
        public void ExtractUseCaseFromSwaggerVersion_WhenCalledWithInvalidVersion_ShouldReturnDefaultUseCase(string version)
        {
            // Arrange
            var swaggerDocument = new SwaggerDocument { info = new Info { version = version } };

            // Act
            var res = this._service.ExtractUseCaseFromSwaggerVersion(swaggerDocument);

            // Assert
            Assert.AreEqual(UseCases.None, res);
        }

        [Test]
        public void ExtractUseCaseFromSwaggerVersion_WhenCalledWithNullDocument_ShouldReturnDefaultUseCase()
        {
            // Act
            var res = this._service.ExtractUseCaseFromSwaggerVersion(null);

            // Assert
            Assert.AreEqual(UseCases.None, res);
        }

        [Test]
        public void ExtractUseCaseFromSwaggerVersion_WhenCalledWithNullDocumentInfo_ShouldReturnDefaultUseCase()
        {
            // Arrange
            var swaggerDocument = new SwaggerDocument { info = null };

            // Act
            var res = this._service.ExtractUseCaseFromSwaggerVersion(swaggerDocument);

            // Assert
            Assert.AreEqual(UseCases.None, res);
        }
    }
}
