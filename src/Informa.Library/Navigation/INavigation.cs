using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Informa.Library.Navigation
{
	public interface INavigation
	{
        string Code { get; }
		Link Link { get; }
		string Text { get; }
		IEnumerable<INavigation> Children { get; }
	}
}
