using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
	public class RegisterUserViewModel : GlassViewModel<IRegistration_Page>
	{
		protected readonly ITextTranslator TextTranslator;

		public RegisterUserViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string Title => GlassModel?.Title;
		public string SubTitle => GlassModel?.Sub_Title;
		public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
        public string RequiedFieldsText => TextTranslator.Translate("Registration.RequiredFields");
		public string UsernameLabelText => TextTranslator.Translate("Registration.UsernameLabel");
		public string UsernamePlaceholderText => TextTranslator.Translate("Registration.UsernamePlaceholder");
		public string FirstNameLabelText => TextTranslator.Translate("Registration.FirstNameLabel");
		public string FirstNamePlaceholderText => TextTranslator.Translate("Registration.FirstNamePlaceholder");
		public string LastNameLabelText => TextTranslator.Translate("Registration.LastNamePlaceholder");
		public string PasswordLabelText => TextTranslator.Translate("Registration.PasswordLabel");
		public string PasswordPlaceholderText => TextTranslator.Translate("Registration.PasswordPlaceholder");
		public string PasswordRepeatLabelText => TextTranslator.Translate("Registration.PasswordRepeatLabel");
		public IHtmlString TermsLabel => new MvcHtmlString(GlassModel?.User_Agreement_Text);
		public string SubmitText => TextTranslator.Translate("Registration.Submit");
		public string NewsletterSignUpText => GlassModel?.Newsletter_Sign_Up_Text;
		public string RequiredErrorText => TextTranslator.Translate("Registration.RequiredError");
		public string UsernameRequirementsErrorText => TextTranslator.Translate("Registration.UsernameRequirementsError");
		public string PasswordMismatchErrorText => TextTranslator.Translate("Registration.PasswordMismatchError");
		public string PasswordRequirementsErrorText => TextTranslator.Translate("Registration.PasswordRequirementsError");
		public string TermsNotAcceptedErrorText => TextTranslator.Translate("Registration.TermsNotAcceptedError");
		public string GeneralErrorText => TextTranslator.Translate("Registration.GeneralError");
	}
}
