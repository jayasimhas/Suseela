using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.HtmlControls.Data;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using log4net;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.SecurityModel;
using Informa.Library.Services.Global;
using Sitecore.Sites;

namespace Informa.Library.CustomSitecore.RTECustomization
{
    public class InsertSidebarArticles : DialogForm
    {
        #region Fields
        
        protected Edit Filename;
        protected Edit searchText;       
        protected Combobox VerticalCombo;


        protected string articleType;
        protected string itemID;
        //private readonly ILog _logger;
        #endregion Fields

        #region Properties
        Database database = Context.ContentDatabase ?? Context.Database;
        Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
        /// <summary>
        /// Gets the content language.
        /// 
        /// </summary>
        protected Sitecore.Globalization.Language ContentLanguage
        {
            get
            {
                Language result;
                if (!Language.TryParse(WebUtil.GetQueryString("la"), out result))
                    result = Context.ContentLanguage;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the mode.
        /// 
        /// </summary>
        protected string Mode
        {
            get
            {
                return Assert.ResultNotNull<string>(StringUtil.GetString(this.ServerProperties["Mode"], "shell"));
            }
            set
            {
                Assert.ArgumentNotNull((object)value, "value");
                this.ServerProperties["Mode"] = (object)value;
            }
        }
        #endregion



        /// <summary>
        /// On load of the dialog form
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(System.EventArgs e)
        {
            //_logger.Error("On load of multisearchRTE");
            Assert.ArgumentNotNull((object)e, "e");
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            this.Mode = WebUtil.GetQueryString("mo");           
        }


        /// <summary>
        /// On click of Insert in the Dialog form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            articleType = WebUtil.GetQueryString("articletype");
            itemID = WebUtil.GetQueryString("itemid");
          
            if (Filename != null)
            {
                var returnText = SetReturnText(itemID, articleType, Filename.Value);
                if (articleType == "sidebar")
                    SheerResponse.Eval("scClose(" + StringUtil.EscapeJavascriptString(returnText) + ")");
                else if (articleType == "referenced")
                    SheerResponse.Eval("scCloseAndUpdateReferencedArticles(" + StringUtil.EscapeJavascriptString(returnText) + ")");
            }
            else
                SheerResponse.Alert("Please select atleast one article");         
        }


        public string SetReturnText(string currentItemId, string articleType, string selectedItemId)
        {
            //UpdateReferencedArticles(currentItemId, selectedItemId);

            string placeholder = string.Empty;
            if (!string.IsNullOrEmpty(articleType))
            {
                var articleNumber = GetArticleNumber(selectedItemId);
                if (articleType == "sidebar")
                {
                    placeholder = "[Sidebar#";
                    placeholder += articleNumber + "]";

                    return placeholder;
                }
                else if (articleType == "referenced")
                {
                    placeholder = "[A#";
                    placeholder += articleNumber + "]";
                    return placeholder;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the article number using the item id.
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns></returns>
        protected string GetArticleNumber(string id)
        {
            var article = database.GetItem(new ID(id));
            if (article.Fields[IArticleConstants.Article_NumberFieldName] != null)
                return article.Fields[IArticleConstants.Article_NumberFieldName].Value;
            else
                return string.Empty;
        }


        /// <summary>
        /// On cancel of the dialog form
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