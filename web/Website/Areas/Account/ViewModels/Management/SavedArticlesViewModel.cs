using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using Informa.Library.Services.Article;
using Informa.Library.Site;
using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SavedArticlesViewModel : GlassViewModel<ISaved_Articles_Page>
	{
		public readonly ITextTranslator TextTranslator;
		public readonly IAuthenticatedUserContext UserContext;
		public readonly ISignInViewModel SignInViewModel;
		protected readonly ISavedDocumentItemsContext SavedDocumentItemsContext;

		public SavedArticlesViewModel(
			ITextTranslator translator,
			IAuthenticatedUserContext userContext,
			ISignInViewModel signInViewModel,
			ISavedDocumentItemsContext savedDocumentItemsContext)
		{
			TextTranslator = translator;
			UserContext = userContext;
			SignInViewModel = signInViewModel;
			SavedDocumentItemsContext = savedDocumentItemsContext;
		}
		public IEnumerable<ISavedDocumentItem> SavedDocumentItems => SavedDocumentItemsContext.SavedDocumentItems;
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public IHtmlString NoSavedDocumentsBody => new HtmlString(GlassModel?.No_Articles ?? string.Empty);
		public string GeneralErrorText => TextTranslator.Translate("SavedDocuments.GeneralError");
		public string NullUserText => TextTranslator.Translate("SavedDocuments.NullUserError");
		public string RequestFailedText => TextTranslator.Translate("SavedDocuments.RequestFailedError");
		public string TitleText => TextTranslator.Translate("SavedDocuments.TitleText");
		public string DateText => TextTranslator.Translate("SavedDocuments.DateText");
		public string RemoveText => TextTranslator.Translate("SavedDocuments.RemoveText");
		public string ItemRemovedMessage => TextTranslator.Translate("SavedDocuments.ItemRemovedMessage");
		public string BadIDText => TextTranslator.Translate("SavedDocuments.BadIDText");
		public string RemoveModalCancelText => TextTranslator.Translate("SavedDocuments.RemoveModalCancel");
		public string RemoveModalConfirmText => TextTranslator.Translate("SavedDocuments.RemoveModalConfirm");
		public string RemoveModalText => TextTranslator.Translate("SavedDocuments.RemoveModalText");
		public string RemoveModalTitleText => TextTranslator.Translate("SavedDocuments.RemoveModalTitle");

	}
}