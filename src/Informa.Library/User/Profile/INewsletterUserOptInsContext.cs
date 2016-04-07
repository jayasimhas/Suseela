using System.Collections.Generic;

namespace Informa.Library.User.Profile
{
	public interface INewsletterUserOptInsContext
	{
		IEnumerable<INewsletterUserOptIn> OptIns { get; }
		void Clear();
	}
}
