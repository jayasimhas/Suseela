using System.Linq;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Site;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleListItemModelFactory : IArticleListItemModelFactory
	{
		protected readonly ISiteRootContext SiteRootContext;

		public ArticleListItemModelFactory(
			ISiteRootContext siteRootContext)
		{
			SiteRootContext = siteRootContext;
		}

		public IListable Create(IArticle article)
		{
			if (article == null)
			{
				return null;
			}

			var publication = SiteRootContext?.Item?.Publication_Name?.StripHtml();

			return new ArticleListItemModel
			{
				ListableAuthors = article.Authors?.Select(x => new LinkableModel { LinkableText = x.First_Name + " " + x.Last_Name }),
				ListableDate = article.Actual_Publish_Date,
				ListableImage = article.Featured_Image_16_9?.Src,
				ListableSummary = article.Summary,
				ListableTitle = article.Title,
				ListableByline = publication,
				ListableTopics = article.Taxonomies?.Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = "/Search?QueryParam" }),
				ListableType = article.Media_Type?.Item_Name == "Data" ? "chart" : article.Media_Type?.Item_Name?.ToLower() ?? "",
				ListableUrl = new Link { Url = article._Url, Text = article.Title },
				LinkableText = article.Content_Type?.Item_Name,
				LinkableUrl = article._Url,
				Publication = publication
			};
		}
	}
}