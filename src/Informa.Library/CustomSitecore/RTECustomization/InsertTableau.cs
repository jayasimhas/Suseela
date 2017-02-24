using HtmlAgilityPack;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web;
using HtmlControls = Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Events;
using Sitecore.Data;
using Sitecore.Data.Fields;

namespace Informa.Library.CustomSitecore.RTECustomization
{
    public class InsertTableau : DialogForm
    {
        //fields   
        protected HtmlControls.Edit inptPageTitle;
        protected HtmlControls.Edit inptDashboardName;
        protected HtmlControls.Edit inptMobileDashboardName;
        protected HtmlControls.Checkbox chkDisplayTabs;
        protected HtmlControls.Checkbox chkAllowCustomViews;
        protected HtmlControls.Checkbox chkDisplayToolbars;
        protected HtmlControls.Edit inptFilter;
        protected HtmlControls.Edit inptHeight;
        protected HtmlControls.Edit inptWidth;

        protected string Mode
        {
            get
            {
                string str = StringUtil.GetString(base.ServerProperties["Mode"]);
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return "shell";
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                base.ServerProperties["Mode"] = value;
            }
        }
        //setup page
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                this.Mode = WebUtil.GetQueryString("mo");
                string text = WebUtil.GetQueryString("selectedText");
            }
        }

        /// <summary>
        /// When pressed Insert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            var itemId = System.Web.HttpContext.Current.Request.QueryString[2];
            var currentItem = Database.GetDatabase("master").GetItem(new ID(itemId));
            string DashboardName = inptDashboardName.Value;
            string MobileDashboardName = inptMobileDashboardName.Value;
            bool DisplayTabs = chkDisplayTabs.Checked;
            bool AllowCustomViews = chkAllowCustomViews.Checked;
            bool DisplayToolbars = chkDisplayToolbars.Checked;
            string Filter = inptFilter.Value;
            string Height = inptHeight.Value;
            string Width = inptWidth.Value;
            string PageTitle = inptPageTitle.Value;

            TemplateItem tableuDashboardtemplate = Database.GetDatabase("master").GetItem("{580A652A-EB37-446A-A16B-B3409C902FE5}");
            TemplateItem pageAssetsTemplate = Database.GetDatabase("master").GetItem("{EBEB3CE7-6437-4F3F-8140-F5C9A552471F}");
            Item currentArticleItem = Database.GetDatabase("master").GetItem(WebUtil.GetQueryString("contextItem"));
            if (currentArticleItem != null && tableuDashboardtemplate != null && pageAssetsTemplate != null)
            {
                using (new SecurityDisabler())
                {
                    
                    Item PageAssets = currentArticleItem.Add("PageAssets", pageAssetsTemplate);
                    Item tableau = PageAssets.Add("tableau", tableuDashboardtemplate);
                    tableau.Editing.BeginEdit();
                    tableau["Page Title"] = PageTitle;
                    tableau["Dashboard Name"] = DashboardName;
                    tableau["Mobile Dashboard Name"] = MobileDashboardName;
                    ((CheckboxField)tableau.Fields["Display Tabs"]).Checked = DisplayTabs;
                    ((CheckboxField)tableau.Fields["Allow Custom Views"]).Checked = AllowCustomViews;
                    ((CheckboxField)tableau.Fields["Display Toolbars"]).Checked = DisplayToolbars;
                    tableau["Filter"] = Filter;
                    tableau["Width"] = Height;
                    tableau["Height"] = Width;
                    tableau.Editing.EndEdit();

                    string tableauToken = getTokenForTableau(tableau.ID.ToString(), currentArticleItem[DashboardName + "-sourceid"]);
                    
                    SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(tableauToken) + ")");
                    
                }
            }
        }

        private string getTokenForTableau(string tableau, string sourceid)
        {
            tableau = tableau.Replace("{", String.Empty).Replace("}", String.Empty);

            return "<strong id=" + sourceid + ">[T#:" + tableau + "]</strong>";
        }

        /// <summary>
        /// When pressed cancelled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnCancel(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            if (this.Mode == "webedit")
            {
                base.OnCancel(sender, args);
            }
            else
            {
                SheerResponse.Eval("scCancel()");
            }
        }

    }
}
