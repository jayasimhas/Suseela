using Jabberwocky.Glass.Models;

namespace Informa.Library.Page
{
	public interface IPageItemContext
	{
		T Get<T>() where T : class, IGlassBase;
	}
}