namespace Informa.Library.User.Content
{
	public interface IContentResponse
	{
		bool Success { get; set; }
		string Message { get; set; }
	}
}