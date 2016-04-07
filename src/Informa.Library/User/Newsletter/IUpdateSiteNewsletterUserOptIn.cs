namespace Informa.Library.User.Newsletter
{
	public interface IUpdateSiteNewsletterUserOptIn
	{
		bool Update(string username, bool optIn);
	}
}