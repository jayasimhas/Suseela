using System;
using System.Text;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Library.Services.NlmExport.Parser.Legacy.Link;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;

namespace Informa.Library.Services.NlmExport.Parser.Legacy
{
    public static class XmlWriterHelper
    {
        public static IDisposable StartElement(this XmlWriter writer, string elementName)
        {
            return new DisposableElementWriter(writer, elementName);
        }

        private class DisposableElementWriter : IDisposable
        {
            private readonly XmlWriter _writer;

            public DisposableElementWriter(XmlWriter writer, string elementName)
            {
                _writer = writer;
                _writer.WriteStartElement(elementName);
            }

            public void Dispose()
            {
                _writer.WriteEndElement();
            }
        }
    }

    public static class Helpers
    {
        public static string ConvertSpecialCharacters(string inputHtml)
        {
            inputHtml = inputHtml.Replace(Link.LinkNode.GreaterThanTemporaryIdentifier, ">");
            inputHtml = inputHtml.Replace(Link.LinkNode.LessThanTemporaryIdentifier, "<");

            var outputString = new StringBuilder();
            foreach (var character in inputHtml)
            {
                if ((int)character > 128)
                {
                    outputString.AppendFormat("&#{0};", (int) character);
                }
                else
                {
                    outputString.Append(character);
                }
            }

            return outputString.ToString();
        }

        /// <summary>
        /// Resolves Dynamic Links to either the item path or the media item path with extension
        /// </summary>
        /// <param name="link">The href value of a node</param>
        /// <param name="resolvedLink">Output of the resolved link.</param>
        /// <returns></returns>
        public static bool TryResolveInternalLink(string link, out string resolvedLink)
        {
            DynamicLink parsed;
            if (DynamicLink.TryParse(link, out parsed))
            {
                var item = Factory.GetDatabase(Constants.MasterDb).GetItem(parsed.ItemId);
                if (item != null)
                {
                    if (item.Paths.IsMediaItem)
                    {
                        bool includesExtension = false;
                        link = "/~/media" + GetMediaPath(parsed.ItemId, true, ref includesExtension);
                    }
                    else
                    {
                        using (var db = new SitecoreService(Constants.MasterDb))
                        {
                            var articleItem = db.GetItem<ArticleItem>(item.ID.Guid);
                            if (articleItem._TemplateId == IArticleConstants.TemplateId.Guid)
                            {
                                link = string.Format("{1}/a/{0}", articleItem.Article_Number, AssetLinkBase.BaseUrl);
                            }
                            else
                            {
                                link = LinkManager.GetItemUrl(item).ToLower();

                                const string partToRemove = "/sitecore/shell";
                                int indexOfPartToRemove = link.IndexOf(partToRemove);
                                if (indexOfPartToRemove != -1)
                                {
                                    link = link.Substring(partToRemove.Length + indexOfPartToRemove);
                                }
                            }
                        }
                    }
                }

                resolvedLink = link;
                return true;
            }

            resolvedLink = null;
            return false;
        }

        private static string GetMediaPath(ID itemId, bool includeExtension, ref bool includedExtension)
        {
            includedExtension = false;
            Database database = Factory.GetDatabase(Constants.MasterDb);
            if (database == null)
                return Sitecore.Configuration.Settings.LinkItemNotFoundUrl;
            MediaItem mediaItem = (MediaItem)ItemManager.GetItem(itemId, Language.Current, Sitecore.Data.Version.Latest, database, SecurityCheck.Disable);
            if (mediaItem == null)
                return Sitecore.Configuration.Settings.LinkItemNotFoundUrl;
            string mediaPath = mediaItem.MediaPath;
            if (!includeExtension)
                return mediaPath;
            string part2 = mediaItem.Extension.Length > 0 ? MediaManager.Config.GetUrlExtension(mediaItem.Extension) : (string)null;

            if (string.IsNullOrEmpty(part2) && !string.IsNullOrEmpty(mediaItem.Extension))
            {
                part2 = mediaItem.Extension;
            }

            if (string.IsNullOrEmpty(part2))
                return mediaPath;
            includedExtension = true;
            return FileUtil.MakePath(mediaPath, part2, '.');
        }
    }
}
