using Informa.Library.Article.Search;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Save;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Informa.Models;
using Informa.Library.Utilities.TokenMatcher;
using System.Net.Http;
using Informa.Library.Utilities.References;
using System.Net;
using Sitecore.Configuration;
using Sitecore.Web;
using Newtonsoft.Json;
using Informa.Library.Rss;

namespace Informa.Library.CustomSitecore.Pipelines.Article
{
    public class SaveTitle
    {
        public void Process(SaveArgs args)
        {
            foreach (var saveItem in args.Items) //loop through item(s) being saved
            {
                var item = Sitecore.Context.ContentDatabase.GetItem(saveItem.ID, saveItem.Language, saveItem.Version); //get the actual item being saved
                if (item == null)
                    continue;
                if (string.Equals(item.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var allreferencedArticlesIds = GetReferencedArticleFromBody(item);
                    if (allreferencedArticlesIds.Count() > 0)
                    {
                        //var updatedBody = UpdateBody(item);
                        string updatedValue = string.Empty;
                        var referencedArticles = item.Fields[IArticleConstants.Referenced_ArticlesFieldName];
                        if (referencedArticles != null)
                        {
                            using (new SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                foreach (var id in allreferencedArticlesIds)
                                {
                                    if (!referencedArticles.Value.Contains(id))
                                    {
                                        if (!string.IsNullOrEmpty(referencedArticles.Value))
                                        {
                                            updatedValue = referencedArticles.Value;
                                            updatedValue += "|" + id;
                                        }
                                        else
                                            updatedValue = id;
                                        if (!string.IsNullOrEmpty(updatedValue))
                                        {
                                            item.Fields[IArticleConstants.Referenced_ArticlesFieldName].Value = updatedValue;
                                        }
                                    }
                                }
                                //item.Fields[IArticleConstants.BodyFieldName].Value = updatedBody;
                                item.Editing.EndEdit();
                            }
                        }
                    }

                    item.Fields.ReadAll();
                    var field = item.Fields[IArticleConstants.TitleFieldId];
                    if (field != null && !string.IsNullOrEmpty(field.Value))
                    {
                        if (!string.Equals(item.Name, field.Value, StringComparison.OrdinalIgnoreCase))
                        {
                            using (new SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                item.Name = ItemUtil.ProposeValidItemName(field.Value.Length > 100 ? new string(field.Value.Take(100).ToArray()) : field.Value);
                                item.Editing.EndEdit();
                            }
                        }
                    }
                }
            }
        }

        public List<string> GetReferencedArticleFromBody(Item item)
        {
            List<string> referencedPlaceholder = new List<string>();
            var body = item.Fields[IArticleConstants.BodyFieldName].Value;
            var referenceArticleTokenRegex = new Regex(@"\[A#(.*?)\]");
            List<string> referencedArticlesInBody = new List<string>();

            foreach (Match match in referenceArticleTokenRegex.Matches(body))
            {
                string articleNumber = match.Groups[1].Value;
                string url;
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    //string searchPageId = new ItemReferences().VwbSearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
                    string hostName = Factory.GetSiteInfo("website")?.HostName ?? WebUtil.GetHostName();
                    url = string.Format("{0}://{1}/api/SearchArticlesInRTE?articleNumber={2}", HttpContext.Current.Request.Url.Scheme, hostName, articleNumber);
                }
                using (var client = new HttpClient())
                {
                    var response = client.GetStringAsync(url).Result;
                    if (!string.IsNullOrEmpty(response))
                    {
                        var resultarticles = JsonConvert.DeserializeObject<ArticleItem>(response);
                        if (resultarticles != null)
                        {
                            referencedArticlesInBody.Add(resultarticles._Id.ToString().ToUpper());
                        }
                        //var resultarticles = JsonConvert.DeserializeObject<SearchResults>(response);
                        //if (resultarticles.results.Any())
                        //{
                        //    var id = resultarticles.results.FirstOrDefault().ItemId;
                        //    return id.ToString().ToUpper();
                        //}
                    }
                    //return response.IsSuccessStatusCode;
                }
            }
            return referencedArticlesInBody;
        }

        public string UpdateBody(Item item)
        {
            var body = item.Fields[IArticleConstants.BodyFieldName].Value;
            var referenceArticleTokenRegex = new Regex(@"L\[A#");

            foreach (Match match in referenceArticleTokenRegex.Matches(body))
            {
                body = body.Replace(match.ToString(), "[A#");
            }
            return body;
        }
    }
}
