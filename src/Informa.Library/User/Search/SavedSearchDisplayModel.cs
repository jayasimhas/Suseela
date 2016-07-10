using System;
using System.Collections.Generic;

namespace Informa.Library.User.Search
{
	public class SavedSearchDisplayModel : SavedSearchInput, ISavedSearchDisplayable
	{
		public IEnumerable<string> Sources { get; set; }
		public DateTime DateSaved { get; set; }
	}
}