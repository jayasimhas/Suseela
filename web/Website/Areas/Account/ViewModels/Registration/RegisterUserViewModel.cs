using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.User.Registration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;
using Informa.Library.Services.Global;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
	public class RegisterUserViewModel : GlassViewModel<IRegistration_Details_Page>
	{
		protected readonly IGlobalSitecoreService GlobalService;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IReturnUrlContext ReturnUrlContext;
		protected readonly IRegisterReturnUrlContext RegisterReturnUrlContex;

		public RegisterUserViewModel(
            IGlobalSitecoreService globalService,
			ITextTranslator textTranslator,
			IReturnUrlContext returnUrlContext,
			IRegisterReturnUrlContext registerReturnUrlContext)
		{
            GlobalService = globalService;
			TextTranslator = textTranslator;
			ReturnUrlContext = returnUrlContext;
			RegisterReturnUrlContex = registerReturnUrlContext;
		}

		public string Title => GlassModel?.Title;
		public string SubTitle => GlassModel?.Sub_Title;
		public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
		public string NextStepUrl
		{
			get
			{
				if (GlassModel == null)
				{
					return string.Empty;
				}

				var nextStepItem = GlobalService.GetItem<I___BasePage>(GlassModel.Next_Step_Page);

				if (nextStepItem == null)
				{
					return string.Empty;
				}

				return nextStepItem == null ? string.Empty : nextStepItem._Url;
			}
		}
        public string RequiedFieldsText => TextTranslator.Translate("Registration.RequiredFields");
		public string UsernameValue => HttpContext.Current?.Request?["username"] ?? string.Empty;
		public string UsernameLabelText => TextTranslator.Translate("Registration.UsernameLabel");
		public string UsernamePlaceholderText => TextTranslator.Translate("Registration.UsernamePlaceholder");
		public string FirstNameLabelText => TextTranslator.Translate("Registration.FirstNameLabel");
		public string FirstNamePlaceholderText => TextTranslator.Translate("Registration.FirstNamePlaceholder");
		public string LastNameLabelText => TextTranslator.Translate("Registration.LastNamePlaceholder");
		public string PasswordLabelText => TextTranslator.Translate("Registration.PasswordLabel");
		public string PasswordPlaceholderText => TextTranslator.Translate("Registration.PasswordPlaceholder");
		public string PasswordRepeatLabelText => TextTranslator.Translate("Registration.PasswordRepeatLabel");
		public string MasterToggleLabelText => TextTranslator.Translate("Registration.MasterToggleLabel");
		public string MasterIdLabelText => TextTranslator.Translate("Registration.MasterIdLabel");
		public string MasterPasswordLabelText => TextTranslator.Translate("Registration.MasterPasswordLabel");
		public IHtmlString MasterAlternateVerficationText => new MvcHtmlString(GlassModel?.Corporate_Alternative_Verification_Method);
		public IHtmlString TermsLabel => new MvcHtmlString(GlassModel?.User_Agreement_Text);
		public string SubmitText => TextTranslator.Translate("Registration.Submit");
		public string NewsletterSignUpText => GlassModel?.Newsletter_Sign_Up_Text;
		public string RequiredErrorText => TextTranslator.Translate("Registration.RequiredError");
		public string UsernameRequirementsErrorText => TextTranslator.Translate("Registration.UsernameRequirementsError");
		public string UsernamePublicRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedPublicDomainError");
		public string UsernameCompetitorRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedCompetitorDomainError");
		public string UsernameExistsErrorText => TextTranslator.Translate("Registration.UsernameExistsError");
		public string PasswordMismatchErrorText => TextTranslator.Translate("Registration.PasswordMismatchError");
		public string PasswordRequirementsErrorText => TextTranslator.Translate("Registration.PasswordRequirementsError");
		public string TermsNotAcceptedErrorText => TextTranslator.Translate("Registration.TermsNotAcceptedError");
		public string GeneralErrorText => TextTranslator.Translate("Registration.GeneralError");
		public string MasterIdInvalidErrorText => TextTranslator.Translate("Registration.MasterIdInvalidError");
		public string MasterIdExpiredErrorText => TextTranslator.Translate("Registration.MasterIdExpiredError");
		public string RegisterReturnUrl => RegisterReturnUrlContex.Url;
		public string RegisterReturnUrlKey => ReturnUrlContext.Key;

        //Master ID/Password settings
        public string MasterId => Sitecore.Configuration.Settings.GetSetting("MasterId");
        public string MasterPassword => Sitecore.Configuration.Settings.GetSetting("MasterPassword");
    }
}
