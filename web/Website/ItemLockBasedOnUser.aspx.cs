using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Autofac;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Search;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Informa.Web
{
    public partial class ItemLockBasedOnUser : System.Web.UI.Page
    {
        StringBuilder sb = new StringBuilder();
        int articleCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static List<IArticle> SearchArticlesByAuthorName(string authorName)
        {
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
            {
                using (new DatabaseSwitcher(Sitecore.Data.Database.GetDatabase("master")))
                {
                    IArticleSearch articleSearch = scope.Resolve<IArticleSearch>();
                    IArticleSearchFilter filter = articleSearch.CreateFilter();
                    filter.AuthorGuids.Add(authorName);
                    var results = articleSearch.SearchArticleByAuthorName(filter);
                    if (results != null)
                    {
                        var articlesByAuthorName = results.Articles.ToList();
                        return articlesByAuthorName;
                    }
                    return null;
                }
            }
        }

        Database sitecoreDb = Sitecore.Data.Database.GetDatabase("master");

        protected void btnLockArticle_Click(object sender, EventArgs e)
        {
            string inputAuthorName = txtUserName.Text;
            string authorItemID = string.Empty;
            Sitecore.Data.Items.Item[] allAuthorItems = sitecoreDb.SelectItems("fast:/sitecore/content/Environment Globals/Staff/*[@@templateid='{DAF954E3-BCC7-4353-996F-1CAF9D35B44C}']");
            foreach (var authorItem in allAuthorItems)
            {
                if (authorItem.Name == txtUserName.Text || authorItem.Name.Contains(txtUserName.Text))
                {
                    if (!(string.IsNullOrEmpty(authorItem.Fields["First Name"].Value) && string.IsNullOrEmpty(authorItem.Fields["Last Name"].Value) || string.IsNullOrEmpty(authorItem.Fields["Email Address"].Value)))
                    {
                        string userFullname = string.Concat(authorItem.Fields["First Name"].Value, " ", authorItem.Fields["Last Name"].Value);
                        if (userFullname.ToLower() != null)
                        {
                            if ((userFullname.ToLower() == txtUserName.Text.ToLower()) && (txtEmail.Text.ToLower() == authorItem.Fields["Email Address"].Value.ToLower()))
                            {
                                authorItemID = authorItem.ID.ToShortID().ToString();
                                break;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(authorItemID))
            {
                var articleslist = SearchArticlesByAuthorName(authorItemID.ToLower());
                foreach (var item in articleslist)
                {
                    if (item != null)
                    {
                        if (sitecoreDb != null)
                        {
                            Item artcleItem = sitecoreDb.GetItem(item._Path);
                            if (artcleItem != null)
                                LockArticle(artcleItem);
                        }
                    }
                }
                if (articleCount > 0)
                {
                    lblLockedItemsCount.Text = articleCount.ToString() + " Articles Locked for this Particular User";
                }
                else
                {
                    lblLockedItemsCount.Text = "Already all Articles are locked in CMS if you want to edit, Please Contact with Admin";
                }
                lblLockedItems.Text = sb.ToString();
            }
        }

        public void LockArticle(Item article)
        {
            if (!article.Locking.IsLocked())
            {
                using (new EditContext(article))
                {
                    article.Locking.Lock();
                    articleCount++;
                    sb.Append(article.Paths.FullPath).Append("</br>");
                }
            }

        }
    }
}

