using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Library.Services.Article;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.Extensions;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Library.Site;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticleBodyContentModel : ArticleEntitledViewModel
	{
		public readonly ICallToActionViewModel CallToActionViewModel;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IArticleService ArticleService;
        //JIRA IPMP-56
        protected readonly ISiteRootContext SiteRootContext;
        private readonly Lazy<string> _lazyBody;

		public ArticleBodyContentModel(
						IArticle model,
						IIsEntitledProducItemContext entitledProductContext,
						ITextTranslator textTranslator,
						ICallToActionViewModel callToActionViewModel,
						IArticleService articleService,
                        IAuthenticatedUserContext authenticatedUserContext,
                        ISiteRootContext siteRootContext,
                        ISitecoreUserContext sitecoreUserContext)
						: base(entitledProductContext, authenticatedUserContext, sitecoreUserContext)
         {
			TextTranslator = textTranslator;
			CallToActionViewModel = callToActionViewModel;
			ArticleService = articleService;
            // JIRA IPMP-56
            SiteRootContext = siteRootContext;
            _lazyBody = new Lazy<string>(() => IsFree || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated) || IsEntitled(model) ? ArticleService.GetArticleBody(model) : "");
		}

		public string Title => GlassModel.Title;
		public string Sub_Title => GlassModel.Sub_Title;
		public bool DisplayLegacyPublication => LegacyPublicationNames.Any();

		public IEnumerable<string> LegacyPublicationNames => ArticleService.GetLegacyPublicationNames(GlassModel, SiteRootContext.Item.Legacy_Brand_Active);// JIRA IPMP-56

        public string LegacyPublicationText => ArticleService.GetLegacyPublicationText(GlassModel, SiteRootContext.Item.Legacy_Brand_Active,GlassModel.Escenic_ID,GlassModel.Legacy_Article_Number);  // JIRA IPMP-56      

        private string _summary;
		public string Summary => _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));

		private IEnumerable<IPersonModel> _authors;
		public IEnumerable<IPersonModel> Authors
				=> _authors ?? (_authors = GlassModel.Authors.Select(x => new PersonModel(x)));

		private DateTime? _date;
		public DateTime Date
		{
			get
			{
				if (!_date.HasValue)
				{
					_date = GlassModel.GetDate();
				}
				return _date.Value;
			}
		}
		public string Category => GlassModel.Article_Category;
		public string Body => _lazyBody.Value;
		public string ContentType => GlassModel.Content_Type?.Item_Name;
		public MediaTypeIconData MediaTypeIconData => ArticleService.GetMediaTypeIconData(GlassModel);
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
		public string FeaturedImageSource => TextTranslator.Translate("Article.FeaturedImageSource");
        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");

        // JIRA IPMP-56
        public bool IsActiveLegacyBrand => SiteRootContext.Item.Legacy_Brand_Active;
        public List<string> LagacyBrandUrl => ArticleService.GetLegacyPublicationNames(GlassModel, SiteRootContext.Item.Legacy_Brand_Active).ToList<string>();

        public IArticle ArticleItem => GlassModel;
    }
}