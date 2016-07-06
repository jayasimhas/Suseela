using Autofac;
using Jabberwocky.Glass.Autofac.Util;
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

		    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
		    {
		        return scope.Resolve<IAuthorService>().ConvertAuthorNameToUrlName(item.Name);
		    }
		}
	}
}
