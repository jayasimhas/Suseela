using Informa.Library.Globalization;
using Informa.Library.User.Profile;
using Informa.Library.User.ResetPassword;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.ResetPassword
{
	public class ChangeViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IFindUserResetPassword FindUserResetPassword;

		public ChangeViewModel(
			ITextTranslator textTranslator,
			IFindUserProfileByUsername findUserProfile,
			IFindUserResetPassword findUserResetPassword)
		{
			TextTranslator = textTranslator;
			FindUserProfile = findUserProfile;
			FindUserResetPassword = findUserResetPassword;
		}

		protected IUserResetPassword UserResetPassword
		{
			get
			{
				return FindUserResetPassword.Find(Token);
			}
		}

		public string Title => GlassModel?.Title;
		public IHtmlString ResetBody
		{
			get
			{
				var userProfile = FindUserProfile.Find(UserResetPassword?.Username ?? string.Empty);
				var body = GlassModel?.Body ?? string.Empty;

				if (userProfile != null && !string.IsNullOrWhiteSpace(body))
				{
					body = body.Replace("#first_name#", userProfile.FirstName).Replace("#last_name#", userProfile.LastName);
				}

				return new MvcHtmlString(body);
			}
		}
		public string NewPasswordLabelText => TextTranslator.Translate("Authentication.ResetPassword.NewPasswordLabel");
		public string NewPasswordPlaceholderText => TextTranslator.Translate("Authentication.ResetPassword.NewPasswordPlaceholder");
		public string NewPasswordRepeatLabelText => TextTranslator.Translate("Authentication.ResetPassword.NewPasswordRepeatLabel");
		public string NewPasswordRepeatPlaceholderText => TextTranslator.Translate("Authentication.ResetPassword.NewPasswordRepeatPlaceholder");
		public string SubmitButtonText => TextTranslator.Translate("Authentication.ResetPassword.SubmitButton");
		public string ResetErrorRequirementsText => TextTranslator.Translate("Authentication.ResetPassword.ResetErrorRequirements");
		public string ResetErrorMismatchText => TextTranslator.Translate("Authentication.ResetPassword.ResetErrorMismatch");
		public string ResetErrorGeneralText => TextTranslator.Translate("Authentication.ResetPassword.ResetErrorGeneral");
		public string ResetSuccessText => TextTranslator.Translate("Authentication.ResetPassword.ResetSuccess");
		public string RetryBody => TextTranslator.Translate("Authentication.ResetPassword.RetryBody");
		public string RetryErrorGeneralText => TextTranslator.Translate("Authentication.ResetPassword.RetryErrorGeneral");
		public string RetrySuccessText => TextTranslator.Translate("Authentication.ResetPassword.RetrySuccess");
		public string TokenNotFoundBody => TextTranslator.Translate("Authentication.ResetPassword.TokenNotFound");
		public bool IsValidToken => UserResetPassword.IsValid();
		public bool TokenFound => UserResetPassword != null;
		public string Token => HttpContext.Current.Request["rpToken"]; // TODO: Replace with centralised configuration for parameter (email uses it too)
	}
}