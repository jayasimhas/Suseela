using System.Collections.Generic;

namespace Informa.Library.User.Registration
{
	public interface IRestrictedEmailDomains
	{
		IEnumerable<string> RestrictedDomains { get; }
	}
}