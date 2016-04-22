using System.Collections.Generic;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SavedSearchesViewModel : GlassViewModel<I___BasePage>
	{
		public readonly ITextTranslator TextTranslator;
		public readonly IAuthenticatedUserContext UserContext;
		public readonly ISignInViewModel SignInViewModel;
		public readonly ISitecoreService SitecoreService;
		protected readonly IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> SavedSearchService;
		protected readonly ISiteRootContext SiteRootContext;

		public SavedSearchesViewModel(
			ITextTranslator translator,
			IAuthenticatedUserContext userContext,
			ISignInViewModel signInViewModel,
			ISitecoreService sitecoreService,
			IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> savedSearchService,
			ISiteRootContext siteRootContext)
		{
			TextTranslator = translator;
			UserContext = userContext;
			SignInViewModel = signInViewModel;
			SitecoreService = sitecoreService;
			SavedSearchService = savedSearchService;
			SiteRootContext = siteRootContext;
		}

		public IEnumerable<ISavedSearchDisplayable> SavedSearches => SavedSearchService.GetContent();
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string Title => GlassModel?.Title;
		public string GeneralErrorText => TextTranslator.Translate("SavedSearches.GeneralError");
		public string NullUserText => TextTranslator.Translate("SavedSearches.NullUserError");
		public string RequestFailedText => TextTranslator.Translate("SavedSearches.RequestFailedError");
		public string SourceText => TextTranslator.Translate("SavedSearches.SourceText");
		public string TitleText => TextTranslator.Translate("SavedSearches.TitleText");
		public string DateText => TextTranslator.Translate("SavedSearches.DateText");
		public string EmailAlertText => TextTranslator.Translate("SavedSearches.EmailAlertText");
		public string RemoveText => TextTranslator.Translate("SavedSearches.RemoveText");
		public string ItemRemovedMessage => TextTranslator.Translate("SavedSearches.ItemRemovedMessage");
	}
}