using Informa.Library.Globalization;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
	public class TopicContainerViewModel : GlassViewModel<ITopic>
	{
		protected readonly ITextTranslator TextTranslator;

		public TopicContainerViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string Title => GlassModel?.Title;
		public ILinkable Link => new LinkableModel
		{
			LinkableText = TextTranslator.Translate("Topic.Explore"),
			LinkableUrl = GlassModel?.Navigation_Link?.Url
		};
	}
}