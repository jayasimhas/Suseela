using System.Collections.Generic;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.User.Registration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Informa.Library.Services.Global;
using Informa.Library.User.Profile;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
	public class RegisterFreeTrialUserViewModel : GlassViewModel<IRegistration_Free_Trial_Page>
	{
		protected readonly IGlobalSitecoreService GlobalService;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IReturnUrlContext ReturnUrlContext;
		protected readonly IRegisterReturnUrlContext RegisterReturnUrlContex;
        public readonly IUserProfile Profile;

        public RegisterFreeTrialUserViewModel(
            IGlobalSitecoreService globalService,
			ITextTranslator textTranslator,
			IReturnUrlContext returnUrlContext,
            IUserProfileContext profileContext,
            IRegisterReturnUrlContext registerReturnUrlContext)
		{
            GlobalService = globalService;
			TextTranslator = textTranslator;
			ReturnUrlContext = returnUrlContext;
            Profile = profileContext.Profile;
            RegisterReturnUrlContex = registerReturnUrlContext;

            Countries = globalService.GetCountries();
        }
        
        public string Title => GlassModel?.Title;
		public string SubTitle => GlassModel?.Sub_Title;
		public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
		public string NextStepUrl
		{
			get
			{
				if (GlassModel == null)
					return string.Empty;
				
				var nextStepItem = GlobalService.GetItem<I___BasePage>(GlassModel.Next_Step_Page);
                return (nextStepItem == null)
					? string.Empty
                    : nextStepItem._Url;
			}
		}

        #region Profile Fields

        public string ProfileFirstName => Profile?.FirstName ?? string.Empty;
        public string ProfileLastName => Profile?.LastName ?? string.Empty;
        public string ProfileCompany => Profile?.Company ?? string.Empty;
        public string ProfileJobTitle => Profile?.JobTitle ?? string.Empty;
        public string ProfileAddress1 => Profile?.ShipAddress1 ?? string.Empty;
        public string ProfileAddress2 => Profile?.ShipAddress2 ?? string.Empty;
        public string ProfileCity => Profile?.ShipCity ?? string.Empty;
        public string ProfileState => Profile?.ShipState ?? string.Empty;
        public string ProfilePostalCode => Profile?.ShipPostalCode ?? string.Empty;
        public string ProfileCountry => Profile?.ShipCountry ?? string.Empty;
        public string ProfilePhone => Profile?.Phone ?? string.Empty;

        #endregion Profile Fields

        #region Labels Properties and Errors

        public string GeneralErrorText => TextTranslator.Translate("ContactInfo.GeneralError");
        public string RequiedFieldsText => TextTranslator.Translate("Registration.RequiredFields");
        public string RegisterReturnUrlKey => ReturnUrlContext.Key;
        public string RegisterReturnUrl => RegisterReturnUrlContex.Url;
        public string UsernameLabelText => TextTranslator.Translate("Registration.UsernameLabel");
        public string UsernameRequirementsErrorText => TextTranslator.Translate("Registration.UsernameRequirementsError");
        public string UsernameCompetitorRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedCompetitorDomainError");
        public string UsernamePublicRestrictedDomainErrorText => TextTranslator.Translate("Registration.UsernameRestrictedPublicDomainError");
        public string UsernameExistsErrorText => TextTranslator.Translate("Registration.UsernameExistsError");
        public string RequiredErrorText => TextTranslator.Translate("Registration.RequiredError");
        public string UsernamePlaceholderText => TextTranslator.Translate("Registration.UsernamePlaceholder");
        public string UsernameValue => HttpContext.Current?.Request?["username"] ?? string.Empty;
        public string PasswordLabelText => TextTranslator.Translate("Registration.PasswordLabel");
        public string PasswordRequirementsErrorText => TextTranslator.Translate("Registration.PasswordRequirementsError");
        public string PasswordMismatchErrorText => TextTranslator.Translate("Registration.PasswordMismatchError");
        public string PasswordPlaceholderText => TextTranslator.Translate("Registration.PasswordPlaceholder");
        public string PasswordRepeatLabelText => TextTranslator.Translate("Registration.PasswordRepeatLabel");
        public string TermsNotAcceptedErrorText => TextTranslator.Translate("Registration.TermsNotAcceptedError");
        public IHtmlString TermsLabel => new MvcHtmlString(GlassModel?.User_Agreement_Text);
        public string SubmitText => TextTranslator.Translate("Registration.Submit");
        public string NewsletterSignUpText => GlassModel?.Newsletter_Sign_Up_Text;

        #endregion Labels Properties and Errors

        #region Contact Info

        public string FirstNameLabelText => TextTranslator.Translate("ContactInfo.FirstNameLabel");
        public string FirstNamePlaceholderText => TextTranslator.Translate("ContactInfo.FirstNamePlaceholder");
        public string LastNameLabelText => TextTranslator.Translate("ContactInfo.LastNameLabel");
        public string CompanyLabelText => TextTranslator.Translate("ContactInfo.CompanyLabel");
        public string CompanyPlaceholderText => TextTranslator.Translate("ContactInfo.CompanyPlaceholder");
        public string JobTitleLabelText => TextTranslator.Translate("ContactInfo.JobTitleLabel");
        public string JobTitlePlaceholderText => TextTranslator.Translate("ContactInfo.JobTitlePlaceholder");
        public string PhoneLabelText => TextTranslator.Translate("ContactInfo.PhoneLabel");
        public string PhonePlaceholderText => TextTranslator.Translate("ContactInfo.PhonePlaceholder");
        public string CountryLabelText => TextTranslator.Translate("ContactInfo.CountryLabel");
        public string Address1LabelText => TextTranslator.Translate("ContactInfo.Address1Label");
        public string Address1PlaceholderText => TextTranslator.Translate("ContactInfo.Address1Placeholder");
        public string Address2LabelText => TextTranslator.Translate("ContactInfo.Address2Label");
        public string Address2PlaceholderText => TextTranslator.Translate("ContactInfo.Address2Placeholder");
        public string CityLabelText => TextTranslator.Translate("ContactInfo.CityLabel");
        public string CityPlaceholderText => TextTranslator.Translate("ContactInfo.CityPlaceholder");
        public string PostalCodeLabelText => TextTranslator.Translate("ContactInfo.PostalCodeLabel");
        public string PostalCodePlaceholderText => TextTranslator.Translate("ContactInfo.PostalCodePlaceholder");
        public string StateLabelText => TextTranslator.Translate("ContactInfo.StateLabel");
        public string StatePlaceholderText => TextTranslator.Translate("ContactInfo.StatePlaceholder");
        
        #endregion Contact Info

        #region Drop Down Lists 

        public IEnumerable<ListItem> Countries { get; set; }

        #endregion Drop Down Lists

        public string CurVerticalName => GlobalService.GetVerticalRootAncestor(Sitecore.Context.Item.ID.ToGuid())?._Name;
    }
}
