using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Description;
using Business.UseCase;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Swashbuckle.Swagger;
using Unity;
using WebApp.Documentation;
using WebApp.Documentation.Contracts;

namespace WebApp.Tests.Documentation
{
    public class SwaggerFilterTests
    {
        private SwaggerFilter _service;

        private Mock<IUseCaseExtractorService> _useCaseExtractorServiceMock;

        private Mock<IPropertiesInfoProvider> _propertiesInfoProviderMock;

        private Mock<ISwaggerDocInspectionService> _swaggerDocInspectionServiceMock;

        [SetUp]
        public void SetUp()
        {
            this._useCaseExtractorServiceMock = new Mock<IUseCaseExtractorService>();
            this._propertiesInfoProviderMock = new Mock<IPropertiesInfoProvider>();
            this._swaggerDocInspectionServiceMock = new Mock<ISwaggerDocInspectionService>();

            UnityConfig.Container.RegisterInstance(_useCaseExtractorServiceMock.Object);
            UnityConfig.Container.RegisterInstance(_propertiesInfoProviderMock.Object);
            UnityConfig.Container.RegisterInstance(_swaggerDocInspectionServiceMock.Object);

            this._service = new SwaggerFilter();
        }

        [Test]
        public void Apply_WhenDocumentIsNull_ShouldReturnWithoutFailing()
        {
            // Arrange
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);
            var apiExplorer = new ApiExplorer(null);

            // Act
            this._service.Apply(null, schemaRegistry, apiExplorer);
        }

        [Test]
        public void Apply_WhenPropertyDoesntHaveUseCaseAttribute_ShouldRemoveDefinitionTypesIfNotUsed()
        {
            // Arrange
            var useCase = UseCases.Customer;
            var swaggerDocument = new SwaggerDocument();
            _useCaseExtractorServiceMock.Setup(m => m.ExtractUseCaseFromSwaggerVersion(swaggerDocument)).Returns(useCase);

            var apiExplorer = new ApiExplorer(null);
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);

            // type 1 settings
            var typeProperties1 = new Dictionary<string, Schema>();
            var type1Prop1Schema = new Schema();
            typeProperties1.Add("prop1", type1Prop1Schema);
            typeProperties1.Add("prop2", new Schema());

