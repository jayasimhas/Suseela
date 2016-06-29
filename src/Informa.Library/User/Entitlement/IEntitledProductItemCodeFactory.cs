using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitledProductItemCodeFactory
	{
		string Create(IEntitled_Product item);
	}
}