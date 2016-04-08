namespace Informa.Library.User.Document
{
	public interface ISaveDocumentContext
	{
		ISavedDocumentWriteResult Save(string documentName, string documentDescription, string documentId);
	}
}