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
using Jabberwocky.Glass.Models;
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
        public void AddGlobalMeta_GetsSiteRoot_AddsGlobalProps()
        {
            //ARRANGE
            var props = new Dictionary<string, string>();
            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name = "Moose Monthly";
            fakeRoot.Twitter_Handle = "moose2daMAX";
            fakeRoot.Site_Logo.Returns(Substitute.For<Image>().Alter(x => x.Src = "/Site_Root_Image"));

            //ACT
            _headMetaDataGenerator.AddGlobalMeta(props, uri, fakeRoot);

            //ASSERT
            Assert.Contains(new KeyValuePair<string, string>("og:url", "http://www.moosemonthly.com/"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:type", "website"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:site_name", "Moose Monthly"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:site", "moose2daMAX"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:image", "www.moosemonthly.com/Site_Root_Image"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:image", "www.moosemonthly.com/Site_Root_Image"), props);
        }

        [Test]
        public void AddBasePageMeta_GetsBasePage_ReturnsBasePageMeta()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
            var fakePage = Substitute.For<I___BasePage>();
            fakePage.Meta_Title_Override = "Title Override";
            fakePage.Meta_Description = "Backup Description";

            // ACT
            _headMetaDataGenerator.AddBasePageMeta(props, fakePage);

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("og:description", "Backup Description"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:description", "Backup Description"), props);
        }

        [Test]
        public void AddOpenGraphMeta_GetsTopicPage_ReturnsOgMetaOverridingBase()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
            var fakePage = Substitute.For<ITopic_Page>();
            fakePage.Meta_Title_Override = "Title Override";    //base page
            fakePage.Meta_Description = "Backup Description";   //base page
            fakePage.Og_Title = "Home Title";
            fakePage.Og_Description.Returns("description of descriptions");
            fakePage.Og_Image.Returns(Substitute.For<Image>().Alter(i => i.Src = "/bunnies"));

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Site_Logo.Returns(Substitute.For<Image>().Alter(x => x.Src = "/Site_Root_Image")); //site root meta
            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            _headMetaDataGenerator.AddGlobalMeta(props, uri, fakeRoot);
            _headMetaDataGenerator.AddBasePageMeta(props, fakePage);
            _headMetaDataGenerator.AddOpenGraphMeta(props, fakePage);

            // ASSERT
            Assert.Contains(new KeyValuePair<string,string>("og:title", "Home Title"), props);
            Assert.Contains(new KeyValuePair<string,string>("twitter:title", "Home Title"), props);
            Assert.Contains(new KeyValuePair<string,string>("og:description", "description of descriptions"), props);
            Assert.Contains(new KeyValuePair<string,string>("twitter:description", "description of descriptions"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:image", "www.moosemonthly.com/bunnies"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:image", "www.moosemonthly.com/bunnies"), props);
        }

        [Test]
        public void AddOpenGraphMeta_GetsTopicPageNoOgData_ReturnsBaseMeta()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
            var fakePage = Substitute.For<ITopic_Page>();
            fakePage.Meta_Title_Override = "Title Backup";    //base page
            fakePage.Meta_Description = "Backup Description";   //base page
            fakePage.Og_Title = null;
            fakePage.Og_Description = string.Empty;
            fakePage.Og_Image.Returns(Substitute.For<Image>().Alter(i => i.Src = ""));

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Site_Logo.Returns(Substitute.For<Image>().Alter(x => x.Src = "/Site_Root_Image")); //site root meta
            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            _headMetaDataGenerator.AddGlobalMeta(props, uri, fakeRoot);
            _headMetaDataGenerator.AddBasePageMeta(props, fakePage);
            _headMetaDataGenerator.AddOpenGraphMeta(props, fakePage);

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("og:title", "Title Backup"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:title", "Title Backup"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:description", "Backup Description"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:description", "Backup Description"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:image", "www.moosemonthly.com/Site_Root_Image"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:image", "www.moosemonthly.com/Site_Root_Image"), props);
        }

        [Test]
        public void AddArticleMeta_GetsArticlePage_ReturnsUniqueArticleMeta()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
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

            // ACT
            _headMetaDataGenerator.AddArticleMeta(props, fakeArticle);

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("article:published_time", "2016-06-13T23:36:53.0000000Z"), props);
            Assert.Contains(new KeyValuePair<string, string>("article:modified_time", "2017-06-13T23:36:53.0000000Z"), props);
            Assert.Contains(new KeyValuePair<string, string>("article:author", "Jason Moosterpolis"), props);
            Assert.Contains(new KeyValuePair<string, string>("article:section", "Amazing Mamals"), props);
            Assert.Contains(new KeyValuePair<string, string>("article:tag", "beautiful,powerful,cuddly"), props);
        }

        [Test]
        public void GetMetaHtml_ArticlePage_ReturnsArticleMetaOverrides()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Featured_Image_16_9.Returns(Substitute.For<Image>().Alter(x => x.Src = "/bunnies"));
            fakeArticle.Summary = "Summary Town";
            fakeArticle.Title = "The Fast and the Fluffy";
            _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType:true).Returns(fakeArticle);

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            _headMetaDataGenerator.AddArticleMeta(props, fakeArticle);

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("og:type", "article"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:title", "The Fast and the Fluffy"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:title", "The Fast and the Fluffy"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:image", "www.moosemonthly.com/bunnies"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:image", "www.moosemonthly.com/bunnies"), props);
            Assert.Contains(new KeyValuePair<string, string>("og:description", "Summary Town"), props);
            Assert.Contains(new KeyValuePair<string, string>("twitter:description", "Summary Town"), props);
        }

        [Test]
        public void GetMetaHtml_ArticlePage_ReturnsCommaDelimitedAuthors()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();
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

            _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType:true).Returns(fakeArticle);

            // ACT
            _headMetaDataGenerator.AddArticleMeta(props, fakeArticle);

            // ASSERT
            Assert.Contains(
                new KeyValuePair<string, string>("article:author",
                    "Jason Moosterpolis, Moosfriend McAnimalpal, Lilbunny Mooserider"), props);
        }

        [Test]
        public void GetMetaHtml_TopicPageNoImage_ReturnsTwitterCardIsSummary()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<ITopic_Page>();
            _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType:true).Returns(fakeArticle);

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Site_Logo.Returns(Substitute.For<Image>().Alter(i => i.Src = "/Pantlers"));    //pants, for your antlers!
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("twitter:card", "summary"), result);
        }
        [Test]
        public void GetMetaHtml_ArticlePageWithImage_ReturnsTwitterCardIsSummaryLargeImage()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Featured_Image_16_9.Returns(Substitute.For<Image>().Alter(i => i.Src = "/special_snowflake"));
            _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType: true).Returns(fakeArticle);

            var fakeRoot = Substitute.For<ISite_Root>();
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var uri = new Uri("http://www.moosemonthly.com/");
            _dependencies.HttpContextProvider.RequestUri.Returns(uri);

            // ACT
            var result = _headMetaDataGenerator.BuildPropertyDictionary().ToArray();

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("twitter:card", "summary_large_image"), result);
        }

        [Test]
        public void AddArticleMeta_ArticlePageWithNoDates_ReturnsEmptyDates()
        {
            // ARRANGE
            var props = new Dictionary<string, string>();

            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Planned_Publish_Date.Returns(default(DateTime));
            fakeArticle.Modified_Date.Returns(default(DateTime));
            _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType: true).Returns(fakeArticle);

            // ACT
            _headMetaDataGenerator.AddArticleMeta(props, fakeArticle);

            // ASSERT
            Assert.Contains(new KeyValuePair<string, string>("article:published_time", ""), props);
            Assert.Contains(new KeyValuePair<string, string>("article:modified_time", ""), props);
        }
    }
}