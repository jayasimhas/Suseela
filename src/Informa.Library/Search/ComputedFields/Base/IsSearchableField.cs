using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.Base
{
    public class IsSearchableField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            IGlassBase glassItem = indexItem.GlassCast<IGlassBase>(inferType: true);

            I___BasePage page = glassItem as I___BasePage;

            if (page == null) return false;

            if (!glassItem._Path.StartsWith("/sitecore/content")) return false;

            return page.Include_In_Search;
        }
    }
}
