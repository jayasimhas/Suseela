using System.Linq;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class ArticleIntegerNumberField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            //string ScripPublicationPrefix = Constants.ScripRootNodeIntials;

            string numField = indexItem[IArticleConstants.Article_NumberFieldId];

            int articleNumber = 0;
            foreach (var eachPublicationPrefix in Constants.PublicationPrefixCollection.Where(eachPublicationPrefix => numField.StartsWith(eachPublicationPrefix.Key)))
            {
                int.TryParse(numField.Remove(0, eachPublicationPrefix.Key.Length), out articleNumber);
            }
            return articleNumber;
        }
    }
}