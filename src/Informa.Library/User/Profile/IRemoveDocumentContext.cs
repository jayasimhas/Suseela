namespace Informa.Library.User.Profile
{
	public interface IRemoveDocumentContext
	{
		ISavedDocumentWriteResult Remove(string documentId);
	}
}