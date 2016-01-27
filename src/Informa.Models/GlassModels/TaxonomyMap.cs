using System.Linq;
using Glass.Mapper.Sc.Maps;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Models;

namespace Informa.Models.GlassModels
{
    public class TaxonomyMap : SitecoreGlassMap<ITaxonomyHierarchy>
    {
        public override void Configure()
        {
            Map(x => x.Delegate(y => y.LongIds).GetValue(z => z.Item.Paths.LongID.Split('/')
                .Select(tag => z.Service.GetItem<IGlassBase>(tag, true, true)).OfType<ITaxonomy_Item>()),
                x => x.Delegate(y => y.Category).GetValue(z => z.Item.Paths.LongID.Split('/')
                    .Select(tag => z.Service.GetItem<IGlassBase>(tag, true, true)).OfType<IFolder>().FirstOrDefault()?._Name ?? string.Empty));
        }
    }
}