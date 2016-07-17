using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.CustomSitecore.Pipelines.HttpRequest;
using NUnit.Framework;
using NSubstitute;

namespace Informa.Tests.Library.CustomSitecore.Pipelines.HttpRequest
{
	[TestFixture]
	public class ArticleItemResolver_Tests
	{
		private string BuildSitecoreRequestItemUrl(string subpath) => $"/sitecore/content/home/{subpath}";

		private ArticleItemResolver _resolver { get; set; }

		[SetUp]
		public void Setup()
		{
			_resolver = new ArticleItemResolver(Substitute.For<IArticleSearch>(), Substitute.For<ISitecoreContext>());
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathNoArticleNumber_ReturnsEmpty()
		{
			string url = BuildSitecoreRequestItemUrl("some folder/some topic page");
			//Assert.AreEqual(null, _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathNoSubpath_ReturnsEmpty()
		{
			string url = BuildSitecoreRequestItemUrl("");
			//Assert.AreEqual(null, _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathFullArticleUrl_ReturnsArticleNumber()
		{
			string url = BuildSitecoreRequestItemUrl("SC092765/Gene-Therapy-Cures-Within-Reach");
			//Assert.AreEqual("SC092765", _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathArticleNumberWithSlash_ReturnsArticleNumber()
		{
			string url = BuildSitecoreRequestItemUrl("SC092765/");
			//Assert.AreEqual("SC092765", _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathArticleNumberWithoutSlash_ReturnsArticleNumber()
		{
			string url = BuildSitecoreRequestItemUrl("SC092765");
			//Assert.AreEqual("SC092765", _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathArticleNumberInArticleName_ReturnsArticleNumber()
		{
			string url = BuildSitecoreRequestItemUrl("SC092765/Some article with FU12345678 in the title");
			//Assert.AreEqual("SC092765", _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathDatePath_and_ArticleName_ReturnsEmpty()
		{
			string url = BuildSitecoreRequestItemUrl("2016/07/07/Some article title");
			//Assert.AreEqual(null, _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}

		[Test]
		public void GetArticleNumberFromRequestItemPathDatePath_and_ArticleNumberInArticleName_ReturnsEmpty()
		{
			string url = BuildSitecoreRequestItemUrl("2016/07/07/Some article with FU123456 in the title");
			//Assert.AreEqual(null, _resolver.GetArticleNumberFromRequestItemPath(url).ArticleNumber);
		}
	}
}
