using Informa.Library.Globalization;
using Informa.Library.User.Content;
using Informa.Library.User.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels
{
	public class TopicAlertButtonModel : GlassViewModel<ITopic>
	{
		public TopicAlertButtonModel(ITopic model, IRenderingContextService renderingService, IUserContentService<ISavedSearchSaveable, ISavedSearchDisplayable> savedSearchService, ITextTranslator textTranslator)
		{
			RenderingParameters = renderingService.GetCurrentRenderingParameters<ITopic_Alert_Options>();

			SetAlertText = textTranslator.Translate("Topic.SetAlertText");
			DisplayButton = model != null && (!string.IsNullOrEmpty(RenderingParameters?.Related_Search) || Sitecore.Context.PageMode.IsExperienceEditor);
			if (DisplayButton)
			{
				AlertIsSet = savedSearchService.Exists(new SavedSearchDisplayModel
				{
					Url = RenderingParameters?.Related_Search ?? string.Empty
				});
				TopicName = model?.Title;
				AlertTitle = !string.IsNullOrEmpty(RenderingParameters?.Search_Name) ? RenderingParameters.Search_Name : TopicName;
				AlertUrl = RenderingParameters?.Related_Search;
			}
		}
		public ITopic_Alert_Options RenderingParameters { get; set; }
		public bool DisplayButton { get; set; }
		public bool AlertIsSet { get; set; }
		public string SetAlertText { get; set; }
		public string TopicName { get; set; }
		public string AlertTitle { get; set; }
		public string AlertUrl { get; set; }
	}
}