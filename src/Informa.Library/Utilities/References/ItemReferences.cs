using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;

namespace Informa.Library.Utilities.References
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class ItemReferences : IItemReferences
	{
		public static IItemReferences Instance => AutofacConfig.ServiceLocator.Resolve<Owned<IItemReferences>>().Value;

		public Guid HomePage => new Guid("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}");
        
		public Guid NlmConfiguration => new Guid("{B0C03A57-0C1E-4BC9-BE7A-5871695FD79B}");
		public Guid NlmErrorDistributionList => new Guid("{54C6F361-9A73-453F-89E4-6230090C122A}");

		#region Pharma Globals

		public Guid NlmCopyrightStatement => new Guid("{EE38E489-23F7-4637-A152-3FDC696FAACF}");
		public Guid InformaBar => new Guid("{F3082926-B856-4B48-8DD8-5D55573FE4F6}");

		public Guid GeneratedDictionary => new Guid("{1B81B972-B282-46F0-89DF-6C1A25A68A92}");

        public Guid DownloadTypes => new Guid("{3939BE43-6300-4AFA-ABE6-F7798C16E34D}");
        #endregion

        #region Taxonomy Folders

        public Guid SubjectsTaxonomyFolder => new Guid("{46D8B99F-4A19-4D67-A083-0EFE313154AC}");
		public Guid RegionsTaxonomyFolder => new Guid("{5728D226-839C-44E3-B044-C88321A53421}");
		public Guid TherapyAreasTaxonomyFolder => new Guid("{49A93890-E459-44F1-9453-A6F3FF0AF4C1}");

		#endregion

		public Guid SearchPage => new Guid("{0FF66777-7EC7-40BE-ABC4-6A20C8ED1EF0}");
		public Guid VwbSearchPage => new Guid("{A0163A51-2FF8-4A9C-8FBA-6516546E5AE1}");

		public Guid SubscriptionPage => new Guid("{39611772-CD97-4610-BB55-F96BE4C1F540}");

		public Guid EmailPreferences => new Guid("{BAF1D4FB-7599-4EDA-8926-0A4995E4DC2D}");

		#region Account Contact Info Drop Downs

		public Guid AccountCountries => new Guid("{C1479FF7-F581-4A71-B25A-5FCB6312A0CF}");
		public Guid AccountJobFunctions => new Guid("{FC588B3B-499F-41F1-BB48-A362EA72FD0C}");
		public Guid AccountJobIndustries => new Guid("{88CC966F-877E-4B79-9C69-A5AF27CCA4DD}");
		public Guid AccountNameSuffixes => new Guid("{A2065C72-92F8-4F71-9913-11A7AE7E6D72}");
		public Guid AccountPhoneTypes => new Guid("{361E7AC5-BF4B-4FD2-A840-5BE7457BBDD5}");
		public Guid AccountSalutations => new Guid("{E7366564-0E17-43FF-8ECA-0BD9829392AC}");

		#endregion Account Contact Info Drop Downs

		#region Templates

		public Guid FolderTemplate => new Guid("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");
		public Guid TaxonomyRoot => new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}");



		#endregion
	}
}
