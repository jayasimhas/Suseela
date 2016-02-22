using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.Article.Search;

namespace Informa.Library.Utilities.Extensions
{
    public class TokenHtml<TK>
    {
        protected GlassHtmlMvc<TK> GlassHtml { get; private set; }       
        protected HtmlHelper<TK> HtmlHelper { get; private set; }        
        protected TextWriter Output { get; private set; }                
        protected TK Model { get; set; }

        public ISitecoreContext SitecoreContext => GlassHtml.SitecoreContext;
        public IArticleSearch ArticleSearch { get; private set; }

        public static string Token = @"\[Sidebar#(.*?)\]";
        private readonly object ArticleListableFactory;

        public TokenHtml(HtmlHelper<TK> helper)
        {
            HtmlHelper = helper;
            GlassHtml = helper.Glass();
            Model = HtmlHelper.ViewData.Model;

            ArticleSearch = DependencyResolver.Current.GetService<IArticleSearch>();

        }                                       

        /// <summary>
        /// Render HTML for a link
        /// 
        /// </summary>
        /// <typeparam name="T">The model type</typeparam><param name="model">The model</param><param name="field">The link field to user</param><param name="attributes">Any additional link attributes</param><param name="isEditable">Make the link editable</param><param name="contents">Content to override the default decription or item name</param>
        /// <returns/>
        public virtual IHtmlString RenderTokenBody(Expression<Func<TK, string>> expression, string partialName)
        {

            var sidebarRegex = new Regex(@"\[Sidebar#(.*?)\]");   
             
            var fieldValue = expression.Compile()(this.Model);

            foreach (Match match in sidebarRegex.Matches(fieldValue))
            {
                string articleNumber = match.Groups[1].Value;


                IArticleSearchFilter filter = ArticleSearch.CreateFilter();
                filter.ArticleNumber = articleNumber;
                var results = ArticleSearch.Search(filter);

                HtmlString replace = new HtmlString("");

                if (results.Articles.Any())
                {
                    replace = HtmlHelper.Partial(partialName, results.Articles.FirstOrDefault());
                }     
               
                fieldValue = fieldValue.Replace(match.Value, replace.ToString());
               
            }

            return HtmlHelper.Raw(fieldValue);
            //return new HtmlString(this.GlassHtml.RenderLink<T>(model, field, attributes, isEditable, contents));
        }
    }
}