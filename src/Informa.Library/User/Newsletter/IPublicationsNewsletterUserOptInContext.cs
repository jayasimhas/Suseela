using System.Collections.Generic;

namespace Informa.Library.User.Newsletter
{
	public interface IPublicationsNewsletterUserOptInContext
	{
		IEnumerable<IPublicationNewsletterUserOptIn> OptIns { get; }
	}
}