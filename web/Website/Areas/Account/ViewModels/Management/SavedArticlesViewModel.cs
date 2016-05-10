using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Document;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.General_Content;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SavedArticlesViewModel : GlassViewModel<IContent_Text>
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
		public IHtmlString NoSavedDocumentsBody => new HtmlString(GlassModel?.Text ?? string.Empty);
		public string GeneralErrorText => TextTranslator.Translate("SavedDocuments.GeneralError");
		public string NullUserText => TextTranslator.Translate("SavedDocuments.NullUserError");
		public string RequestFailedText => TextTranslator.Translate("SavedDocuments.RequestFailedError");
		public string TitleText => TextTranslator.Translate("SavedDocuments.TitleText");
		public string DateText => TextTranslator.Translate("SavedDocuments.DateText");
		public string RemoveText => TextTranslator.Translate("SavedDocuments.RemoveText");
		public string ItemRemovedMessage => TextTranslator.Translate("SavedDocuments.ItemRemovedMessage");
		public string BadIDText => TextTranslator.Translate("SavedDocuments.BadIDText");
	}
}