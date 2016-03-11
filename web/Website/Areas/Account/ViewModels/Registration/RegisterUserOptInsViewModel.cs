using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
	public class RegisterUserOptInsViewModel : GlassViewModel<IRegistration_Thank_You_Page>
	{
		protected readonly ICompanyContext CompanyContext;
		protected readonly ITextTranslator TextTranslator;

		public RegisterUserOptInsViewModel(
			ICompanyContext companyContext,
			ITextTranslator textTranslator)
		{
			CompanyContext = companyContext;
			TextTranslator = textTranslator;
		}

		public string Title => GlassModel?.Title;
		public string SubTitle => CompanyContext.Company == null ? GlassModel?.Sub_Title : GlassModel?.Company_Sub_Title.ReplacePatternCaseInsensitive("#User_Company_Name#", CompanyContext.Company.Name);
		public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
		public string SubmitText => TextTranslator.Translate("Registration.OptIn.Submit");
		public string GeneralErrorText => TextTranslator.Translate("Registration.OptIn.GeneralError");
		public string OffersLabelText => TextTranslator.Translate("Registration.OptIn.OffersLabel");
		public string NewslettersLabelText => TextTranslator.Translate("Registration.OptIn.NewslettersLabel");
	}
}
