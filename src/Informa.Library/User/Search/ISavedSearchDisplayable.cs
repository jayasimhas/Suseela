using System;
using System.Collections.Generic;

namespace Informa.Library.User.Search
{
	public interface ISavedSearchDisplayable : ISavedSearchSaveable
	{
		IEnumerable<string> Sources { get; set; }
		DateTime DateSaved { get; set; }
	}
}