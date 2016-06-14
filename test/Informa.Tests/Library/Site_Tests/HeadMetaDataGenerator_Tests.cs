using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
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
        public void GetMetaHtml_NullPage_LogsErrorBackendAndFront()
        {
            // ARRANGE
            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns((I___BasePage)null);

            var uri = new Uri("http://www.moosemonthly.com/buggypage/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.GetMetaHtml();

            // ASSERT
            _dependencies.LogWrapper.Received(1).SitecoreError(Arg.Any<string>());
            Assert.AreEqual(result, "<script type='text/javascript'>if(console){console.error('Failed to load page metadata.')}</script>");
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
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string,string>("og:title", "Home Title"), result);
            Assert.Contains(new KeyValuePair<string,string>("twitter:title", "Home Title"), result);
            Assert.Contains(new KeyValuePair<string,string>("og:description", "description of descriptions"), result);
            Assert.Contains(new KeyValuePair<string,string>("twitter:description", "description of descriptions"), result);
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
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string,string>("og:title", "More Awesome Title"), result);
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
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string,string>("og:site_name", "Moose Monthly"), result);
            Assert.Contains(new KeyValuePair<string,string>("og:image", "www.moosemonthly.com/bunnies"), result);
            Assert.Contains(new KeyValuePair<string,string>("twitter:image", "www.moosemonthly.com/bunnies"), result);
            Assert.Contains(new KeyValuePair<string,string>("twitter:card", "www.moosemonthly.com/bunnies"), result);
            Assert.Contains(new KeyValuePair<string,string>("twitter:site", "moose2daMAX"), result);
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
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string,string>("og:url", "http://www.moosemonthly.com/"), result);
            Assert.Contains(new KeyValuePair<string,string>("og:type", "website"), result);
        }

        [Test]
        public void GetMetaHtml_ArticlePage_ReturnsUniqueArticleMeta()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Actual_Publish_Date.Returns(DateTime.Parse("2016-06-13T23:36:53Z"));
            fakeArticle.Modified_Date.Returns(DateTime.Parse("2017-06-13T23:36:53Z"));

            var fakeAuthor = Substitute.For<IStaff_Item>();
            fakeAuthor.First_Name = "Jason";
            fakeAuthor.Last_Name = "Moosterpolis";
            fakeArticle.Authors.Returns(new[] {fakeAuthor});

            var fakeTax1 = Substitute.For<ITaxonomy_Item>().Alter(x => x.Item_Name = "Amazing Mamals");
            var fakeTax2 = Substitute.For<ITaxonomy_Item>().Alter(x => x.Item_Name = "beautiful");
            var fakeTax3 = Substitute.For<ITaxonomy_Item>().Alter(x => x.Item_Name = "powerful");
            var fakeTax4 = Substitute.For<ITaxonomy_Item>().Alter(x => x.Item_Name = "cuddly");
            fakeArticle.Taxonomies.Returns(new[] {fakeTax1, fakeTax2, fakeTax3, fakeTax4});

            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakeArticle);
            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("article:published_time", "2016-06-13T23:36:53.0000000Z"), result);
            Assert.Contains(new KeyValuePair<string, string>("article:modified_time", "2017-06-13T23:36:53.0000000Z"), result);
            Assert.Contains(new KeyValuePair<string, string>("article:author", "Jason Moosterpolis"), result);
            Assert.Contains(new KeyValuePair<string, string>("article:section", "Amazing Mamals"), result);
            Assert.Contains(new KeyValuePair<string, string>("article:tag", "beautiful,powerful,cuddly"), result);
        }

        [Test]
        public void GetMetaHtml_ArticlePage_ReturnsAlteredArticleMeta()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Featured_Image_16_9.Returns(Substitute.For<Image>().Alter(x => x.Src = "/bunnies"));
            fakeArticle.Summary = "Summary Town";

            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakeArticle);
            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("og:type", "article"), result);
            Assert.Contains(new KeyValuePair<string, string>("og:image", "www.moosemonthly.com/bunnies"), result);
            Assert.Contains(new KeyValuePair<string, string>("twitter:image", "www.moosemonthly.com/bunnies"), result);
            Assert.Contains(new KeyValuePair<string, string>("og:description", "Summary Town"), result);
            Assert.Contains(new KeyValuePair<string, string>("twitter:description", "Summary Town"), result);
        }

        [Test]
        public void GetMetaHtml_ArticlePage_ReturnsCommaDelimitedAuthors()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Jason";
            fakeAuthor1.Last_Name = "Moosterpolis";
            var fakeAuthor2 = Substitute.For<IStaff_Item>();
            fakeAuthor2.First_Name = "Moosfriend";
            fakeAuthor2.Last_Name = "McAnimalpal";
            var fakeAuthor3 = Substitute.For<IStaff_Item>();
            fakeAuthor3.First_Name = "Lilbunny";
            fakeAuthor3.Last_Name = "Mooserider";

            fakeArticle.Authors.Returns(new[] { fakeAuthor1, fakeAuthor2, fakeAuthor3 });

            _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>().Returns(fakeArticle);
            _dependencies.SiteRootContext.Item.Returns(Substitute.For<ISite_Root>());

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(
                new KeyValuePair<string, string>("article:author",
                    "Jason Moosterpolis, Moosfriend McAnimalpal, Lilbunny Mooserider"), result);
        }
    }
}