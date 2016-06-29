using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromSectionToRegion : InformaListToRegions
	{
		public FromSectionToRegion(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			var d = base.GetMapping();

			d.Add("BLOG OPEN SURGERY", "");
			d.Add("BUSINESS", "");
			d.Add("CARDIOVASCULAR", "");
			d.Add("CELLULARANDGENETIC", "");
			d.Add("DEALS", "");
			d.Add("ECE_INCOMING", "");
			d.Add("EVENTS", "");
			d.Add("FEATURES", "");
			d.Add("FINANCIALS", "");
			d.Add("FRONTPAGE", "");
			d.Add("FUNDING", "");
			d.Add("HOME", "");
			d.Add("HTA", "");
			d.Add("IMAGING&IT", "");
			d.Add("IMAGINGANDIT", "");
			d.Add("IVDS", "");
			d.Add("LEGAL", "");
			d.Add("LIVE", "");
			d.Add("MANDA", "");
			d.Add("MARKET MONITOR", "");
			d.Add("MARKET SECTOR", "");
			d.Add("MEDTECH VENTURES", "");
			d.Add("NEUROLOGICAL", "");
			d.Add("NEW ARTICLES", "");
			d.Add("NEWS", "");
			d.Add("OPEN SURGERY", "");
			d.Add("ORTHOPAEDICS", "");
			d.Add("OTHER", "");
			d.Add("PDF LIBRARY", "");
			d.Add("PEOPLE", "");
			d.Add("POLICY", "");
			d.Add("POLICY&REGULATION", "");
			d.Add("POLICYANDREGULATION", "");
			d.Add("REGULATION", "");
			d.Add("REIMBURSEMENT", "");
			d.Add("START-UPSANDSMES", "");
			d.Add("SUPPLEMENTS", "");
			d.Add("SURGICALANDWOUND CARE", "");
			d.Add("VIDEOS&PODCASTS", "");
			d.Add("VIGILANCE", "");
			d.Add("WORLD", "");
			
			return d;
		}
	}
}