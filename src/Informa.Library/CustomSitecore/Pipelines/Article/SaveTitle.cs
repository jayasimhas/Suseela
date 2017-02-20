using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Save;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.CustomSitecore.Pipelines.Article
{
    public class SaveTitle
    {
        public void Process(SaveArgs args)
        {
            foreach (var saveItem in args.Items) //loop through item(s) being saved
            {
                var item = Sitecore.Context.ContentDatabase.GetItem(saveItem.ID, saveItem.Language, saveItem.Version); //get the actual item being saved
                if (item == null)
                    continue;
                if (string.Equals(item.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase))
                {

                    item.Fields.ReadAll();
                    var field = item.Fields[IArticleConstants.TitleFieldId];
                    if (field != null)
                    {
                        if (!string.Equals(item.Name, field.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            using (new SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                item.Name = ItemUtil.ProposeValidItemName(field.Value);
                                item.Editing.EndEdit();
                            }
                        }
                    }
                }
            }
        }
    }
}
