using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.ViewModels.Account;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.PopOuts
{
	public class SaveSearchPopOutViewModel : GlassViewModel<IGlassBase>
	{
		private readonly ITextTranslator _textTranslator;

		public SaveSearchPopOutViewModel(IGlassBase model, IAuthenticatedUserContext userContext, ITextTranslator textTranslator, ISignInViewModel signInViewModel)
		{
			_textTranslator = textTranslator;

			IsAuthenticated = userContext.IsAuthenticated;
			IsSearch = model is ISearch;
			SignInViewModel = signInViewModel;

			TitleLabelText = textTranslator.Translate("Search.SavePopout.TitleLabelText");
			ButtonLoggedInText = textTranslator.Translate("Search.SavePopout.ButtonLoggedInText");
			ButtonLoggedOutText = textTranslator.Translate("Search.SavePopout.ButtonLoggedOutText");
		}

		public bool IsAuthenticated { get; set; }
		public bool IsSearch { get; set; }
		public ISignInViewModel SignInViewModel { get; set; }
		public string SignInText => _textTranslator.Translate("Search.SavePopout.SignInText");
		public string TitleLabelText { get; set; }
		public string TitleFieldRequiredText => _textTranslator.Translate("Search.SavePopout.TitleFieldRequiredText");
		public string SearchDescriptionText => _textTranslator.Translate("Search.SavePopout.DescriptionText");
		public string TopicDescriptionText => _textTranslator.Translate("Topic.SavePopout.DescriptionText");
		public string EmailAlertLabelText => _textTranslator.Translate("Search.SavePopout.EmailAlertLabelText");
		public string ButtonLoggedInText { get; set; }
		public string ButtonLoggedOutText { get; set; }
	}
}