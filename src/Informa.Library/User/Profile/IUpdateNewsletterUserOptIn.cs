using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface IUpdateNewsletterUserOptIn
	{
		bool Update(IUser user, IEnumerable<INewsletterUserOptIn> newsletterOptIns);
	}
}
