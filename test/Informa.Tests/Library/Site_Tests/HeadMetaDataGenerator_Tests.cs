using System;
using System.Collections.Generic;
using System.Text;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Site_Tests
{

    [TestFixture]
    public class HeadMetaDataGenerator_Tests
    {
        HeadMetaDataGenerator _headMetaDataGenerator;
        HeadMetaDataGenerator.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<HeadMetaDataGenerator.IDependencies>();
            _headMetaDataGenerator = new HeadMetaDataGenerator(_dependencies);
        }

        [Test]
        public void GetMetaHtml_CurrentPageIsNotIGlassBase_ReturnsEmptyString()
        {
            // ARRANGE
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns((I___BasePage)null);

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            Assert.AreEqual(result, string.Empty);
        }

        [Test]
        public void TransformToHtml_TakesKeyValuePairs_ReturnsMetaTags()
        {
            // ARRANGE
            var kvCollection = new Dictionary<string, string>
            {
                {"animal", "moose"},
                {"parable", "meandering story that is supposed to be meaningful"}
            };

            // ACT
            string result = _headMetaDataGenerator.TransformToHtml(kvCollection);

            // ASSERT
            var expected = new StringBuilder();
            expected.AppendLine("<meta property=\"animal\" content=\"moose\">");
            expected.AppendLine("<meta property=\"parable\" content=\"meandering story that is supposed to be meaningful\">");
            Assert.AreEqual(expected.ToString(), result);
        }

        [Test]
        public void GetMetaHtml_BasePage_ReturnsBasePageMeta()
        {
            // ARRANGE
            var fakePage = Substitute.For<I___BasePage>();
            fakePage.Title.Returns("Home Title");
            fakePage.Meta_Description.Returns("description of descriptions");
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakePage);

            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            Assert.IsTrue(result.Contains("<meta property=\"og:title\" content=\"Home Title\">"));
            Assert.IsTrue(result.Contains("<meta property=\"twitter:title\" content=\"Home Title\">"));
            Assert.IsTrue(result.Contains("<meta property=\"og:description\" content=\"description of descriptions\">"));
            Assert.IsTrue(result.Contains("<meta property=\"twitter:description\" content=\"description of descriptions\">"));
        }

        [Test]
        public void GetMetaHtml_BaseWithOverrideTitle_ReturnsOverrideTitleMeta()
        {
            // ARRANGE
            var fakePage = Substitute.For<I___BasePage>();
            fakePage.Title.Returns("Home Title");
            fakePage.Meta_Title_Override.Returns("More Awesome Title");
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakePage);

            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            Assert.IsTrue(result.Contains("<meta property=\"og:title\" content=\"More Awesome Title\">"));
        }

        [Test]
        public void GetMetaHtml_AnyPage_ReturnsPubRootMeta()
        {
            // ARRANGE
            var fakePage = Substitute.For<I___BasePage>();
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakePage);

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name = "Moose Monthly";
            fakeRoot.Twitter_Handle = "moose2daMAX";
            fakeRoot.Site_Logo.Returns(Substitute.For<Image>().Alter(x => x.Src = "/bunnies"));
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            Assert.IsTrue(result.Contains("<meta property=\"og:site_name\" content=\"Moose Monthly\">"));
            Assert.IsTrue(result.Contains("<meta property=\"og:image\" content=\"www.moosemonthly.com/bunnies\">"));
            Assert.IsTrue(result.Contains("<meta property=\"twitter:image\" content=\"www.moosemonthly.com/bunnies\">"));
            Assert.IsTrue(result.Contains("<meta property=\"twitter:card\" content=\"www.moosemonthly.com/bunnies\">"));
            Assert.IsTrue(result.Contains("<meta property=\"twitter:site\" content=\"moose2daMAX\">"));
        }

        [Test]
        public void GetMetaHtml_AnyPage_ReturnsGlobalMeta()
        {
            // ARRANGE
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(Substitute.For<I___BasePage>());
            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            Assert.IsTrue(result.Contains("<meta property=\"og:url\" content=\"http://www.moosemonthly.com/\">"));
            Assert.IsTrue(result.Contains("<meta property=\"og:type\" content=\"website\">"));
        }
    }
}