using Informa.Models.FactoryInterface;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels
{
	public class TopicContainerViewModel : GlassViewModel<IGlassBase>
	{
		public TopicContainerViewModel(
			)
		{

		}

		public string Title => TestData.TestData.GetRandomTopic().LinkableText;
		public ILinkable Link => new LinkableModel
		{
			LinkableText = "Explore this Topic",
			LinkableUrl = "#"
		};
	}
}