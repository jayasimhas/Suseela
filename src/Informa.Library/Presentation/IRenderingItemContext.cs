using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public interface IRenderingItemContext
	{
		T Get<T>() where T : class, IGlassBase;
	}
}
