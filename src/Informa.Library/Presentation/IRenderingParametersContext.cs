using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public interface IRenderingParametersContext
	{
		T GetParameters<T>() where T : class, IGlassBase;
	}
}
