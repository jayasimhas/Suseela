using AutoMapper;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Utilities.AutoMapper.Resolvers.Nlm
{
    public class ArticleTypeResolver : ValueResolver<ArticleItem, string>
    {
        private const string SidebarType = "sidebar";

        protected override string ResolveCore(ArticleItem articleItem)
        {
            return articleItem.Is_Sidebar_Article 
                ? SidebarType 
                : articleItem.Content_Type?.Item_Name?.ToLowerInvariant();
        }
    }
}
