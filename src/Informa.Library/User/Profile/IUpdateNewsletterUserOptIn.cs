using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface IUpdateNewsletterUserOptIn
	{
		bool Update(IEnumerable<INewsletterUserOptIn> newsletterOptIns, string userName);
		bool IsUserSignedUp(string userName);
	}
}
