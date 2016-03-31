﻿namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementAccessLevelContext
	{
		EntitledAccessLevel Determine(IEntitlement entitlement);
		string GetEntitledProducts();
		string GetEntitledProductStatus();
	}
}