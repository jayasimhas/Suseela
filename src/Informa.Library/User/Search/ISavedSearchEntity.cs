using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Search
{
	public interface ISavedSearchEntity
	{
		string Username { get; set; }
		string Title { get; set; }
		string Url { get; set; }
		bool HasAlert { get; set; }
		DateTime DateSaved { get; set; }
	}
}
