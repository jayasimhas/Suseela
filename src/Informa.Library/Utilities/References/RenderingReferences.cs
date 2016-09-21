using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RenderingReferences : IRenderingReferences
	{
		public static IRenderingReferences Instance
		{
			get { return AutofacConfig.ServiceLocator.Resolve<Owned<IRenderingReferences>>().Value; }
		}

		public Guid ListableContentSmall => new Guid("{9FEFE78D-9024-40A8-AC16-16260F4D70D7}"); //not added to config, since this guid missing CMS
		public Guid LoginPopout => new Guid(ItemIdResolver.GetItemIdByKey("LoginRendering"));
		public Guid RegisterPopout => new Guid(ItemIdResolver.GetItemIdByKey("RegistrationRendering"));
		public Guid TopicAlertButton => new Guid(ItemIdResolver.GetItemIdByKey("TopicAlertButtonRendering"));
		public Guid LogoutMessage => new Guid(ItemIdResolver.GetItemIdByKey("LogoutRendering"));
		public Guid RenderingMVC => new Guid(ItemIdResolver.GetItemIdByKey("AccountLoginRendering"));        
        public Guid MediaTypeIcon => new Guid(ItemIdResolver.GetItemIdByKey("MediaTypeIconRendering"));
	}
}
