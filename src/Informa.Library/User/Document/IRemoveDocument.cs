namespace Informa.Library.User.Document
{
	public interface IRemoveDocument
	{
		ISavedDocumentWriteResult Remove(string username, string documentId);
	}
}
