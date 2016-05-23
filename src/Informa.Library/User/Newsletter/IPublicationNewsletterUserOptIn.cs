namespace Informa.Library.User.Newsletter
{
	public interface IPublicationNewsletterUserOptIn
	{
		bool OptIn { get; set; }
		string Publication { get; }
	}
}