using Informa.Library.Globalization;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class TopicJumpToViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly ITextTranslator TextTranslator;

		public TopicJumpToViewModel(
			ITextTranslator textTranslator)
		{
			TextTranslator = textTranslator;
		}

		public string Text => TextTranslator.Translate("Topic.JumpToText");
	}
}