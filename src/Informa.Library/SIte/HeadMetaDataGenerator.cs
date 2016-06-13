using System;
using System.Collections.Generic;
using System.Text;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Site
{
    public enum OpenGraphTypes
    {
        website, article
    }

    public interface IHeadMetaDataGenerator
    {
        string GetMetaHtml();
    }

    [AutowireService]
    public class HeadMetaDataGenerator : IHeadMetaDataGenerator
    {
        private readonly IDependencies _dependencies;
        private Dictionary<string, string> _props;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreContext SitecoreContext { get; }
            ISiteRootContext SiteRootContext { get; }
            IHttpContextProvider HttpContextProvider { get; }
        }

        public HeadMetaDataGenerator(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public string GetMetaHtml()
        {
            var currentItem = _dependencies.SitecoreContext.GetCurrentItem<I___BasePage>();
            var siteRoot = _dependencies.SiteRootContext.Item;
            var currentUri = _dependencies.HttpContextProvider.RequestUri;

            if (currentItem == null || siteRoot == null) return string.Empty;

            _props = new Dictionary<string, string>();

            AddGlobalMeta(currentUri);
            AddPublicationRootMeta(siteRoot, currentUri);
            AddBasePageMeta(currentItem);

            return TransformToHtml(_props);
        }

        public string TransformToHtml(IEnumerable<KeyValuePair<string, string>> properties)
        {
            var result = new StringBuilder();
            properties.Each(prop => result.AppendLine($"<meta property=\"{prop.Key}\" content=\"{prop.Value}\">"));
            return result.ToString();
        }

        private void AddGlobalMeta(Uri uri)
        {
            _props["og:url"] = uri?.AbsoluteUri;
            _props["og:type"] = OpenGraphTypes.website.ToString();  // this may be override by specific page types
        }

        private void AddPublicationRootMeta(ISite_Root siteRoot, Uri uri)
        {
            var host = !string.IsNullOrEmpty(uri?.Host) ? uri.Host : null;
            var path = !string.IsNullOrEmpty(siteRoot.Site_Logo?.Src) ? siteRoot.Site_Logo?.Src : null;
            var siteLogoUrl = (host != null && path != null)
                ? host + path
                : string.Empty;

            _props["og:image"] = _props["twitter:image"] = _props["twitter:card"] = siteLogoUrl;
            _props["og:site_name"] = siteRoot.Publication_Name;
            _props["twitter:site"] = siteRoot.Twitter_Handle;
        }

        private void AddBasePageMeta(I___BasePage basePage)
        {
            _props["og:description"] = _props["twitter:description"] = basePage.Meta_Description;
            _props["og:title"] = _props["twitter:title"] =
                string.IsNullOrEmpty(basePage.Meta_Title_Override)
                    ? basePage.Title
                    : basePage.Meta_Title_Override;
        }
    }
}