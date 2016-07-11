namespace Informa.Web.ViewModels.PopOuts
{
	public interface IEmailAuthorPopOutViewModel
	{
        string AuthUserEmail { get; }
        string AuthUserName { get; }
        string EmailAuthorText { get; }
        string EmailSentSuccessMessage { get; }
        string GeneralError { get; }
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
        string AuthorName { get; }
	}
}
