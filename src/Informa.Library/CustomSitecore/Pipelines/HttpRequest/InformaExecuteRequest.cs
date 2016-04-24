using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Pipelines.Processors;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Web;

namespace Informa.Library.CustomSitecore.Pipelines.HttpRequest
{
    public class InformaExecuteRequest : ExecuteRequest
    {
        private HttpRequestArgs _args;

        public override void Process(HttpRequestArgs args)
        {
            _args = args;
            base.Process(args);
        }

        protected override void RedirectOnItemNotFound(string url)
        {
            try
            {
                //string notFoundContent = WebUtil.ExecuteWebPage("/");
                //if (string.IsNullOrWhiteSpace(notFoundContent))
                //{
                //    return;
                //}
                //_args.Context.Response.TrySkipIisCustomErrors = true;
                //_args.Context.Response.StatusCode = 404;
                //_args.Context.Response.StatusDescription = "Page Not Found";
                //_args.Context.Response.ContentType = "text/html";
                //_args.Context.Response.Write(notFoundContent);
                //_args.Context.Response.End();
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    if (Settings.RequestErrors.UseServerSideRedirect)
                        HttpContext.Current.Server.Transfer("/");
                    else
                        WebUtil.Redirect("/", false);
                }
            }
            catch (Exception e)
            {
                //if (Settings.RequestErrors.UseServerSideRedirect)
                //    HttpContext.Current.Server.Transfer("/");
                //else
                //    WebUtil.Redirect("/", false);
            }
        }
    }
}