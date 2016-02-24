using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    public class ArticleIntegerNumberField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            string ScripPublicationPrefix = "SC";

            string numField = indexItem[IArticleConstants.Article_NumberFieldId];

            int articleNumber = 0;
            if (numField.StartsWith(ScripPublicationPrefix))
            {
                int.TryParse(numField.Remove(0, ScripPublicationPrefix.Length), out articleNumber);
            }
            return articleNumber;
        }
    }
}