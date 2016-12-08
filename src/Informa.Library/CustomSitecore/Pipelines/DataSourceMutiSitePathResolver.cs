using System.Web;
using Sitecore.Pipelines.LoggedIn;
using Sitecore.Web;
using Sitecore.Pipelines.GetLookupSourceItems;
using System;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;

namespace Informa.Library.CustomSitecore.Pipelines
{
    public class DataSourceMutiSitePathResolver
    {
        public void Process(GetLookupSourceItemsArgs args)
        {
            if (args == null)

                throw new ArgumentNullException(nameof(args));

            if (!args.Source.StartsWith("query:"))

                return;

            var url = WebUtil.GetQueryString();

            if (string.IsNullOrWhiteSpace(url) || !url.Contains("hdl")) return;

            var parameters = FieldEditorOptions.Parse(new UrlString(url)).Parameters;

            var currentItemId = parameters["contentitem"];

            if (string.IsNullOrEmpty(currentItemId)) return;

            var contentItemUri = new Sitecore.Data.ItemUri(currentItemId);

            args.Item = Sitecore.Data.Database.GetItem(contentItemUri);
        }
    }
}