using Informa.Library.Globalization;
using Informa.Library.Presentation;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;

namespace Informa.Web.ViewModels
{
	public class TopicContainerViewModel : GlassViewModel<ITopic>
	{
		protected readonly IRenderingContext RenderingContext;
		protected readonly ITextTranslator TextTranslator;

		public TopicContainerViewModel(
			IRenderingContext renderingContext,
			ITextTranslator textTranslator)
		{
			RenderingContext = renderingContext;
			TextTranslator = textTranslator;
		}

		public Guid Id => RenderingContext.Id;
		public string Title => GlassModel?.Title;
		public string NavigationText => GlassModel?.Navigation_Text;
		public ILinkable Link => new LinkableModel
		{
			LinkableText = TextTranslator.Translate("Topic.Explore"),
			LinkableUrl = GlassModel?.Navigation_Link?.Url
		};
	}
}