using System.Text.RegularExpressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Informa.Library.Authors.ComputedFields
{
    public class AuthorUrlName : IComputedIndexField
	{
		public string FieldName { get; set; }
		public string ReturnType { get; set; }

		public object ComputeFieldValue(IIndexable indexable)
		{
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
            {
                return null;
            }

		    var name = item.DisplayName.ToLower();
		    var nonChars = new Regex(@"[^a-z]+");
		    name = nonChars.Replace(name, "-");
		    name = name.Trim('-');
            
			return name;
		}
	}
}
