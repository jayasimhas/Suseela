using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc.Configuration.Attributes;
using Informa.Models.FactoryInterface;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages
{
    partial interface IArticle : IListable
    {

        [SitecoreQuery("./ancestor-or-self::*[@@templateid='{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}']", IsRelative = true
            )]
        Guid Publication { get; set; }

        string LookAtMe { get; set; }
    }
}


namespace Informa.Models.Velir.Search.Models.FactoryInterface
{
    partial interface IInterfaceTemplate
    {
        [SitecoreItem]
        Item BaseItem { get; }   
    }
}
