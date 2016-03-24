using System.Collections.Generic;
using Informa.Library.User.Entitlement;

namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUserEntitlementsViewModel
	{
		IEnumerable<IEntitlement> Entitlements { get; }
	}
}