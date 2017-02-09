using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Query;
using Sitecore;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Configuration;
using System.Web;
using Informa.Library.Utilities.CMSHelpers;
using Sitecore.Pipelines.GetLookupSourceItems;

namespace Informa.Library.CustomSitecore.Fields
{
    /// <summary>
    /// This field type is like a tree list, but you can specify more than one root item to select from (for example, videos or photos)
    /// The data source roots are specified using pipe delimiting just like regular Sitecore Query language
    /// </summary>
    public class MultipleRootTreelistField : TreeList
    {
        protected override void OnLoad(EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            base.OnLoad(args);

            if (!Sitecore.Context.ClientPage.IsEvent)
            {
                Item item = Sitecore.Context.ContentDatabase.GetItem(base.ItemID);
                if (item != null && item.Name == "__Standard Values")
                {
                    if (args == null)

                        throw new ArgumentNullException(nameof(args));

                    //if (!args.Source.StartsWith("query:"))

                    //    return;

                    var url = WebUtil.GetQueryString();

                    if (string.IsNullOrWhiteSpace(url) || !url.Contains("hdl")) return;

                    var parameters = FieldEditorOptions.Parse(new UrlString(url)).Parameters;

                    var currentItemId = parameters["contentitem"];

                    if (string.IsNullOrEmpty(currentItemId)) return;

                    var contentItemUri = new Sitecore.Data.ItemUri(currentItemId);

                    item = Sitecore.Data.Database.GetItem(contentItemUri);
                }

                if (item != null && item.Name!= "__Standard Values")
                {
                    // find the existing TreeviewEx that the base OnLoad added, get a ref to its parent, and remove it from controls
                    var existingTreeView = (TreeviewEx)WebUtil.FindControlOfType(this, typeof(TreeviewEx));
                    var treeviewParent = existingTreeView.Parent;

                    existingTreeView.Parent.Controls.Clear(); // remove stock treeviewex, we replace with multiroot

                    // find the existing DataContext that the base OnLoad added, get a ref to its parent, and remove it from controls
                    var dataContext = (DataContext)WebUtil.FindControlOfType(this, typeof(DataContext));
                    var dataContextParent = dataContext.Parent;

                    dataContextParent.Controls.Remove(dataContext); // remove stock datacontext, we parse our own

                    // create our MultiRootTreeview to replace the TreeviewEx
                    var impostor = new Sitecore.Web.UI.WebControls.MultiRootTreeview();
                    impostor.ID = existingTreeView.ID;
                    impostor.DblClick = existingTreeView.DblClick;
                    impostor.Enabled = existingTreeView.Enabled;
                    impostor.DisplayFieldName = existingTreeView.DisplayFieldName;

                    // parse the data source and create appropriate data contexts out of it
                    var dataContexts = ParseDataContexts(dataContext, item);

                    impostor.DataContext = string.Join("|", dataContexts.Select(x => x.ID));
                    foreach (var context in dataContexts) dataContextParent.Controls.Add(context);

                    // inject our replaced control where the TreeviewEx originally was
                    treeviewParent.Controls.Add(impostor);
                }
            }
        }

        /// <summary>
        /// Parses multiple source roots into discrete data context controls (e.g. 'dataSource=/sitecore/content|/sitecore/media library')
        /// </summary>
        /// <param name="originalDataContext">The original data context the base control generated. We reuse some of its property values.</param>
        /// <returns></returns>
        protected virtual DataContext[] ParseDataContexts(DataContext originalDataContext, Item item)
        {
            return new ListString(DataSource).Select(x => CreateDataContext(originalDataContext, x, item)).ToArray();
        }

        /// <summary>
        /// Creates a DataContext control for a given Sitecore path data source
        /// </summary>
        protected virtual DataContext CreateDataContext(DataContext baseDataContext, string dataSource, Item item)
        {
            DataContext dataContext = new DataContext();
            dataContext.ID = GetUniqueID("D");
            dataContext.Filter = baseDataContext.Filter;
            dataContext.DataViewName = "Master";
            if (!string.IsNullOrEmpty(DatabaseName))
            {
                dataContext.Parameters = "databasename=" + DatabaseName;
            }
            dataContext.Root = dataSource;
            dataContext.Language = Language.Parse(ItemLanguage);
            //Checking DataSource Have $verticalnode Token
            if (dataContext.Root.Contains("$verticalnode"))
            {
                // On Select Item ID and trying to fetch Item based on ID
                if (!string.IsNullOrEmpty(base.ItemID))
                {
                   // Item item = Sitecore.Context.ContentDatabase.GetItem(base.ItemID);
                    //To detect the source item for the cloned item
                    if (item != null && item.IsClone)
                    {
                        item = item.Source;
                    }
                    if (item != null)
                    {
                        //Feching Ancestor of Selected Item and replaceing with Token
                        //var rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == ItemIdResolver.GetItemIdByKey("VerticalTemplate"));
                        var rootItem = item.Axes.GetAncestors().FirstOrDefault(ancestor => ancestor.TemplateID.ToString() == Settings.GetSetting("VerticalTemplate.global"));

                        if (rootItem != null)
                            dataContext.Root = dataContext.Root.Replace("$verticalnode", rootItem.Name);
                        //ToDo
                        return dataContext;
                    }
                }
            }
            return dataContext;
        }
    }
}
