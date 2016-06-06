using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.SRA
{
	public class FromSectionToRegion : InformaListToRegions
	{
		public FromSectionToRegion(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("EUROPE", "Europe");
			d.Add("ASIA", "Asia");
			d.Add("NORTH AMERICA", "Americas");
			d.Add("LATIN AMERICA", "Americas");
			
			return d;
		}
	}
}