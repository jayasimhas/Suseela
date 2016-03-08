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
	    public Guid NlmConfiguration => new Guid("{B0C03A57-0C1E-4BC9-BE7A-5871695FD79B}");

	    #region Taxonomy Folders

	    public Guid SubjectsTaxonomyFolder => new Guid("{46D8B99F-4A19-4D67-A083-0EFE313154AC}");
	    public Guid RegionsTaxonomyFolder => new Guid("{5728D226-839C-44E3-B044-C88321A53421}");
        public Guid TherapyAreasTaxonomyFolder => new Guid("{49A93890-E459-44F1-9453-A6F3FF0AF4C1}");

        #endregion


        #region Templates

        public Guid FolderTemplate => new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

	    public Guid TaxonomyRoot => new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}");

	    #endregion
	}
}
