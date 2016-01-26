using System.Collections.Generic;

namespace Informa.Library.Presentation
{
	public interface IDuplicateRenderingsFactory
	{
		IEnumerable<IRendering> Create();
	}
}
