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

			d.Add("medical devices", "");
			d.Add("pdf library", "");
			d.Add("pharmaceuticals", "");
			d.Add("events", "");
			d.Add("news", "");
			d.Add("advanced therapies", "");
			d.Add("pharmacovigilance", "");
			d.Add("labelling and packaging", "");
			d.Add("europe", "Europe");
			d.Add("ip", "");
			d.Add("asia", "Asia");
			d.Add("north america", "Americas");
			d.Add("randd", "");
			d.Add("application process", "");
			d.Add("people", "");
			d.Add("trade", "");
			d.Add("diagnostics", "");
			d.Add("postmarketing regulation", "");
			d.Add("features", "");
			d.Add("analysis", "");
			d.Add("clinical trials", "");
			d.Add("home", "");
			d.Add("reference", "");
			d.Add("veterinary", "");
			d.Add("premarketing regulation", "");
			d.Add("biologics", "");
			d.Add("new articles", "");
			d.Add("latin america", "Americas");
			d.Add("healthcare systems", "");
			d.Add("qse", "");
			d.Add("multimedia", "");
			d.Add("pricing and reimbursement", "");
			d.Add("combination products", "");
			d.Add("structures", "");

			return d;
		}
	}
}