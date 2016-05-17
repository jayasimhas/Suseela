using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels.PopOuts
{
	public class SaveSearchPopOutViewModel : GlassViewModel<IGlassBase>
	{
		public SaveSearchPopOutViewModel(IAuthenticatedUserContext userContext, ITextTranslator textTranslator, ISignInViewModel signInViewModel)
		{
			IsAuthenticated = userContext.IsAuthenticated;
			SignInViewModel = signInViewModel;
			SignInText = textTranslator.Translate("Search.SavePopout.SignInText");
			TitleLabelText = textTranslator.Translate("Search.SavePopout.TitleLabelText");
			DescriptionText = textTranslator.Translate("Search.SavePopout.DescriptionText");
			EmailAlertLabelText = textTranslator.Translate("Search.SavePopout.EmailAlertLabelText");
			ButtonLoggedInText = textTranslator.Translate("Search.SavePopout.ButtonLoggedInText");
			ButtonLoggedOutText = textTranslator.Translate("Search.SavePopout.ButtonLoggedOutText");
			TitleFieldRequiredText = textTranslator.Translate("Search.SavePopout.TitleFieldRequiredText");
		}

		public bool IsAuthenticated { get; set; }
		public ISignInViewModel SignInViewModel { get; set; }
		public string SignInText { get; set; }
		public string TitleLabelText { get; set; }
		public string DescriptionText { get; set; }
		public string EmailAlertLabelText { get; set; }
		public string ButtonLoggedInText { get; set; }
		public string ButtonLoggedOutText { get; set; }
		public string TitleFieldRequiredText { get; set; }
	}
}