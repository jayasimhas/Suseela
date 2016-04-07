using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface IUpdateNewsletterUserOptIns
	{
		bool Update(IEnumerable<INewsletterUserOptIn> newsletterOptIns, string userName);
	}
}
