using System;
using System.IO;
using System.Text;
using System.Xml;
using Autofac;
using AutoMapper;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport;
using Informa.Library.Services.NlmExport.Serialization;
using Informa.Library.Utilities.Autofac.Modules;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Services.NlmExport
{
    [TestFixture]
    public class NlmExportServiceTests
    {
        private NlmExportService _exportService;
        private ISitecoreService _mockService;
        private IItemReferences _mockReferences;

        [SetUp]
        public void Setup()
        {
            var dependencies = Substitute.For<NlmExportService.IDependencies>();
            _mockService = Substitute.For<ISitecoreService>();
            _mockReferences = Substitute.For<IItemReferences>();

            // May as well use real dependencies for testing; more of a system test
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutomapperModule("Informa.Library"));
            builder.Register(c => _mockService).As<ISitecoreService>();
            builder.Register(c => _mockReferences).As<IItemReferences>();
            var container = builder.Build();
            AutofacConfig.ServiceLocator = container;

            dependencies.Mapper.Returns(c => container.Resolve<IMapper>());
            dependencies.Serializer.Returns(c => new NlmSerializer());

            _exportService = new NlmExportService(dependencies);
        }

        [TearDown]
        public void Cleanup()
        {
            // Cleanup static state
            AutofacConfig.ServiceLocator = null;
        }

        [Test]
        public void ExportNlm_WithArticle_IsValid()
        {
            var article = new ArticleItem();

            var stream = _exportService.ExportNlm(article);

            Assert.AreEqual(0, stream.Position);
            Assert.IsTrue(ValidateXml(stream));
        }

        public bool ValidateXml(Stream sourceStream)
        {
            //sourceStream = new FileStream(@"D:\Velir\Informa\web\Website\Util\XSD\INVIVO_2015800056.xml", FileMode.Open);
            StringBuilder errors = new StringBuilder();

            try
            {
                var path =
                    Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\..\\..\\..\\..\\web\\Website\\" +
                                     Constants.DTDPath);

                var resolver = new XmlUrlResolver();
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.DTD,
                    DtdProcessing = DtdProcessing.Parse,
                    XmlResolver = resolver
                };
                settings.ValidationEventHandler += (sender, args) =>
                {
                    if (!args.Message.Contains("The parameter entity replacement text"))
                        errors.AppendLine(args.Message);
                };
                
                using (var reader = XmlReader.Create(sourceStream, settings, path))
                {
                    while (reader.Read())
                    {
                    }
                }

                Assert.IsEmpty(errors.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail("Failed DTD validation, " + ex.Message);
            }

            return true;
        }
    }
}
