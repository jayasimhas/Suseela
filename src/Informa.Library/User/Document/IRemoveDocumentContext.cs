namespace Informa.Library.User.Document
{
	public interface IRemoveDocumentContext
	{
		ISavedDocumentWriteResult Remove(string documentId, string salesforceId);
	}
}