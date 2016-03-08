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
		public static IItemReferences Instance => AutofacConfig.ServiceLocator.Resolve<Owned<IItemReferences>>().Value;

	    public Guid HomePage => new Guid("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}");

        public Guid DCDConfigurationItem => new Guid("{}");

        public Guid SiteConfig  => new Guid("{BE2B8891-635F-49C1-8BA9-4D2F6C7C5ACE}");

        public Guid SearchPage  => new Guid("{0FF66777-7EC7-40BE-ABC4-6A20C8ED1EF0}");
        public Guid VwbSearchPage  => new Guid("{5B5DCF96-98F2-4CDC-9A5F-75F3E0CE6F52}");

        #region Templates

        public Guid FolderTemplate => new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

	    public Guid TaxonomyRoot => new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}");

	    #endregion
	}
}
