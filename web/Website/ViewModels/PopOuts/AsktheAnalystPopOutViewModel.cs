using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Services.Captcha;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.Site;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class AsktheAnalystPopOutViewModel : IAsktheAnalystPopOutViewModel
    {
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IArticle Article;
		protected readonly IRecaptchaService RecaptchaSettings;
        protected readonly ISiteRootContext SiteRootContext;

        public AsktheAnalystPopOutViewModel(
            ISiteRootContext siteRootContext,
                ITextTranslator textTranslator,
				IRenderingItemContext articleRenderingContext,
				IAuthenticatedUserContext userContext,
				IRecaptchaService recaptchaSettings)
		{
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
			UserContext = userContext;
			RecaptchaSettings = recaptchaSettings;
			Article = ArticleRenderingContext.Get<IArticle>();
		}

        public string AskTheAnalystText => TextTranslator.Translate("Article.ATAPopout.Text");

        public string ATAFormInstructionsText => TextTranslator.Translate("Article.ATAPopout.ATAFormInstructions");

        public string ATASentSuccessMessage => TextTranslator.Translate("Article.ATAPopout.ATASentSuccessMessage");

        public string GeneralError => TextTranslator.Translate("Article.ATAPopout.GeneralError");

        public string SenderPlaceHolderName => TextTranslator.Translate("Article.ATAPopout.YourNamePlaceholder");

        public string SenderPlaceHolderCompanyName => TextTranslator.Translate("Article.ATAPopout.YourCompanyNamePlaceholder");

        public string SenderPlaceHolderPhoneNumber => TextTranslator.Translate("Article.ATAPopout.YourPhoneNumberPlaceholder");

        public string SenderPlaceHolderEmail => TextTranslator.Translate("Article.ATAPopout.YourATAEmailPlaceholder");
                    
        public string Invalidemailaddress => TextTranslator.Translate("Article.ATAPopout.InvalidEmail");

        public string EmptyFieldText => TextTranslator.Translate("Article.ATAPopout.EmptyField");

        public string Invalidphonenumber => TextTranslator.Translate("Article.ATAPopout.InvalidPhone");

        public string ArticleNumber => Article.Article_Number;
       
        public string SubjectText => TextTranslator.Translate("Article.ATAPopout.SubjectText");

        public string ArticleTitle => Article.Title;

        public string Askyourquestion => TextTranslator.Translate("Article.ATAPopout.AddMessage");

        public string CancelText => TextTranslator.Translate("Article.ATAPopout.Cancel");

        public string SendText => TextTranslator.Translate("Article.ATAPopout.Send");

        public string PublicationName => TextTranslator.Translate("Article.PubName");

        public string CaptchaSiteKey => RecaptchaSettings.SiteKey;

        public string AskTheAnalystLink => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Ask_The_Analyst;
        public string AskTheAnalystEmailID => SiteRootContext.Item == null ? string.Empty : SiteRootContext.Item.Analyst_Email_ID;

    }
}