using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Informa.Library.Navigation
{
	public class Navigation : INavigation
	{
        public string Code { get; set; }
        public Link Link { get; set; }
		public string Text { get; set; }
		public IEnumerable<INavigation> Children { get; set; }
	}
}
