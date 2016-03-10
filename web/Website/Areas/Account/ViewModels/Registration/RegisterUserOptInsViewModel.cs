using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
	public class RegisterUserOptInsViewModel : GlassViewModel<IRegistration_Page>
	{
		protected readonly ITextTranslator TextTranslator;

		public RegisterUserOptInsViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string Title => GlassModel?.Title;
		public string SubTitle => GlassModel?.Sub_Title;
		public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
		public string SubmitText => TextTranslator.Translate("Registration.OptIn.Submit");
		public string GeneralErrorText => TextTranslator.Translate("Registration.OptIn.GeneralError");
		public string OffersLabelText => TextTranslator.Translate("Registration.OptIn.OffersLabel");
		public string NewslettersLabelText => TextTranslator.Translate("Registration.OptIn.NewslettersLabel");
	}
}
