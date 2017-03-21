namespace Informa.Web.ViewModels.PopOuts
{
    public interface IAsktheAnalystPopOutViewModel
    {
        string AskTheAnalystEmailID { get; }
        string SenderPlaceHolderName { get; }
        string SenderPlaceHolderCompanyName { get; }
        string SenderPlaceHolderPhoneNumber { get; }
        string ArticleNumber { get; }
        string ArticleTitle { get; }
        string AskTheAnalystText { get; }
        string ATASentSuccessMessage { get; }
        string SenderPlaceHolderEmail { get; }
        string ATAFormInstructionsText { get; }
        string PublicationName { get; }
        string GeneralError { get; }
        string Invalidemailaddress { get; }
        string Invalidphonenumber { get; }
        string SubjectText { get; }
        string Askyourquestion { get; }
        string CancelText { get; }
        string SendText { get; }
        string EmptyFieldText { get; }
        string AskTheAnalystLink { get; }        
    }
}