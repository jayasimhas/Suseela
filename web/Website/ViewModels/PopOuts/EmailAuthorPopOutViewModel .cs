using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Library.Services.Captcha;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Attributes;
using Informa.Library.Authors;
using Informa.Library.Wrappers;
using System.Linq;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class EmailAuthorPopOutViewModel : IEmailAuthorPopOutViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IStaff_Item Author;
		protected readonly IRecaptchaService RecaptchaSettings;
        protected readonly IAuthorIndexClient AuthorIndexClient;
        protected readonly IHttpContextProvider HttpContext;

        public EmailAuthorPopOutViewModel(
				ITextTranslator textTranslator,
				IRenderingItemContext articleRenderingContext,
				IAuthenticatedUserContext userContext,
				IRecaptchaService recaptchaSettings,
                IAuthorIndexClient authorIndexClient,
                IHttpContextProvider httpContext)
		{
			TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
			UserContext = userContext;
			RecaptchaSettings = recaptchaSettings;
            AuthorIndexClient = authorIndexClient;
            HttpContext = httpContext;

            Author = AuthorIndexClient.GetAuthorByUrlName(HttpContext.Current.Request.Url.Segments.Last());
		}

		public string AuthUserEmail => UserContext.User?.Email ?? string.Empty;
		public string AuthUserName => UserContext.User?.Name ?? string.Empty;

		public string EmailAuthorText => TextTranslator.Translate("Author.EmailAuthor");
		public string EmailSentSuccessMessage => TextTranslator.Translate("Article.EmailPopout.EmailSentSuccessMessage");
		public string EmailFormInstructionsText => TextTranslator.Translate("Article.EmailPopout.EmailFormInstructions");
		public string GeneralError => TextTranslator.Translate("Article.EmailPopout.GeneralError");
		public string RecipientEmailPlaceholderText => TextTranslator.Translate("Article.EmailPopout.RecipientEmailPlaceholder");
		public string YourNamePlaceholderText => TextTranslator.Translate("Article.EmailPopout.YourNamePlaceholder");
		public string YourEmailPlaceholderText => TextTranslator.Translate("Article.EmailPopout.YourEmailPlaceholder");
		public string SubjectText => TextTranslator.Translate("Article.EmailPopout.SubjectText");
		public string AddMessageText => TextTranslator.Translate("Article.EmailPopout.AddMessage");
		public string CancelText => TextTranslator.Translate("Article.EmailPopout.Cancel");
		public string SendText => TextTranslator.Translate("Article.EmailPopout.Send");
		public string InvalidEmailText => TextTranslator.Translate("Article.EmailPopout.InvalidEmail");
		public string EmptyFieldText => TextTranslator.Translate("Article.EmailPopout.EmptyField");
		public string NoticeText => TextTranslator.Translate("Article.EmailPopout.Notice");
		public string AuthorName => Author.First_Name + " " + Author.Last_Name;
		public string CaptchaSiteKey => RecaptchaSettings.SiteKey;


	}
}