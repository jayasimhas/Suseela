using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RenderingReferences : IRenderingReferences
	{
		public static IRenderingReferences Instance
		{
			get { return AutofacConfig.ServiceLocator.Resolve<Owned<IRenderingReferences>>().Value; }
		}

		public Guid ListableContentSmall => new Guid("{9FEFE78D-9024-40A8-AC16-16260F4D70D7}");
		public Guid LoginPopout => new Guid("{BD6A0FD0-1B33-4410-A044-B6BE00E342C3}");
		public Guid RegisterPopout => new Guid("{7A0B4F46-A4A9-4A2B-A45E-5AF282A4BE3F}");
		public Guid TopicAlertButton => new Guid("{39C8CD59-AFEA-4B6C-8071-FF799AD45596}");

		public Guid LogoutMessage => new Guid("{1460F117-C1C2-45EF-884A-9520FB3A7FDB}");
		public Guid RenderingMVC => new Guid("{3A09B629-FF07-4B1A-B1D4-748E712C18CA}");
		//public Guid UtilityNavigationRendering => new Guid("{D0BBF308-5AF1-46AF-B74D-C89E95D18635}");
		//public Guid GlobalFooterRendering => new Guid("{FC5BDC0B-66A1-40C5-A6E7-F615B3318585}");
		//public Guid MakeAnAppointmentRendering => new Guid("{4CB233D5-1AEB-4261-8BCA-65D53F035047}");
		//public Guid SharebarMetaTagsRendering => new Guid("{0038CCCF-A666-4A6D-8F73-E0B99B324EEC}");


	}
}
