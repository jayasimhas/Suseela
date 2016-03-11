using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Site;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ArticleListItemModelFactory : IArticleListItemModelFactory
	{
		protected readonly ISiteRootContext SiteRootContext;
	    protected readonly ISitecoreContext SitecoreContext;
	    protected readonly IArticleSearch ArticleSearch;

		public ArticleListItemModelFactory(
			ISiteRootContext siteRootContext, ISitecoreContext sitecoreContext, IArticleSearch articleSearch)
		{
			SiteRootContext = siteRootContext;
		    SitecoreContext = sitecoreContext;
		    ArticleSearch = articleSearch;
		}

		public IListableViewModel Create(IArticle article)
		{
			if (article == null)
			{
				return null;
			}

			var publication = SiteRootContext?.Item?.Publication_Name?.StripHtml();
			var image = article.Featured_Image_16_9?.Src;

			return new ArticleListItemModel
			{
				DisplayImage = !string.IsNullOrWhiteSpace(image),
				ListableAuthors = article.Authors?.Select(x => new LinkableModel { LinkableText = x.First_Name + " " + x.Last_Name }),
				ListableDate = article.Actual_Publish_Date,
				ListableImage = image,
				ListableSummary = article.Summary,
				ListableTitle = article.Title,
				ListableByline = publication,
				ListableTopics = article.Taxonomies?.Select(x => new LinkableModel { LinkableText = x.Item_Name, LinkableUrl = $"/Search?tag={x._Id}" }),
				ListableType = article.Media_Type?.Item_Name == "Data" ? "chart" : article.Media_Type?.Item_Name?.ToLower() ?? "",
				ListableUrl = new Link { Url = article._Url, Text = article.Title },
				LinkableText = article.Content_Type?.Item_Name,
				LinkableUrl = article._Url,
				Publication = publication
			};
		}

	    public IListableViewModel Create(Guid articleId)
	    {
	        return Create(SitecoreContext.GetItem<IArticle>(articleId));
	    }

	    public IListableViewModel Create(string articleNumber)
	    {
            IArticleSearchFilter filter = ArticleSearch.CreateFilter();
            filter.ArticleNumber = articleNumber;
	        var results = ArticleSearch.Search(filter);
            if (results.Articles.Any())
            {
                return Create(results.Articles.FirstOrDefault());
            }

	        return null;
	    }
	}
}