using System;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitledProduct
	{
		string DocumentId { get; }
		string ProductCode { get; }
		bool IsFree { get; }
		DateTime PublishedOn { get; }
	}
}
