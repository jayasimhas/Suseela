namespace Informa.Library.User.Profile
{
	public interface IUpdateSiteNewsletterUserOptIn
	{
		bool Update(string username, bool optIn);
	}
}