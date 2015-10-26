using System;
using System.Collections.Generic;
using Jabberwocky.Glass.Factory.Attributes;

namespace Informa.Library.InterfaceFactory.Interfaces
{
	[GlassFactoryInterface]
	public interface IPublishingLogic
	{
		IEnumerable<Guid> GetItemPublishingCandidates();
	}
}
