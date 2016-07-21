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
using Jabberwocky.Glass.Models;
using PluginModels;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows;
using Constants = Informa.Library.Utilities.References.Constants;
using Sitecore.Web.UI;

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
			if (articleStruct.Publication == Guid.NewGuid()) return;
			var siteConfigItem = _service.GetItem<ISite_Config>(articleStruct.Publication);
			if (siteConfigItem == null) return;
			var fromEmail = siteConfigItem.From_Email_Address;
			var title = siteConfigItem.Email_Title; 
			var replyToEmail = Sitecore.Context.User.Profile.Email;
			if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(replyToEmail)) return;
			var isAuthorInSenderList = false;

			var notificationList = !articleStruct.ArticleSpecificNotifications.Any() ? new List<StaffStruct>() :
				articleStruct.ArticleSpecificNotifications; 

			//IIPP-1092
			try
			{
				var stateItem =
					_service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.ICommand>(
						articleStruct.CommandID.ToString());
				if (stateItem != null)
				{
					var workflowitem =
						_service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.IState>(stateItem.Next_State);
					var workflowBasednotificationList = workflowitem.Staffs;
					foreach (var eachUser in workflowBasednotificationList)
					{
						try
						{
							var toSender = new StaffStruct { ID = eachUser._Id };
							notificationList.Add(toSender);
						}
						catch (Exception ex)
						{
							//TODO - Add logging
						}
					}

				}
			}
			catch (Exception ex)
			{
				//TODO Logging - Failed to find the workflow item.
			}

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
				if (replyToEmail == notificationUser.Email_Address)
				{
					isAuthorInSenderList = true;
				}
			}

			if (isAuthorInSenderList) return;
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

		/// <summary>
		/// 
		/// </summary>
		public void EditAfterPublishSendNotification(ArticleStruct articleStruct)
		{
			if (articleStruct.Publication == Guid.NewGuid()) return;
			var siteConfigItem = _service.GetItem<ISite_Config>(articleStruct.Publication);
			if (siteConfigItem == null) return;
			var fromEmail = siteConfigItem.From_Email_Address;
			var title = siteConfigItem.Email_Title;
			var replyToEmail = Sitecore.Context.User.Profile.Email;
			if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(replyToEmail)) return;

			var workflowItem =
				_service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.IWorkflow>(
					Constants.ScripWorkflow);
			if (workflowItem == null) return;
			var notificationList = workflowItem.Notified_After_Publishes;
			var staffItems = notificationList as IStaff_Item[] ?? notificationList.ToArray();
			if (notificationList == null || !staffItems.Any()) return;
			var emailBody = CreateEditAfterPublishBody(articleStruct, title, siteConfigItem.Publication_Name);

			foreach (var eachEmail in staffItems)
			{
				if (string.IsNullOrEmpty(eachEmail.Email_Address)) continue;
				var email = new Email
				{
					To = eachEmail.Email_Address,
					Subject = title,
					From = fromEmail,
					Body = emailBody,
					IsBodyHtml = true
				};
				EmailSender.SendWorkflowNotification(email, replyToEmail);
			}
		}
		public string CreateBody(ArticleStruct articleStruct, string emailTitle, string publication, WorkflowInfo oldWorkflow)
		{
			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("Workflow");
			string siteRoot = HttpContext.Current.Request.Url.Host;
			if (htmlEmailTemplate == null)
			{
				return null;
			}

			var article = _articleUtil.GetArticleItemByNumber(articleStruct.ArticleNumber);

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

			ArticleItem articleItem = _service.GetItem<ArticleItem>(article.ID.ToString());
			if (articleItem != null)
			{
				var preview = _articleUtil.GetPreviewInfo(articleItem);
				if (preview != null)
				{
					replacements["#preview_url#"] = preview.PreviewUrl;
				}
			}

			replacements["#Authors#"] = string.IsNullOrEmpty(authorString) ? "No authors selected" : authorString;
			replacements["#Publication#"] = publication;
			replacements["#Body_Content#"] = articleStruct.NotificationText;
			replacements["#content_editor#"] = Sitecore.Context.User.Profile.FullName;
			replacements["#current_time#"] = DateTime.Now.ToString();

			var oldState = _service.Database.WorkflowProvider.GetWorkflow(oldWorkflow.StateID);
			if (oldState != null)
			{
                var oldiconUrl = Sitecore.Resources.Images.GetThemedImageSource(oldState.Appearance.Icon, ImageDimension.id24x24);
                replacements["#old_state_image#"] = "http://" + siteRoot + oldiconUrl;
                replacements["#old_state#"] = oldState.Appearance.DisplayName;
			}
			var newState = _service.Database.WorkflowProvider.GetWorkflow(articleStruct.CommandID.ToString());
			if (newState != null)
			{
                var newiconUrl = Sitecore.Resources.Images.GetThemedImageSource(newState.Appearance.Icon, ImageDimension.id24x24);
                replacements["#new_state_image#"] = "http://" + siteRoot + newiconUrl;
                replacements["#new_state#"] = newState.Appearance.DisplayName;
			}


			List<WorkflowEvent> workflowHistory = GetWorkflowHistory(article);
			replacements["#history#"] = HistoryTableCreation(workflowHistory);

			return emailHtml.ReplacePatternCaseInsensitive(replacements);
		}

		public string CreateEditAfterPublishBody(ArticleStruct articleStruct, string emailTitle, string publication)
		{
			var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("EditAfterPublishEmail");
			if (htmlEmailTemplate == null)
			{
				return null;
			}

			var article = _articleUtil.GetArticleItemByNumber(articleStruct.ArticleNumber);

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

			ArticleItem articleItem = _service.GetItem<ArticleItem>(article.ID.ToString());
			if (articleItem != null)
			{
				var preview = _articleUtil.GetPreviewInfo(articleItem);
				if (preview != null)
				{
					replacements["#preview_url#"] = preview.PreviewUrl;
				}
			}

			replacements["#Authors#"] = string.IsNullOrEmpty(authorString) ? "No authors selected" : authorString;
			replacements["#Publication#"] = publication;
			replacements["#Body_Content#"] = articleStruct.NotificationText;
			replacements["#content_editor#"] = Sitecore.Context.User.Profile.FullName;
			replacements["#current_time#"] = DateTime.Now.ToString();

			List<WorkflowEvent> workflowHistory = GetWorkflowHistory(article);
			replacements["#history#"] = HistoryTableCreation(workflowHistory);

			return emailHtml.ReplacePatternCaseInsensitive(replacements);
		}

		public string GetWordURL(ArticleStruct articleStruct)
		{
			string url = string.Empty;
			var article = _articleUtil.GetArticleItemByNumber(articleStruct.ArticleNumber);
			//Item article = _service.GetItem<Item>(articleStruct.ArticleGuid);
			LinkField wordDocument = article.Fields[IArticleConstants.Word_DocumentFieldId];
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
            string siteRoot = HttpContext.Current.Request.Url.Host;
            StringBuilder outputString = new StringBuilder();
            if (!history.Any()) return outputString.ToString();
            outputString.Append("<u>Prior Workflow History:</u>");
            outputString.Append("<table style='border: 1px solid black;border-collapse: collapse;'><th style='border: 1px solid black'></th><th style='border: 1px solid black'>To State</th><th style='border: 1px solid black'>By...</th><th style='border: 1px solid black'>At...</th>");
            foreach (var eachWorkflow in history)
            {
                var state = _service.Database.WorkflowProvider.GetWorkflow(eachWorkflow.NewState);
                var iconUrl = Sitecore.Resources.Images.GetThemedImageSource(state.Appearance.Icon, ImageDimension.id24x24);

                outputString.Append("<tr><td style='border: 1px solid black'>" + "<img src='http://" + siteRoot + iconUrl + "'/> </td>" + 
                    "<td style='border: 1px solid black'>" + state.Appearance.DisplayName + "</td>" +
                    
                                    eachWorkflow.User + "</td><td style='border: 1px solid black'>" + eachWorkflow.Date.ToString(CultureInfo.InvariantCulture));
                outputString.Append("</tr>");
            }
            outputString.Append("</table>");

            return outputString.ToString();
        }
	}
}
