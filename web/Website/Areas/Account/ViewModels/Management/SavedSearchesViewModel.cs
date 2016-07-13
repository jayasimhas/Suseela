using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Search;
using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Publication;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SavedSearchesViewModel : GlassViewModel<ISaved_Searches_Page>
	{
		protected readonly ISitePublicationNameContext PublicationNameContext;

		public readonly ISignInViewModel SignInViewModel;
		
		public SavedSearchesViewModel(
			ITextTranslator translator,
			IAuthenticatedUserContext userContext,
			ISignInViewModel signInViewModel,
			IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> savedSearchService,
			ISitePublicationNameContext publicationNameContext)
		{
			PublicationNameContext = publicationNameContext;

			SignInViewModel = signInViewModel;
			
			SavedSearches = savedSearchService.GetContent();
			IsAuthenticated = userContext.IsAuthenticated;
			GeneralErrorText = translator.Translate("SavedSearches.GeneralError");
			NullUserText = translator.Translate("SavedSearches.NullUserError");
			RequestFailedText = translator.Translate("SavedSearches.RequestFailedError");
			SourceText = translator.Translate("SavedSearches.SourceText");
			TitleText = translator.Translate("SavedSearches.TitleText");
			DateText = translator.Translate("SavedSearches.DateText");
			EmailAlertText = translator.Translate("SavedSearches.EmailAlertText");
			RemoveText = translator.Translate("SavedSearches.RemoveText");
		    ItemRemovedMessage = translator.Translate("SavedSearches.ItemRemovedMessage");
            EmailAlertTooltipText = translator.Translate("IconTooltips.Email.EmailAlert");
		}

		public IEnumerable<ISavedSearchDisplayable> SavedSearches { get; set; }
		public bool IsAuthenticated { get; set; }
		public string GeneralErrorText { get; set; }
		public string NullUserText { get; set; }
		public string RequestFailedText { get; set; }
		public string SourceText { get; set; }
		public string TitleText { get; set; }
		public string DateText { get; set; }
		public string EmailAlertText { get; set; }
		public string EmailAlertTooltipSvg { get; set; }
		public string EmailAlertTooltipText { get; set; }
		public string RemoveText { get; set; }
		public string ItemRemovedMessage { get; set; }
		public string Publication => PublicationNameContext.Name;
	}
}