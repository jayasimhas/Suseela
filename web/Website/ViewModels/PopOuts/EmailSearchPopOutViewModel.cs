﻿using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.PopOuts
{
	[AutowireService]
	public class EmailSearchPopOutViewModel : IEmailSearchPopOutViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly IRenderingItemContext ArticleRenderingContext;
	    protected readonly IAuthenticatedUserContext UserContext;

        public EmailSearchPopOutViewModel(
            ITextTranslator textTranslator,
            IRenderingItemContext articleRenderingContext,
            IAuthenticatedUserContext userContext)
        {
            TextTranslator = textTranslator;
            ArticleRenderingContext = articleRenderingContext;
            UserContext = userContext;
        }

	    public string AuthUserEmail => UserContext.User.Email;
	    public string AuthUserName => UserContext.User.Name;

        public string EmailSearchText => TextTranslator.Translate("Search.EmailPopout.EmailSearchResults");
	    public string EmailSentSuccessMessage => TextTranslator.Translate("Search.EmailPopout.EmailSentSuccessMessage");
        public string EmailFormInstructionsText => TextTranslator.Translate("Search.EmailPopout.EmailFormInstructions");
	    public string GeneralError => TextTranslator.Translate("Search.EmailPopout.GeneralError");
	    public string RecipientEmailPlaceholderText => TextTranslator.Translate("Search.EmailPopout.RecipientEmailPlaceholder");
	    public string YourNamePlaceholderText => TextTranslator.Translate("Search.EmailPopout.YourNamePlaceholder");
	    public string YourEmailPlaceholderText => TextTranslator.Translate("Search.EmailPopout.YourEmailPlaceholder");
	    public string SubjectPlaceholderText => TextTranslator.Translate("Search.EmailPopout.SubjectPlaceholderText");
        public string CancelText => TextTranslator.Translate("Search.EmailPopout.Cancel");
	    public string SendText => TextTranslator.Translate("Search.EmailPopout.Send");
	    public string InvalidEmailText => TextTranslator.Translate("Search.EmailPopout.InvalidEmail"); 
	    public string EmptyFieldText => TextTranslator.Translate("Search.EmailPopout.EmptyField");
	    public string NoticeText => TextTranslator.Translate("Search.EmailPopout.Notice");
        public string ToLabel => TextTranslator.Translate("Search.EmailPopout.ToLabel");
        public string NameLabel => TextTranslator.Translate("Search.EmailPopout.NameLabel");
        public string EmailLabel => TextTranslator.Translate("Search.EmailPopout.EmailLabel");
        public string SubjectLabel => TextTranslator.Translate("Search.EmailPopout.SubjectLabel");
        public string AddMessageLabel => TextTranslator.Translate("Search.EmailPopout.AddMessageLabel");
    }
}