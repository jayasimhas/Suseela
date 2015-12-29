using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper;
using Glass.Mapper.Sc.Maps;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Web.Controllers;
using SitecoreTreeWalker.SitecoreTree;


//namespace SitecoreTreeWalker.SitecoreTree
//{
//    public partial class HDirectoryStruct
//    {
        
//    }
//}

namespace Informa.Web.GlassMaps
{

    [ExcludeFromCodeCoverage] // Nothing to test
    public class TaxonomyItemMap : SitecoreGlassMap<HDirectoryStruct>
    {
        public override void Configure()
        {
            Map(x => x.AutoMap(),
                x => x.TemplateId(ITaxonomy_ItemConstants.TemplateId),
                x =>
                    x.EnforceTemplateAndBase()
                        .Delegate(y => y.Children)
                        .GetValue(z => z.Item.Children.Cast<HDirectoryStruct>()));
        }
    }
}
