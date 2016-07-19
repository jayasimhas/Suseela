using Informa.Library.Authors;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Content;
using Informa.Library.User.Search;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using System;
using System.Web;
using System.Web.Mvc;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.ViewModels
{
	public class TopicAlertButtonModel : GlassViewModel<IGlassBase>
	{
        public TopicAlertButtonModel(
            IGlassBase model, 
            IRenderingContextService renderingService, 
            IAuthenticatedUserContext userContext, 
            IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> savedSearchService, 
            ITextTranslator textTranslator)
		{
			var parameters = renderingService.GetCurrentRenderingParameters<ITopic_Alert_Options>();
            if (IsAuthorPage(model))
                parameters = SetupAuthorParameters(parameters);

			IsAuthenticated = userContext.IsAuthenticated;
			SetAlertText = textTranslator.Translate(DictionaryKeys.TopicSetAlertText);
			RemoveAlertText = textTranslator.Translate(DictionaryKeys.TopicRemoveAlertText);
			DisplayButton = model != null && (!string.IsNullOrEmpty(parameters?.Related_Search) || Sitecore.Context.PageMode.IsExperienceEditor);
			if (DisplayButton)
			{
				AlertIsSet = savedSearchService.Exists(new SavedSearchDisplayModel
				{
					Url = parameters?.Related_Search ?? string.Empty,
					AlertEnabled = true
				});
				TopicName = GetTopicTitle(model);
				AlertTitle = !string.IsNullOrEmpty(parameters?.Search_Name) ? parameters.Search_Name : TopicName;
				AlertUrl = parameters?.Related_Search;
			}
		}

        private bool IsAuthorPage(IGlassBase page) => (page != null) ? page._TemplateId.ToString() == IAuthor_PageConstants.TemplateIdString : false;
        private ITopic_Alert_Options SetupAuthorParameters(ITopic_Alert_Options options) {
            var authorOptions = DependencyResolver.Current.GetService<IAuthorAlertOptions>();
            return (authorOptions != null)
                ? authorOptions.SetOptions(options)
                : options;
        }

        public bool IsAuthenticated { get; set; }
		public bool DisplayButton { get; set; }
		public bool AlertIsSet { get; set; }
		public string SetAlertText { get; set; }
		public string RemoveAlertText { get; set; }
		public string TopicName { get; set; }
		public string AlertTitle { get; set; }
		public string AlertUrl { get; set; }

		private string GetTopicTitle(IGlassBase model)
		{
			var topic = model as ITopic;
			if (topic != null)
			{
				return topic.Title;
			}

			var topicPage = model as ITopic_Page;
			if (topicPage != null)
			{
				return topicPage.Title;
			}

			return model._Name;
		}
	}
}