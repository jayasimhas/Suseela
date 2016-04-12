using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Velir.Search.Core.ComputedFields;

namespace Informa.Library.Search.ComputedFields.SearchResults
{
    /// <summary>
    /// Sets the value that is used in search results
    /// </summary>
    public class SearchResultTitleField : BaseContentComputedField
    {
        public override object GetFieldValue(Item indexItem)
        {
            var glassItem = indexItem.GlassCast<IGlassBase>(inferType: true);
            var page = glassItem as I___BasePage;
            var title = indexItem.Name;

            //Start by stripping any HTML in the title
            title = HtmlUtil.StripHtml(title);

            if (page == null)
            {
                return HttpUtility.HtmlDecode(title);
            }

            //Try to use the specific page title 
            if (!string.IsNullOrEmpty(page.Title))
            {
                title = page.Title;
            }
            else
            {
                //First fallback is the navigation title
                if (!string.IsNullOrEmpty(page.Navigation_Title))
                {
                    title = page.Navigation_Title;
                }
                else
                {
                    //And finally try the Sitecore Display Name
                    if (!string.IsNullOrEmpty(indexItem.DisplayName))
                    {
                        title = indexItem.DisplayName;
                    }
                }
            }

            //Decode and special characters
            return HttpUtility.HtmlDecode(title);
        }
    }
}