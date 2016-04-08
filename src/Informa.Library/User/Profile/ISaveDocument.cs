namespace Informa.Library.User.Profile
{
	public interface ISaveDocument
	{
		ISavedDocumentWriteResult Save(string username, string documentName, string documentDescription, string documentId);
	}
}
