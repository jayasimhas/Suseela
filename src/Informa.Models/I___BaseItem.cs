using System.Diagnostics.CodeAnalysis;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Maps;
using Jabberwocky.Glass.Factory;
using Jabberwocky.Glass.Factory.Attributes;
using Jabberwocky.Glass.Models;

namespace Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates
{                                                                    
    public partial interface I___BaseItem 
    {
    }

    [GlassFactoryInterface]
    public interface IListable
    {
        string ListTitle { get; }
        string Url { get; }
        string Topic { get; }
        string DisplayDate { get; }
        string Author { get; }
        string ListImage { get; }
    }
}