using HtmlAgilityPack;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.Articles
{
    public interface IArticleComponentFactory
    {
        string Component(string componentId, IArticle article);
    }



    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticleComponentFactory : IArticleComponentFactory
    {
        #region Implementation of IArticleComponentFactory

        public string Component(string componentId, IArticle article)
        {
            var componetString = "";
            if (article == null)
            {
                return componetString;
            }   
            
            var document =  new HtmlDocument();
            document.LoadHtml(article.Body);

            var componentNode = document.DocumentNode.SelectSingleNode($"//*[@data-mediaid='{componentId}']");

            return componentNode.OuterHtml;
        }                                  
    }

        #endregion
    }