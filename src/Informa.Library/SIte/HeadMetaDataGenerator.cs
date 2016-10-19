using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;
using Glass.Mapper.Sc;
using Informa.Library.Logging;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Text.RegularExpressions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Custom_Tags;

namespace Informa.Library.Site
{
    public enum OpenGraphTypes
    {
        website, article
    }

    public interface IHeadMetaDataGenerator
    {
        string GetMetaHtml();
        string GetCustomTags(int i);
    }

    [AutowireService]
    public class HeadMetaDataGenerator : IHeadMetaDataGenerator
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreContext SitecoreContext { get; }
            ISiteRootContext SiteRootContext { get; }
            IHttpContextProvider HttpContextProvider { get; }
            ILogWrapper LogWrapper { get; }
        }

        public HeadMetaDataGenerator(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string _host;
        private string Host => _host ?? (_host = _dependencies.HttpContextProvider.RequestUri?.Host);

        public string GetMetaHtml()
        {
            var props = BuildPropertyDictionary();

            if (props == null)
            {
                var url = _dependencies.HttpContextProvider.RequestUri?.OriginalString;
                _dependencies.LogWrapper.SitecoreError($"Failed to build page metadata while loading: {url}");
                return
                    "<script type='text/javascript'>if(console){console.error('Failed to load page metadata.')}</script>";
            }

            return TransformToHtml(props);
        }

        public Dictionary<string, string> BuildPropertyDictionary()
        {
            var currentItem = _dependencies.SitecoreContext.GetCurrentItem<IGlassBase>(inferType:true);
            var siteRoot = _dependencies.SiteRootContext.Item;
            var basePage = currentItem as I___BasePage;

            if (currentItem == null || basePage == null || siteRoot == null) { return null; }

            var currentUri = _dependencies.HttpContextProvider.RequestUri;

            var properties = new Dictionary<string, string>();

            AddGlobalMeta(properties, currentUri, siteRoot);
            AddBasePageMeta(properties, basePage);

            var ogPage = currentItem as I___OpenGraph;
            if(ogPage != null) { AddOpenGraphMeta(properties, ogPage); }

            var article = currentItem as IArticle;
            if (article != null) { AddArticleMeta(properties, article); }

            //Meta meta data
            properties["twitter:card"] = properties.ContainsKey("twitter:image")
                                         && properties["twitter:image"].HasContent()
                                         && !properties["twitter:image"].Equals(GetImageFullUrl(siteRoot.Site_Logo?.Src),
                                                StringComparison.InvariantCultureIgnoreCase)
                                                    ? "summary_large_image"
                                                    : "summary";

            return properties;
        }

        public string TransformToHtml(IEnumerable<KeyValuePair<string, string>> properties)
        {
            var result = new StringBuilder();

            foreach (var pair in properties)
            {
                string propertyName = (pair.Key.ToLower().StartsWith("twitter")) ? "name" : "property";
                result.AppendLine($"<meta {propertyName}=\"{pair.Key}\" content=\"{pair.Value}\">");
            }

            return result.ToString();
        }

        public void AddGlobalMeta(IDictionary<string, string> props, Uri uri, ISite_Root siteRoot)
        {
            props["og:url"] = uri?.AbsoluteUri;
            props["og:type"] = OpenGraphTypes.website.ToString();
            props["og:site_name"] = siteRoot.Publication_Name;
            props["twitter:site"] = siteRoot.Twitter_Handle;

            var siteRootLogoUrl = GetImageFullUrl(siteRoot.Site_Logo?.Src);
            props["og:image"] = props["twitter:image"] = siteRootLogoUrl;
        }

        public void AddBasePageMeta(IDictionary<string, string> props, I___BasePage basePage)
        {
            props["og:title"] = props["twitter:title"] = basePage.Meta_Title_Override;
            props["og:description"] = props["twitter:description"] = basePage.Meta_Description;
        }

        public void AddOpenGraphMeta(IDictionary<string, string> props, I___OpenGraph ogPage)
        {
            var ogImageUrl = GetImageFullUrl(ogPage.Og_Image?.Src);
            ogImageUrl.HasContent().Then(() => props["og:image"] = props["twitter:image"] = ogImageUrl);
            ogPage.Og_Title.HasContent().Then(() => props["og:title"] = props["twitter:title"] = ogPage.Og_Title);
            ogPage.Og_Description.HasContent().Then(() => props["og:description"] = props["twitter:description"] = ogPage.Og_Description);
        }

        public void AddArticleMeta(IDictionary<string, string> props, IArticle article)
        {
            props["og:type"] = "article";
            props["og:title"] = props["twitter:title"] = article.Title;
            props["og:description"] = article.Summary;
            props["twitter:description"] = article.Summary;

            var imageUrl = GetImageFullUrl(article.Featured_Image_16_9?.Src);
            props["og:image"] = imageUrl;
            props["twitter:image"] = imageUrl;

            props["article:published_time"] = FormatDateTime(article.Actual_Publish_Date);
            props["article:modified_time"] = FormatDateTime(article.Modified_Date);
            props["article:author"] = ParseAuthors(article.Authors);

            string section, tags;
            ParseTags(article.Taxonomies.ToArray(), out section, out tags);
            props["article:section"] = section;
            props["article:tag"] = tags;
        }

        private string FormatDateTime(DateTime time)
        {
            return time == DateTime.MinValue
                ? string.Empty
                : time.ToUniversalTime().ToString("o");
        }

        private string GetImageFullUrl(string src)
        {
            return (Host.HasContent() && src.HasContent())
                ? Host + src
                : string.Empty;
        }

        private static string ParseAuthors(IEnumerable<IStaff_Item> authors)
        {
            if(authors == null) { return string.Empty; }

            return string.Join(", ", authors.Select(a => $"{a.First_Name} {a.Last_Name}"));
        }

        private static void ParseTags(ITaxonomy_Item[] taxonomy, out string section, out string tags)
        {
            if (taxonomy.IsNullOrEmpty())
            {
                section = tags = string.Empty;
                return;
            }

            var first = taxonomy.FirstOrDefault();
            if (first == null)
            {
                section = tags = string.Empty;
                return;
            }

            section = first.Item_Name;
            string temp = string.Join(",", taxonomy.Skip(1).Select(t => t.Item_Name));
            tags = Regex.Replace(temp, @"\r\n?|\n|\t", string.Empty);
        }

        public string GetCustomTags(int i)
        {
            bool displayInHead = i == 0 ? true : false;
            var siteRoot = _dependencies.SiteRootContext.Item;
            var globalItemFolder = siteRoot._ChildrenWithInferType.FirstOrDefault(ch => ch._Name.Equals("Globals"));
            if (globalItemFolder != null)
            {
                var customtagsFolder = globalItemFolder._ChildrenWithInferType.FirstOrDefault(ch => ch._Name.Equals("CustomTags"));
                if (customtagsFolder != null && customtagsFolder._ChildrenWithInferType.OfType<ICustom_Tag>().Any())
                {
                    StringBuilder tagBuilder = new StringBuilder();
                    foreach (var ctag in customtagsFolder._ChildrenWithInferType.OfType<ICustom_Tag>().Where(p=>p.Display_In_Head.Equals(displayInHead)))
                    {
                        tagBuilder.Append(ctag.Custom_Tag);
                    }
                    return tagBuilder.ToString();

                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;

        }
    }
}