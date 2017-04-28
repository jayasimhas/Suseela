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
using System.Configuration;
using Sitecore.Workflows;
using Sitecore;
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using Glass.Mapper.Sc;

namespace Informa.Library.CustomSitecore.Pipelines.Article
{
    public class SaveTitle
    {
        /// <summary>
        /// Updates the following fields based on conditions - planned publish date, title field, workflow state field, referenced articles treelist field based on the references provided in body.
        /// </summary>
        /// <param name="args"></param>
        public void Process(SaveArgs args)
        {
            foreach (var saveItem in args.Items) //loop through item(s) being saved
            {
                var item = Sitecore.Context.ContentDatabase.GetItem(saveItem.ID, saveItem.Language, saveItem.Version); //get the actual item being saved
                if (item == null)
                    continue;
                if (string.Equals(item.TemplateID.ToString(), IArticleConstants.TemplateId.ToString(), StringComparison.OrdinalIgnoreCase) && !item.Name.Equals("__Standard Values"))
                {
                    try
                    {
                        var allreferencedArticlesIds = GetReferencedArticleFromBody(item);
                        if (allreferencedArticlesIds.Count() > 0)
                        {
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

                                }
                            }
                        }
                        //item name and title sync
                        item.Fields.ReadAll();
                        var field = item.Fields[IArticleConstants.TitleFieldId];

                        #region temporary fix for $name issue mostly happening when workflow is being changed.
                        var navigationTitle = item.Fields[IArticleConstants.Navigation_TitleFieldId];
                        if (navigationTitle != null && !string.IsNullOrEmpty(navigationTitle.Value))
                        {
                            if (string.Equals(navigationTitle.Value, "$name", StringComparison.OrdinalIgnoreCase))
                            {
                                using (new SecurityDisabler())
                                {
                                    item.Editing.BeginEdit();
                                    navigationTitle.Value = item.Name;
                                }
                            }
                        }
                        #endregion temporary fix for $name issue mostly happening when workflow is being changed.

                        if (field != null && !string.IsNullOrEmpty(field.Value))
                        {
                            //temporary fix for $name issue mostly happening when workflow is being changed.
                            if (string.Equals(field.Value, "$name", StringComparison.OrdinalIgnoreCase))
                            {
                                using (new SecurityDisabler())
                                {
                                    item.Editing.BeginEdit();
                                    field.Value = item.Name;
                                }
                            }

                            if (!string.Equals(item.Name, field.Value, StringComparison.OrdinalIgnoreCase))
                            {
                                using (new SecurityDisabler())
                                {
                                    item.Editing.BeginEdit();
                                    item.Name = ItemUtil.ProposeValidItemName(field.Value.Length > 100 ? new string(field.Value.Take(100).ToArray()) : field.Value);
                                }
                            }
                        }

                        //update planned publish date on save as part of article publish scheduler changes.
                        var plannedPubDate = ((DateField)item.Fields[IArticleConstants.Planned_Publish_DateFieldName]).DateTime;
                        if (plannedPubDate == default(DateTime) || plannedPubDate == null || (plannedPubDate <= DateTime.UtcNow && plannedPubDate.TimeOfDay.TotalHours <= DateTime.UtcNow.TimeOfDay.TotalHours && plannedPubDate.TimeOfDay.TotalMinutes <= DateTime.UtcNow.TimeOfDay.TotalMinutes))
                        {
                            using (new SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                item[IArticleConstants.Planned_Publish_DateFieldName] = DateUtil.ToIsoDate(DateTime.UtcNow);
                            }
                        }
                        //update workflow to "edit after publish" if its in "ready for production" state as part article publish scheduler changes.
                        var wfState = item.State.GetWorkflowState();
                        var editAfterPublishState = getEditAfterPublishState(item);
                        if (wfState.StateID != editAfterPublishState.StateID && wfState.FinalState)
                        {
                            item[FieldIDs.WorkflowState] = editAfterPublishState.StateID.ToString();
                        }
                    }
                    catch
                    {
                        Sitecore.Diagnostics.Log.Error("Error updating the fields from savetitle", item.ID);
                    }
                    finally
                    {
                        item.Editing.EndEdit();
                    }
                }
            }
        }
        /// <summary>
        /// Gets the referenced articles in the format [A#{ArticleNumber}], search using article number and gets the GUID of all referenced articles.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
                    string hostName = Factory.GetSiteInfo("website")?.HostName ?? WebUtil.GetHostName();
                    url = string.Format("{0}://{1}/api/SearchArticlesInRTE?articleNumber={2}", HttpContext.Current.Request.Url.Scheme, hostName, articleNumber);
                }
                try
                {
                    using (var client = new HttpClient())
                    {
                        string disableSSLCertificateValidation = ConfigurationManager.AppSettings["DisableSSLCertificateValidation"];
                        if (!string.IsNullOrWhiteSpace(disableSSLCertificateValidation) && System.Convert.ToBoolean(disableSSLCertificateValidation))
                        {
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        }
                        var response = client.GetStringAsync(url).Result;
                        if (!string.IsNullOrEmpty(response))
                        {
                            var resultarticles = JsonConvert.DeserializeObject<ArticleItem>(response);
                            if (resultarticles != null)
                            {
                                referencedArticlesInBody.Add(resultarticles._Id.ToString().ToUpper());
                            }
                        }

                    }
                }
                catch
                {
                    Sitecore.Diagnostics.Log.Error("Error searching the referenced article from body in savetitle class", item.ID);
                }
            }
            return referencedArticlesInBody;
        }

        /// <summary>
        /// Get the "Edited after publish" workflow state based on the field "Is edited after publish".
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private WorkflowState getEditAfterPublishState(Item item)
        {
            try
            {
                var workflow = item.Database.WorkflowProvider.GetWorkflow(item);
                var states = workflow.GetStates();
                if (states.Count() > 0)
                {
                    foreach (var state in states)
                    {
                        var stateID = state.StateID;
                        var istate = Sitecore.Context.ContentDatabase.GetItem(stateID);
                        if (istate.Fields[IStateConstants.Is_Edit_After_PublishFieldName].Value == "1")
                        {
                            return state;
                        }
                    }
                }
            }
            catch
            {
                Sitecore.Diagnostics.Log.Error("Error occurred while getting the workflow states from savetitle class", item.ID);
            }
            return null;
        }

    }
}
