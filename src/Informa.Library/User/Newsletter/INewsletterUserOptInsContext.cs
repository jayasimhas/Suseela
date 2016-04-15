using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface INewsletterUserOptInsContext
	{
		IEnumerable<INewsletterUserOptIn> OptIns { get; }
		void Clear();
	}
}
