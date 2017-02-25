namespace Informa.Library.User.Document
{
    public class SavedDocumentWriteResult : ISavedDocumentWriteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public string SalesforceId { get; set; }
    }
}
