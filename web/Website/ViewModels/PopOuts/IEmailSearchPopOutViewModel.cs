namespace Informa.Web.ViewModels.PopOuts
{
	public interface IEmailSearchPopOutViewModel
	{
        string AuthUserEmail { get; }
        string AuthUserName { get; }
        string EmailSearchText { get; }
        string EmailSentSuccessMessage { get; }
        string EmailFormInstructionsText { get; } 
        string GeneralError { get; }
        string RecipientEmailPlaceholderText { get; }
        string YourNamePlaceholderText { get; }
        string YourEmailPlaceholderText { get; }
        string SubjectPlaceholderText { get; }
        string CancelText { get; }
        string SendText { get; } 
        string InvalidEmailText { get; }
        string EmptyFieldText { get; }
	    string NoticeText { get; }
        string ToLabel { get; }
        string NameLabel { get; }
        string EmailLabel { get; }
        string SubjectLabel { get; }
        string AddMessageLabel { get; }
    }
}
