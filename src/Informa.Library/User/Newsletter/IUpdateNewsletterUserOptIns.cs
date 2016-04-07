using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface IUpdateNewsletterUserOptIns
	{
		bool Update(IEnumerable<INewsletterUserOptIn> newsletterOptIns, string userName);
	}
}
