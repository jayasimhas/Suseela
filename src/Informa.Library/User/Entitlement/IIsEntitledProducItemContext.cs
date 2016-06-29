using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;

namespace Informa.Library.User.Entitlement
{
	public interface IIsEntitledProducItemContext
	{
		bool IsEntitled(IEntitled_Product item);
	}
}