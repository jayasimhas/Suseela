using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromSectionToSubjects : ToInformaSubjects
	{
		public FromSectionToSubjects(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("business", "Companies");
			d.Add("deals", "Deals");
			d.Add("events", "Events");
			d.Add("financials", "Companies");
			d.Add("funding", "Funding");
			d.Add("hta", "Health Technology Assessment");
			d.Add("legal", "Legal Issues");
			d.Add("manda", "M A");
			d.Add("m&a", "M A");
			d.Add("market sector", "Markets");
			d.Add("medtech ventures", "Medtech Ventures::StartUps SMEs");
			d.Add("people", "Companies");
			d.Add("policy", "Policy");
			d.Add("policy&regulation", "Policy::Regulation");
			d.Add("policyandregulation", "Policy::Regulation");
			d.Add("regulation", "Regulation");
			d.Add("reimbursement", "Reimbursement");
			d.Add("start-upsandsmes", "StartUps SMEs");
			d.Add("StartUps & SMEs", "StartUps SMEs");
			d.Add("vigilance", "Product Safety");

			d.Add("analysis", "");
			d.Add("asia pacific", "Asia");
			d.Add("blog open surgery", "");
			d.Add("cardiovascular", "");
			d.Add("cellularandgenetic", "");
			d.Add("ece_incoming", "");
			d.Add("europe", "Europe");
			d.Add("features", "");
			d.Add("frontpage", "");
			d.Add("home", "");
			d.Add("imaging&it", "");
			d.Add("imagingandit", "");
			d.Add("inside china: investing in medtech companies", "China");
			d.Add("ivds", "");
			d.Add("latin america", "Americas");
			d.Add("live", "");
			d.Add("market monitor", "");
			d.Add("neurological", "");
			d.Add("new articles", "");
			d.Add("news", "");
			d.Add("open surgery", "");
			d.Add("orthopaedics", "");
			d.Add("other", "");
			d.Add("pdf library", "");
			d.Add("supplements", "");
			d.Add("surgicalandwound care", "");
			d.Add("us", "United States");
			d.Add("videos&podcasts", "");
			d.Add("world", "");

			return d;
		}
	}
}