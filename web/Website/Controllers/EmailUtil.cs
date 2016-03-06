using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Mail;
using Informa.Library.User.ResetPassword;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using PluginModels;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows;
using Constants = Informa.Library.Utilities.References.Constants;

namespace Informa.Web.Controllers
{
	public class EmailUtil
	{
		private ISitecoreService _service;
		private ArticleUtil _articleUtil;
		protected readonly IEmailSender EmailSender;
		protected readonly IHtmlEmailTemplateFactory HtmlEmailTemplateFactory;

		public EmailUtil(ArticleUtil articleUtil, Func<string, ISitecoreService> sitecoreFactory, IEmailSender emailSender, IHtmlEmailTemplateFactory htmlEmailTemplateFactory)
		{
			EmailSender = emailSender;
			_articleUtil = articleUtil;
			_service = sitecoreFactory(Constants.MasterDb);
			HtmlEmailTemplateFactory = htmlEmailTemplateFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		public void SendNotification(ArticleStruct articleStruct, WorkflowInfo oldWorkflow)
		{
			var siteConfigItem = _service.GetItem<ISite_Config>(Constants.ScripRootNode);
			if (siteConfigItem == null) return;
			var fromEmail = siteConfigItem.From_Email_Address;
			var title = siteConfigItem.Email_Title;
			var replyToEmail = Sitecore.Context.User.Profile.Email;
			if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(replyToEmail)) return;

			if (!articleStruct.ArticleSpecificNotifications.Any()) return;
			var notificationList = articleStruct.ArticleSpecificNotifications;
			var emailBody = CreateBody(articleStruct, title, siteConfigItem.Publication_Name, oldWorkflow);

			foreach (var eachEmail in notificationList)
			{
				var notificationUser = _service.GetItem<IStaff_Item>(eachEmail.ID);
				if (string.IsNullOrEmpty(notificationUser?.Email_Address)) continue;
				Email email = new Email
				{
					To = notificationUser.Email_Address,
					Subject = title,
					From = fromEmail,
					Body = emailBody,
					IsBodyHtml = true
				};
				EmailSender.SendWorkflowNotification(email, replyToEmail);
			}

			//Emailing the Content Author
			Email contentAuthorEmail = new Email
			{
				To = replyToEmail,
				Subject = title,
				From = fromEmail,
				Body = emailBody,
				IsBodyHtml = true
			};
			EmailSender.SendWorkflowNotification(contentAuthorEmail, replyToEmail);
		}

		public string CreateBody(ArticleStruct articleStruct, string emailTitle, string publication, WorkflowInfo oldWorkflow)
		{
			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("Workflow");
			string siteRoot = HttpContext.Current.Request.Url.Host;
			if (htmlEmailTemplate == null)
			{
				return null;
			}

			var authorString = string.Empty;
			foreach (var eachAuthor in articleStruct.Authors)
			{
				authorString = eachAuthor.Name + ",";
			}
			var emailHtml = htmlEmailTemplate.Html;
			var replacements = new Dictionary<string, string>();
			replacements["#Email_Title#"] = emailTitle;
			replacements["#Article_Title#"] = articleStruct.Title;
			replacements["#Publish_Date#"] = articleStruct.WebPublicationDate.ToString();
			replacements["#word_url#"] = GetWordURL(articleStruct);
			replacements["#Authors#"] = string.IsNullOrEmpty(authorString) ? "No authors selected" : authorString;
			replacements["#Publication#"] = publication;
			replacements["#Body_Content#"] = articleStruct.NotificationText;
			replacements["#content_editor#"] = Sitecore.Context.User.Profile.FullName;
			replacements["#current_time#"] = DateTime.Now.ToString();
			//old_state_image
			//	old_state_image
			var oldState = _service.Database.WorkflowProvider.GetWorkflow(oldWorkflow.StateID);
			if (oldState != null)
			{
				replacements["#old_state_image#"] = "https://" + siteRoot + oldState.Appearance.Icon;
				replacements["#old_state#"] = oldState.Appearance.DisplayName;
			}
			var newState = _service.Database.WorkflowProvider.GetWorkflow(articleStruct.CommandID.ToString());
			if (newState != null)
			{
				replacements["#new_state_image#"] = "https://" + siteRoot + newState.Appearance.Icon;
				replacements["#new_state#"] = newState.Appearance.DisplayName;
			}

			var article = _articleUtil.GetArticleItemByNumber(articleStruct.ArticleNumber);
			List<WorkflowEvent> workflowHistory = GetWorkflowHistory(article);
			replacements["#history#"] = HistoryTableCreation(workflowHistory);

			return emailHtml.ReplaceCaseInsensitive(replacements);
		}

		public string GetWordURL(ArticleStruct articleStruct)
		{
			string url = string.Empty;
			var article = _articleUtil.GetArticleItemByNumber(articleStruct.ArticleNumber);
			//Item article = _service.GetItem<Item>(articleStruct.ArticleGuid);
			LinkField wordDocument = article.Fields[IArticleConstants.Word_DocumentFieldName];
			if (wordDocument == null) return url;
			Item item = _service.GetItem<Item>(wordDocument.TargetID.Guid);
			if (item == null || !MediaManager.HasMediaContent(item)) return url;
			string siteRoot = HttpContext.Current.Request.Url.Host;
			url = siteRoot + MediaManager.GetMediaUrl(item) + "?sc_mode=preview";
			return url;
		}

		/// <summary>
		/// Gets the workflow history for the current item
		/// </summary>
		/// <returns>A list of all workflow events.</returns>
		/// <remarks>Because of versioning, there are duplicate item created workflow events. This method filters
		/// those duplicates out.</remarks>
		public List<WorkflowEvent> GetWorkflowHistory(Item currentItem)
		{
			var completeWorkflowHistory = new List<WorkflowEvent>();
			try
			{
				bool addedFirstEvent = false;
				// versions are in a 1-based array; if you give it "0", it will give you the most recent.
				for (int i = 1; i < currentItem.Versions.Count + 1; i++)
				{
					Item thisVersion = currentItem.Versions[new Sitecore.Data.Version(i)];
					IWorkflow workflow = _service.Database.WorkflowProvider.GetWorkflow(thisVersion[FieldIDs.Workflow]);

					if (workflow != null)
					{
						List<WorkflowEvent> events = workflow.GetHistory(thisVersion).ToList();

						if (addedFirstEvent)
						{
							WorkflowState firstState = workflow.GetStates()[0];
							events.RemoveAll(e => e.OldState == "" && e.NewState == firstState.StateID);
							addedFirstEvent = true;
						}
						addedFirstEvent = true;

						completeWorkflowHistory.AddRange(events);
					}
				}
			}
			catch (Exception exception)
			{
				throw;
			}
			return completeWorkflowHistory;
		}

		public string HistoryTableCreation(List<WorkflowEvent> history)
		{
			StringBuilder outputString = new StringBuilder();
			if (!history.Any()) return outputString.ToString();
			outputString.Append("<table><th>To State</th><th>By...</th><th>At...</th>");
			foreach (var eachWorkflow in history)
			{
				var state = _service.Database.WorkflowProvider.GetWorkflow(eachWorkflow.NewState);
				outputString.Append("<tr><td>" + state.Appearance.DisplayName + "</td><td>" +
									eachWorkflow.User + "</td><td>" + eachWorkflow.Date.ToString(CultureInfo.InvariantCulture));
				outputString.Append("</tr>");
			}
			outputString.Append("</table>");

			return outputString.ToString();
		}
	}
}
