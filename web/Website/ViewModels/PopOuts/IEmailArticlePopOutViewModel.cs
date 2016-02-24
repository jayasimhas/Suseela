﻿namespace Informa.Web.ViewModels.PopOuts
{
	public interface IEmailArticlePopOutViewModel
	{
        string EmailArticleText { get; }
        string EmailFormInstructionsText { get; } 
        string RecipientEmailPlaceholderText { get; }
        string YourNamePlaceholderText { get; }
        string YourEmailPlaceholderText { get; }
        string SubjectText { get; }
        string AddMessageText { get; }
        string CancelText { get; }
        string SendText { get; } 
        string InvalidEmailText { get; }
        string EmptyFieldText { get; }
        string NoticeText { get; }
        string ArticleTitle { get; }
    }
}
