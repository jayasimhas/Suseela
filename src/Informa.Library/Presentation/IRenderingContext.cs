using System;

namespace Informa.Library.Presentation
{
	public interface IRenderingContext : Glass.Mapper.Sc.Web.IRenderingContext
	{
		Guid Id { get; }
	}
}
