using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Utilities.CMSHelpers
{
    public class SiteRootHelper
    {
        public static Item GetSiteRoot(Item item)
        {
            return item.Axes.SelectSingleItem("ancestor-or-self::*[@@templateid ='{DD003F89-D57D-48CB-B428-FFB519AACA56}']");
        } 
    }
}
