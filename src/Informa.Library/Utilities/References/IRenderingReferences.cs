using System;

namespace Informa.Library.Utilities.References
{
	public interface IRenderingReferences
	{
		Guid ListableContentSmall { get; }
		Guid LoginPopout { get; }
		Guid RegisterPopout { get; }
		Guid TopicAlertButton { get; }
		Guid LogoutMessage { get; }
		Guid RenderingMVC { get; }
	}
}
