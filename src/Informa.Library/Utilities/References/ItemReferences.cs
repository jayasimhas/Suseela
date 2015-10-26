using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemReferences : IItemReferences
	{
		public static IItemReferences Instance
		{
			get { return AutofacConfig.ServiceLocator.Resolve<Owned<IItemReferences>>().Value; }
		}
		
		public Guid HomePage
		{
			get { return new Guid("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"); }
		}
		

		#region Templates

		public Guid FolderTemplate
		{
			get { return new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}"); }
		}

		#endregion
	}
}
