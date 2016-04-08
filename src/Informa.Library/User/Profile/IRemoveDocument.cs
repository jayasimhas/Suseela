namespace Informa.Library.User.Profile
{
	public interface IRemoveDocument
	{
		ISavedDocumentWriteResult Remove(string username, string documentId);
	}
}
