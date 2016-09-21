using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;

namespace Informa.Library.Utilities.CMSHelpers
{

    public class ItemIdResolver
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly GlassVerticalRootContext VerticalRootContext;
        static ItemIdResolver itemResolver;
        public ItemIdResolver()
        {
            SitecoreContext = new SitecoreContext();
            VerticalRootContext = new GlassVerticalRootContext(SitecoreContext);
        }
        
        public static string GetItemIdByKey(string itemKey)
        {
            string itemId = string.Empty;
            itemResolver = new ItemIdResolver();
            var vertical = itemResolver.GetVerticalName();           
            if (vertical!=null && !string.IsNullOrEmpty(vertical._Path))
            {
                string[] pathParts = vertical._Path.Split('/');
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + "global");
                if(string.IsNullOrEmpty(itemId))
                    itemId = Sitecore.Configuration.Settings.GetSetting(itemKey + "." + pathParts[3].ToLower());
            }
            else
            {
                itemId = Sitecore.Configuration.Settings.GetSetting(itemKey);
            }
            return itemId;
        }

        public IVertical_Root GetVerticalName()
        {
           return VerticalRootContext.Item;
        }
    }
}
