using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace Informa.Library.Authors.ComputedFields
{
    public class IsValid : IComputedIndexField
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

		    try
		    {
		        var field = (Sitecore.Data.Fields.CheckboxField) item.Fields[I___PersonConstants.InactiveFieldId];
		        return !field.Checked;
		    }
		    catch
		    {
		        // ignored
                // going to return false anyhow
		    }

		    return false;
		}
	}
}
