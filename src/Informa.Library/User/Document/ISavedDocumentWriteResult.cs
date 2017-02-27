namespace Informa.Library.User.Document
{
    public interface ISavedDocumentWriteResult
    {
        bool Success { get; set; }
        string Message { get; set; }
        string SalesforceId { get; set; }
    }
}
