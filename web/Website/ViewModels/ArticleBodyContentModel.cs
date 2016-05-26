﻿using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.ViewModels
{
	public class ArticleBodyContentModel : ArticleEntitledViewModel
	{
		public readonly ICallToActionViewModel CallToActionViewModel;
		protected readonly ITextTranslator TextTranslator;

        private readonly Lazy<string> _lazyBody;

		public ArticleBodyContentModel(IArticle model, IIsEntitledProducItemContext entitledProductContext, ITextTranslator textTranslator, ICallToActionViewModel callToActionViewModel) : base(entitledProductContext)
		{
			TextTranslator = textTranslator;
			CallToActionViewModel = callToActionViewModel;

			Date = Sitecore.Context.PageMode.IsPreview && !model.Planned_Publish_Date.Equals(DateTime.MinValue)
				? model.Planned_Publish_Date
				: model.Actual_Publish_Date;
			Authors = model.Authors.Select(x => new PersonModel(x));
			_lazyBody = new Lazy<string>(() => IsFree || IsEntitled() ? DCDTokenMatchers.ProcessDCDTokens(model.Body) : "");
			Summary = DCDTokenMatchers.ProcessDCDTokens(model.Summary);
		}

		public string Title => GlassModel.Title;
		public string Sub_Title => GlassModel.Sub_Title;
		public bool DisplayLegacyPublication => LegacyPublicationNames.Any();
		public IEnumerable<string> LegacyPublicationNames => GlassModel.Legacy_Publications
					.Select(lp => lp as ITaxonomy_Item)
					.Where(lp => lp != null)
					.Select(lp => lp.Item_Name);
		public string LegacyPublicationText
		{
			get
			{
				var legacyText = TextTranslator.Translate("Article.LegacyPublications");
				var legacyPublicationsText = LegacyPublicationNames.JoinWithFinal(", ", "&");

				return legacyText.Replace("{Legacy Publications}", legacyPublicationsText);
			}
		}
		public DateTime Date { get; set; }
		public IEnumerable<IPersonModel> Authors { get; set; }
		public string Category => GlassModel.Article_Category;
	    public string Body => _lazyBody.Value;
		public string Summary { get; set; }
		public string Content_Type => GlassModel.Content_Type?.Item_Name;
		public string Media_Type => GlassModel.Media_Type?.Item_Name == "Data" ? "chart" : GlassModel.Media_Type?.Item_Name?.ToLower() ?? "";
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
		public string FeaturedImageSource => TextTranslator.Translate("Article.FeaturedImageSource");
	}
}