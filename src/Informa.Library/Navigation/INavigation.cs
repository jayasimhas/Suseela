using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Informa.Library.Navigation
{
	public interface INavigation
	{
		Link Link { get; }
		string Text { get; }
		IEnumerable<INavigation> Children { get; }
        string Code { get; }
    }
}
