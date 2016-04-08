namespace Informa.Library.User.Profile
{
	public interface ISaveDocumentContext
	{
		ISavedDocumentWriteResult Save(string documentName, string documentDescription, string documentId);
	}
}