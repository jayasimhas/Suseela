using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class SearchResultTitleField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            var glassItem = indexItem.GlassCast<IGlassBase>(inferType: true);
            var page = glassItem as I___BasePage;

            if (page == null)
            {
                return indexItem.Name;
            }

            if (!string.IsNullOrEmpty(page.Title))
            {
                return page.Title;
            }

            if (!string.IsNullOrEmpty(page.Navigation_Title))
            {
                return page.Navigation_Title;
            }

            if (!string.IsNullOrEmpty(indexItem.DisplayName))
            {
                return indexItem.DisplayName;
            }

            return indexItem.Name;
        }
    }
}
