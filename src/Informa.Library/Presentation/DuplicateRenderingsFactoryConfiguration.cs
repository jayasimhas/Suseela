using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore.Configuration;
using System;

namespace Informa.Library.Presentation
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class DuplicateRenderingsFactoryConfiguration : IDuplicateRenderingsFactoryConfiguration
	{
		private const string ContentCurationItemIdConfigKey = "DuplicateRenderingsFactoryConfiguration.ContentCurationItemId";

		public Guid ContentCurationItemId => new Guid(Settings.GetSetting(ContentCurationItemIdConfigKey));
	}
}
