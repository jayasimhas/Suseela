using Sitecore.Buckets.FieldTypes;
using Sitecore.Data.Items;

namespace Hi.Sc.Buckets
{
    public class CustomDataSource : IDataSource
    {
        public Item[] ListQuery(Item item)
        {
            string homePath = "/sitecore/content/home";
            Item homeItem = Sitecore.Context.ContentDatabase.GetItem(homePath);
            if (homeItem != null)
            {
                return homeItem.GetChildren().ToArray();
            }
            return new Item[0];
        }
    }
}