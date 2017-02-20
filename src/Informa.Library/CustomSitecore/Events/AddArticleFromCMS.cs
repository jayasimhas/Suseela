using Autofac;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.SecurityModel;
using System;
using System.Linq;
namespace Informa.Library.CustomSitecore.Events
{
    public class AddArticleFromCMS
    {
        public IArticleSearch _articleSearch;
        public void OnArticleAddedFromCMS(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Item newarticleItem = Event.ExtractParameter(args, 0) as Item;
            if (string.Equals(newarticleItem.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
                    {
                        _articleSearch = scope.Resolve<IArticleSearch>();
                        var publication = newarticleItem.Axes.GetAncestors().FirstOrDefault(ancestor => string.Equals(ancestor.TemplateID.ToString(), ISite_RootConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase));
                        using (new SecurityDisabler())
                        {
                            newarticleItem.Editing.BeginEdit();
                            newarticleItem["Created Date"] = Sitecore.DateUtil.ToIsoDate(DateTime.Now);
                            var articleNum = _articleSearch.GetNextArticleNumber(new Guid(publication?.ID.ToString()));
                            newarticleItem["Article Number"] = GetNextArticleNumber(articleNum, new Guid(publication?.ID.ToString()), _articleSearch.GetPublicationPrefix(publication?.ID.ToString()));
                            newarticleItem.Editing.EndEdit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error in Generating Article Number from CMS", ex, Sitecore.Context.User.GetLocalName());
                }
            }
        }
        public static string GetNextArticleNumber(long lastArticleNumber, Guid publication, string publicationPrefix)
        {
            string number = publicationPrefix + lastArticleNumber.ToString(Constants.ArticleNumberLength);
            return number;
        }

        public void SaveItemNameOnTitleChange(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Item newarticleItem = Event.ExtractParameter(args, 0) as Item;
            if (string.Equals(newarticleItem.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase))
            {

            }
        }
    }
}
