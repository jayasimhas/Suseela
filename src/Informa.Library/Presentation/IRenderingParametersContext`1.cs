using Jabberwocky.Glass.Models;

namespace Informa.Library.Presentation
{
	public interface IRenderingParametersContext<T>
		where T : class, IGlassBase
	{
		T Parameters { get; }
	}
}
