using System;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitledProduct
	{
		string DocumentId { get; }
		string ProductCode { get; }
		bool IsFree { get; }
        bool IsFreeWithRegistration { get; }
        DateTime PublishedOn { get; }
	}
}
