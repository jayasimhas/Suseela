using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Autofac;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Mvc;

namespace Informa.Web.sitecore_modules.Shell.Informa {
    public partial class FreeTrialArticleSearch : System.Web.UI.Page {

        protected Database currentDB;
        protected Item folderItem;

        /* Querystring Params available
		    id=%7b59A62A95-9E5D-4478-BDC9-1E793823C48F%7d
		    la=en
		    language=en
		    vs=1
		    version=1
		    database=master
		    readonly=0
		    db=master
	    */

        protected void Page_Load(object sender, EventArgs e) {
            
            string dbName = WebUtil.GetQueryString("db");
            currentDB = (!string.IsNullOrEmpty(dbName))
                ? Sitecore.Configuration.Factory.GetDatabase(dbName)
                : Sitecore.Context.ContentDatabase;

            string idStr = WebUtil.GetQueryString("id");
            if (Sitecore.Data.ID.IsID(idStr))
                folderItem = currentDB.GetItem(Sitecore.Data.ID.Parse(idStr));

            if (folderItem == null)
                return;

            rptFreeTrialArticles.DataSource = GetFreeTrialArticles(folderItem);
            rptFreeTrialArticles.DataBind();
        }

        protected List<IArticle> GetFreeTrialArticles(Item rootItem) {
            IArticleSearch searcher = DependencyResolver.Current.GetService<IArticleSearch>();
            if (searcher == null)
                return new List<IArticle>();

            IArticleSearchFilter filter = searcher.CreateFilter();
              
            var results = searcher.FreeWithRegistrationArticles(currentDB.Name);
            string resultsFormat = "There were {0} articles marked 'Free with Registration'";
            string resultFormat = "There was {0} article marked 'Free with Registration'";
            if (results.Articles.Any()) {
                var filteredResults = results.Articles.Where(a => a._Path.StartsWith(rootItem.Paths.FullPath)).ToList();
                ltlResultCount.Text = string.Format((filteredResults.Count > 1) ? resultsFormat : resultFormat, filteredResults.Count);
                return filteredResults;
            } else {
                ltlResultCount.Text = string.Format(resultsFormat, 0);
                return new List<IArticle>();
            }            
        }
    }
}