            var type1Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type1Props.Add("prop1", new UseCaseAttribute[0]);
            type1Props.Add("prop2", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });
            var schemaType1 = new Schema { properties = typeProperties1 };
            schemaRegistry.Definitions.Add("type1", schemaType1);

            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type1")).Returns(type1Props);

            // type 2 settings
            var typeProperties2 = new Dictionary<string, Schema>();
            typeProperties2.Add("prop1", new Schema());
            var schemaType2 = new Schema { properties = typeProperties2 };
            schemaRegistry.Definitions.Add("type2", schemaType2);
            
            var type2Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type2Props.Add("prop1", new UseCaseAttribute[0]);
            
            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type2")).Returns(type2Props);

            _swaggerDocInspectionServiceMock.Setup(m => m.GetRootTypes(swaggerDocument)).Returns(new HashSet<string>{ "type1" });
            _swaggerDocInspectionServiceMock.Setup(m => m.LoadType(type1Prop1Schema)).Returns("type2");

            // Act
            this._service.Apply(swaggerDocument, schemaRegistry, apiExplorer);

            // Asserts
            Assert.AreEqual(1, schemaRegistry.Definitions.Count);
            Assert.Contains("type1", schemaRegistry.Definitions.Keys.ToArray());

            Assert.AreEqual(1, schemaType1.properties.Count);
            Assert.Contains("prop2", schemaType1.properties.Keys.ToArray());
        }

        [Test]
        public void Apply_WhenPropertyDoesntHaveUseCaseAttribute_ShouldFilterProperties()
        {
            // Arrange
            var useCase = UseCases.Customer;

            var apiExplorer = new ApiExplorer(null);
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);

            var typeProperties1 = new Dictionary<string, Schema>();
            typeProperties1.Add("prop1", new Schema());
            typeProperties1.Add("prop2", new Schema());

            var schemaType1 = new Schema { properties = typeProperties1 };
            schemaRegistry.Definitions.Add("type1", schemaType1);

            var swaggerDocument = new SwaggerDocument();
            _useCaseExtractorServiceMock.Setup(m => m.ExtractUseCaseFromSwaggerVersion(swaggerDocument)).Returns(useCase);

            var type1Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type1Props.Add("prop1", new UseCaseAttribute[0]);
            type1Props.Add("prop2", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });

            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type1")).Returns(type1Props);
            _swaggerDocInspectionServiceMock.Setup(m => m.GetRootTypes(swaggerDocument)).Returns(new HashSet<string> { "type1" });

            // Act
            this._service.Apply(swaggerDocument, schemaRegistry, apiExplorer);

            // Asserts
            Assert.AreEqual(1, schemaRegistry.Definitions.Count);
            Assert.Contains("type1", schemaRegistry.Definitions.Keys.ToArray());

            Assert.AreEqual(1, schemaType1.properties.Count);
            Assert.Contains("prop2", schemaType1.properties.Keys.ToArray());
        }

        [Test]
        public void Apply_WhenPropertyDoesntHaveUseCaseAttribute_ShouldFilterPropertiesRecursively()
        {
            // Arrange
            var useCase = UseCases.JobBoard;

            var apiExplorer = new ApiExplorer(null);
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);

            var typeProperties1 = new Dictionary<string, Schema>();
            var type1Prop1Schema = new Schema();
            var type1Prop2Schema = new Schema();
            var type1Prop4Schema = new Schema();
            var type1Prop3Schema = new Schema();
            typeProperties1.Add("prop1", type1Prop1Schema);
            typeProperties1.Add("prop2", type1Prop2Schema);
            typeProperties1.Add("prop3", type1Prop3Schema);
            typeProperties1.Add("prop4", type1Prop4Schema);

            var typeProperties2 = new Dictionary<string, Schema>();
            var type2Prop1Schema = new Schema();
            var type2Prop2Schema = new Schema();
            typeProperties2.Add("prop1", type2Prop1Schema);
            typeProperties2.Add("prop2", type2Prop2Schema);

            var schemaType1 = new Schema { properties = typeProperties1 };
            schemaRegistry.Definitions.Add("type1", schemaType1);

            var schemaType2 = new Schema { properties = typeProperties2 };
            schemaRegistry.Definitions.Add("type2", schemaType2);

            var swaggerDocument = new SwaggerDocument();
            _useCaseExtractorServiceMock.Setup(m => m.ExtractUseCaseFromSwaggerVersion(swaggerDocument)).Returns(useCase);

            var type1Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type1Props.Add("prop1", new UseCaseAttribute[0]);
            type1Props.Add("prop2", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });
            type1Props.Add("prop3", new UseCaseAttribute[] { new JobBoardUseCaseAttribute(), new CustomerUseCasesAttribute() }); 
            type1Props.Add("prop4", new UseCaseAttribute[] { new JobBoardUseCaseAttribute() });

            var type2Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type2Props.Add("prop1", new UseCaseAttribute[0]);
            type2Props.Add("prop2", new UseCaseAttribute[] { new JobBoardUseCaseAttribute() });

            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type1")).Returns(type1Props);
            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type2")).Returns(type2Props);

            _swaggerDocInspectionServiceMock.Setup(m => m.LoadType(type1Prop4Schema)).Returns("type2");

            _swaggerDocInspectionServiceMock.Setup(m => m.GetRootTypes(swaggerDocument)).Returns(new HashSet<string>{ "type1" });

            // Act
            this._service.Apply(swaggerDocument, schemaRegistry, apiExplorer);

            // Asserts
            Assert.AreEqual(2, schemaRegistry.Definitions.Count);
            Assert.Contains("type1", schemaRegistry.Definitions.Keys.ToArray());
            Assert.Contains("type2", schemaRegistry.Definitions.Keys.ToArray());

            Assert.AreEqual(2, schemaType1.properties.Count);
            Assert.Contains("prop3", schemaType1.properties.Keys.ToArray());
            Assert.Contains("prop4", schemaType1.properties.Keys.ToArray());

            Assert.AreEqual(1, schemaType2.properties.Count);
            Assert.Contains("prop2", schemaType2.properties.Keys.ToArray());
        }

        [Test]
        public void Apply_WhenPropertiesAreRemoved_ShouldRemoveDefinitions()
        {
            // Arrange
            var useCase = UseCases.JobBoard;

            var apiExplorer = new ApiExplorer(null);
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);

            var typeProperties1 = new Dictionary<string, Schema>();
            var type1Prop1Schema = new Schema();
            var type1Prop2Schema = new Schema();
            var type1Prop4Schema = new Schema();
            var type1Prop3Schema = new Schema();
            typeProperties1.Add("prop1", type1Prop1Schema);
            typeProperties1.Add("prop2", type1Prop2Schema);
            typeProperties1.Add("prop3", type1Prop3Schema);
            typeProperties1.Add("prop4", type1Prop4Schema);

            var typeProperties2 = new Dictionary<string, Schema>();
            var type2Prop1Schema = new Schema();
            var type2Prop2Schema = new Schema();
            typeProperties2.Add("prop1", type2Prop1Schema);
            typeProperties2.Add("prop2", type2Prop2Schema);

            var schemaType1 = new Schema { properties = typeProperties1 };
            schemaRegistry.Definitions.Add("type1", schemaType1);

            var schemaType2 = new Schema { properties = typeProperties2 };
            schemaRegistry.Definitions.Add("type2", schemaType2);

            var swaggerDocument = new SwaggerDocument();
            _useCaseExtractorServiceMock.Setup(m => m.ExtractUseCaseFromSwaggerVersion(swaggerDocument)).Returns(useCase);

            var type1Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type1Props.Add("prop1", new UseCaseAttribute[0]);
            type1Props.Add("prop2", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });
            type1Props.Add("prop3", new UseCaseAttribute[] { new JobBoardUseCaseAttribute(), new CustomerUseCasesAttribute() }); 
            type1Props.Add("prop4", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });

            var type2Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type2Props.Add("prop1", new UseCaseAttribute[0]);
            type2Props.Add("prop2", new UseCaseAttribute[] { new JobBoardUseCaseAttribute() });

            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type1")).Returns(type1Props);
            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type2")).Returns(type2Props);

            _swaggerDocInspectionServiceMock.Setup(m => m.LoadType(type1Prop4Schema)).Returns("type2");

            _swaggerDocInspectionServiceMock.Setup(m => m.GetRootTypes(swaggerDocument)).Returns(new HashSet<string> { "type1" });

            // Act
            this._service.Apply(swaggerDocument, schemaRegistry, apiExplorer);

            // Asserts
            Assert.AreEqual(1, schemaRegistry.Definitions.Count);
            Assert.Contains("type1", schemaRegistry.Definitions.Keys.ToArray());

            Assert.AreEqual(1, schemaType1.properties.Count);
            Assert.Contains("prop3", schemaType1.properties.Keys.ToArray());
        }

        [Test]
        public void Apply_WhenPropertiesAreRemoved_ShouldNoteRemoveRootTypes()
        {
            // Arrange
            var useCase = UseCases.JobBoard;

            var apiExplorer = new ApiExplorer(null);
            var schemaRegistry = new SchemaRegistry(new JsonSerializerSettings(), null, null, null, false, null, false, false, false);

            var typeProperties1 = new Dictionary<string, Schema>();
            var type1Prop1Schema = new Schema();
            var type1Prop2Schema = new Schema();
            var type1Prop4Schema = new Schema();
            var type1Prop3Schema = new Schema();
            typeProperties1.Add("prop1", type1Prop1Schema);
            typeProperties1.Add("prop2", type1Prop2Schema);
            typeProperties1.Add("prop3", type1Prop3Schema);
            typeProperties1.Add("prop4", type1Prop4Schema);

            var typeProperties2 = new Dictionary<string, Schema>();
            var type2Prop1Schema = new Schema();
            var type2Prop2Schema = new Schema();
            typeProperties2.Add("prop1", type2Prop1Schema);
            typeProperties2.Add("prop2", type2Prop2Schema);

            var schemaType1 = new Schema { properties = typeProperties1 };
            schemaRegistry.Definitions.Add("type1", schemaType1);

            var schemaType2 = new Schema { properties = typeProperties2 };
            schemaRegistry.Definitions.Add("type2", schemaType2);

            var swaggerDocument = new SwaggerDocument();
            _useCaseExtractorServiceMock.Setup(m => m.ExtractUseCaseFromSwaggerVersion(swaggerDocument)).Returns(useCase);

            var type1Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type1Props.Add("prop1", new UseCaseAttribute[0]);
            type1Props.Add("prop2", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });
            type1Props.Add("prop3", new UseCaseAttribute[] { new JobBoardUseCaseAttribute(), new CustomerUseCasesAttribute() }); 
            type1Props.Add("prop4", new UseCaseAttribute[] { new CustomerUseCasesAttribute() });

            var type2Props = new Dictionary<string, IEnumerable<UseCaseAttribute>>();
            type2Props.Add("prop1", new UseCaseAttribute[0]);
            type2Props.Add("prop2", new UseCaseAttribute[] { new JobBoardUseCaseAttribute() });

            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type1")).Returns(type1Props);
            _propertiesInfoProviderMock.Setup(m => m.GetPropertyInfos("type2")).Returns(type2Props);

            _swaggerDocInspectionServiceMock.Setup(m => m.LoadType(type1Prop4Schema)).Returns("type2");

            _swaggerDocInspectionServiceMock.Setup(m => m.GetRootTypes(swaggerDocument)).Returns(new HashSet<string> { "type1", "type2" });

            // Act
            this._service.Apply(swaggerDocument, schemaRegistry, apiExplorer);

            // Asserts
            Assert.AreEqual(2, schemaRegistry.Definitions.Count);
            Assert.Contains("type1", schemaRegistry.Definitions.Keys.ToArray());
            Assert.Contains("type2", schemaRegistry.Definitions.Keys.ToArray());

            Assert.AreEqual(1, schemaType1.properties.Count);
            Assert.Contains("prop3", schemaType1.properties.Keys.ToArray());

            Assert.AreEqual(1, schemaType2.properties.Count);
            Assert.Contains("prop2", schemaType2.properties.Keys.ToArray());
        }
    }
}