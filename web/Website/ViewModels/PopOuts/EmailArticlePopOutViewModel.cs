using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.PerScope)]
	public class EmailArticlePopOutViewModel : IEmailArticlePopOutViewModel
	{
		protected readonly ITextTranslator TextTranslator;
		protected readonly IRenderingItemContext ArticleRenderingContext;
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IArticle Article;

		public EmailArticlePopOutViewModel(
				ITextTranslator textTranslator,
				IRenderingItemContext articleRenderingContext,
				IAuthenticatedUserContext userContext)
		{
			TextTranslator = textTranslator;
			ArticleRenderingContext = articleRenderingContext;
			UserContext = userContext;

			Article = ArticleRenderingContext.Get<IArticle>();
		}

		public string AuthUserEmail => UserContext.User?.Email ?? string.Empty;
		public string AuthUserName => UserContext.User?.Name ?? string.Empty;

		public string EmailArticleText => TextTranslator.Translate("Article.EmailPopout.EmailArticle");
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
		public string ArticleTitle => Article.Title;
		public string ArticleNumber => Article.Article_Number;
	}
}