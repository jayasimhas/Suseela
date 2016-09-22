using Informa.Library.Globalization;
using Informa.Library.Services.Captcha;
using Informa.Library.User.Authentication;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
	public class CompaniesandDealsShareViewModel : GlassViewModel<I___BasePage>
	{

		protected readonly ITextTranslator TextTranslator;
		protected readonly IDCDReader DCDReader;
		protected readonly IHttpContextProvider HttpContextProvider;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IRecaptchaService RecaptchaSettings;

		public CompaniesandDealsShareViewModel(
								ITextTranslator textTranslator,
								IDCDReader dcdReader,
								IHttpContextProvider context,
								IAuthenticatedUserContext userContext,
								I___BasePage glassModel,
								IRecaptchaService recaptchaSettings)
		{
			TextTranslator = textTranslator;
			DCDReader = dcdReader;
			HttpContextProvider = context;
			UserContext = userContext;
			RecaptchaSettings = recaptchaSettings;

			var wildcardValue = UrlUtils.GetLastUrlSement(HttpContextProvider.Current);
			if (glassModel._TemplateId.Equals(ICompany_PageConstants.TemplateId.Guid))
			{
				var Company = DCDReader.GetCompanyByRecordNumber(wildcardValue);
				PageTitle = Company.Title;
                CompanyId = Company.RecordNumber.ToString();
			}
			else if (glassModel._TemplateId.Equals(IDeal_PageConstants.TemplateId.Guid))
			{
				var Deal = DCDReader.GetDealByRecordNumber(wildcardValue);
				PageTitle = Deal.Title;
                DealId = Deal.RecordNumber.ToString();
			}
		}

		public string AuthUserEmail => UserContext.User?.Email ?? string.Empty;
		public string AuthUserName => UserContext.User?.Name ?? string.Empty;

		public string PageTitle = string.Empty;
        public string CompanyId = string.Empty;
        public string DealId = string.Empty;
		public string PageUrl => $"{HttpContextProvider.Current.Request.Url.Scheme}://{HttpContextProvider.Current.Request.Url.Host}{HttpContextProvider.Current.Request.Url.PathAndQuery}";
		public string ShareText => TextTranslator.Translate("DCD.Share");
		public string EmailCompanyText => TextTranslator.Translate("DCD.EmailPopout.EmailCompany");
		public string EmailDealText => TextTranslator.Translate("DCD.EmailPopout.EmailDeal");
		public string EmailSentSuccessMessage => TextTranslator.Translate("DCD.EmailPopout.EmailSentSuccessMessage");
		public string EmailFormInstructionsText => TextTranslator.Translate("DCD.EmailPopout.EmailFormInstructions");
		public string GeneralError => TextTranslator.Translate("DCD.EmailPopout.GeneralError");
		public string RecipientEmailPlaceholderText => TextTranslator.Translate("DCD.EmailPopout.RecipientEmailPlaceholder");
		public string YourNamePlaceholderText => TextTranslator.Translate("DCD.EmailPopout.YourNamePlaceholder");
		public string YourEmailPlaceholderText => TextTranslator.Translate("DCD.EmailPopout.YourEmailPlaceholder");
		public string SubjectText => TextTranslator.Translate("DCD.EmailPopout.SubjectText");
		public string AddMessageText => TextTranslator.Translate("DCD.EmailPopout.AddMessage");
		public string CancelText => TextTranslator.Translate("DCD.EmailPopout.Cancel");
		public string SendText => TextTranslator.Translate("DCD.EmailPopout.Send");
		public string InvalidEmailText => TextTranslator.Translate("DCD.EmailPopout.InvalidEmail");
		public string EmptyFieldText => TextTranslator.Translate("DCD.EmailPopout.EmptyField");
		public string NoticeText => TextTranslator.Translate("DCD.EmailPopout.Notice");
		public string CaptchaSiteKey => RecaptchaSettings.SiteKey;
	}
}