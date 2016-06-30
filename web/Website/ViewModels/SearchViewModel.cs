using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class SearchViewModel : GlassViewModel<ISearch>
	{
		private readonly Lazy<IEnumerable<string>> _subcriptions; 

		protected readonly ITextTranslator TextTranslator;

		public SearchViewModel(
            ITextTranslator textTranslator, 
            IAuthenticatedUserContext userContext, 
            IUserSubscriptionsContext subscriptionsContext)
		{
			TextTranslator = textTranslator;
			IsAuthenticated = userContext.IsAuthenticated;

			_subcriptions = new Lazy<IEnumerable<string>>(() =>
			{
				return userContext.IsAuthenticated
					? subscriptionsContext.Subscriptions?.Select(s => s.Publication) ?? Enumerable.Empty<string>()
					: Enumerable.Empty<string>();
			});
		}
		public bool IsAuthenticated { get; set; }
		public string PageFirstText => TextTranslator.Translate("Search.Page.First");
		public string PageLastText => TextTranslator.Translate("Search.Page.Last");
		public string SearchTipsText => TextTranslator.Translate("Search.Tips");
		public string SearchTitleText => TextTranslator.Translate("Search.Title");
		public string SearchSearchHeadlinesOnlyText => TextTranslator.Translate("Search.SearchHeadlinesOnly");
		public string SearchViewHeadlinesOnlyText => TextTranslator.Translate("Search.ViewHeadlinesOnly");
		public string SearchSortByText => TextTranslator.Translate("Search.SortBy");
		public string SearchRelevanceText => TextTranslator.Translate("Search.Relevancy");
		public string SearchShowingResults1Text => TextTranslator.Translate("Search.ShowingResults1");
		public string SearchShowingResults2Text => TextTranslator.Translate("Search.ShowingResults2");
		public string SearchShowingResults3Text => TextTranslator.Translate("Search.ShowingResults3");
		public string SearchShowingResults4Text => TextTranslator.Translate("Search.ShowingResults4");
		public string SearchClearAllText => TextTranslator.Translate("Search.ClearAll");
		public string SearchFilterByText => TextTranslator.Translate("Search.FilterBy");
		public string SearchSelectMySubscriptionsText => TextTranslator.Translate("Search.SelectMySubscriptions");
		public string SearchShowAllPublicationsText => TextTranslator.Translate("Search.ShowAllPublications");
		public IEnumerable<string> Subcriptions => _subcriptions.Value;
	}
}