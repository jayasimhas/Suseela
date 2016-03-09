using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Elsevier.Library.CustomItems.Publication.General;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Velir.Utilities;

namespace Elsevier.Web.services
{
    /// <summary>
    /// Summary description for VirtualWhiteBoard
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class VirtualWhiteBoard : System.Web.Services.WebService
    {

        [WebMethod]
        public void UpdateEditorialNotes(string itemID, string text)
        {
            Sitecore.Data.Database masterDB = Factory.GetDatabase("master");

            Item currentItem = masterDB.GetItem(itemID);

            using (new Sitecore.SecurityModel.SecurityDisabler())
            {

                using (new EditContext(currentItem))
                {
                    currentItem.Editing.BeginEdit();
                    currentItem[IArticleConstants.Editorial_NotesFieldId] = text.StripHtmlTags();
                    currentItem.Editing.AcceptChanges();
                    currentItem.Editing.EndEdit();

                }
            }

        }
    }
}
