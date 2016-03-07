using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailArticlePopOutViewModel : IEmailArticlePopOutViewModel
	{
        protected readonly ITextTranslator TextTranslator;
        protected readonly IRenderingItemContext ArticleRenderingContext;

        public EmailArticlePopOutViewModel(
            ITextTranslator textTranslator,
            IRenderingItemContext articleRenderingContext)
        {
            TextTranslator = textTranslator;
            ArticleRenderingContext = articleRenderingContext;
        }

        public string EmailArticleText => TextTranslator.Translate("Article.EmailPopout.EmailArticle");
        public string EmailFormInstructionsText => TextTranslator.Translate("Article.EmailPopout.EmailFormInstructions");
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
        public string ArticleTitle => ArticleRenderingContext.Get<IArticle>().Title;	
		public string ArticleNumber => ArticleRenderingContext.Get<IArticle>().Article_Number;
	}
}