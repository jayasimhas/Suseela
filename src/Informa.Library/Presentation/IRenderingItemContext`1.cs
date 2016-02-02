using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public interface IRenderingItemContext<T>
		where T : class, IGlassBase
	{
		T Item { get; }
	}
}
