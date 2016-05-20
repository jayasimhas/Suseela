using Informa.Library.User.Entitlement;

namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IUserPageEntitlementViewModel
	{
		bool IsValidPage { get; }
		bool IsEntitled { get; }
		string SiteProductCode { get; }
		IEntitledProduct EntitledProduct { get; }
		IEntitlementAccess EntitlementAccess { get; }
	}
}
