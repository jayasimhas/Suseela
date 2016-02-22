using System;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.CustomSitecore.Mvc;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Helpers;
using Glass.Mapper.Sc.Web.Mvc;

namespace Informa.Library.Utilities.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ServerSideInclude(this HtmlHelper helper, string serverPath)
        {
            var filePath = HttpContext.Current.Server.MapPath(serverPath);

            var markup = File.ReadAllText(filePath);
            return new HtmlString(markup);
        }

        public static CustomSitecoreHelper Sitecore(this HtmlHelper htmlHelper)
        {
            Assert.ArgumentNotNull(htmlHelper, "htmlHelper");

            var helperFactory = DependencyResolver.Current.GetService<Func<HtmlHelper, CustomSitecoreHelper>>();

            var threadData = ThreadHelper.GetThreadData<CustomSitecoreHelper>();
            if (threadData != null)
                return threadData;

            var data = helperFactory(htmlHelper);
            ThreadHelper.SetThreadData(data);

            return data;
        }

        public static TokenHtml<T> TokenTransform<T>(this HtmlHelper<T> htmlHelper)
        {
            return new TokenHtml<T>(htmlHelper);
        } 

    }

    //public class GlassTokenHtml
    //{


    //}

  //  public static HtmlTokenHelper<T> Token<T>(this HtmlHelper<T> htmlHelper)
  //      {
  //          return new ArticleHtml<T>(htmlHelper);
  //      }


  //      public string TokenBody<T>(this TokenHtml<T> htmlHelper, string body)
  //      {
  //      //string (T model, Expression<Func<T, object>> field, object attributes = null, bool isEditable = false, string contents = null);

  //      return new MvcHtmlString("");

  //      }

  //      //public static 

  //      public static string CsrfTokenHeaderValue(this HtmlHelper htmlHelper)
		//{
		//	string cookieToken, formToken;
		//	AntiForgery.GetTokens(null, out cookieToken, out formToken);
		//	return cookieToken + ":" + formToken;
		//} 
    }     
