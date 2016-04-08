namespace Informa.Library.User.Document
{
	public interface ISaveDocument
	{
		ISavedDocumentWriteResult Save(string username, string documentName, string documentDescription, string documentId);
	}
}
