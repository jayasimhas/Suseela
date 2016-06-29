using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromTechnologySectionToSubjects : ToInformaSubjects
	{
		public FromTechnologySectionToSubjects(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("business", "Companies");
			d.Add("deals", "Deals");
			d.Add("financials", "Companies");
			d.Add("funding", "Funding");
			d.Add("hta", "Health Technology Assessment");
			d.Add("legal", "Legal");
			d.Add("manda", "M A");
			d.Add("market monitor", "");
			d.Add("policy", "Policy");
			d.Add("policyandregulation", "Policy::Regulation");
			d.Add("regulation", "Regulation");
			d.Add("reimbursement", "Reimbursement");
			d.Add("start-upsandsmes", "StartUps SMEs");
			d.Add("vigilance", "Post-Market Regulation");

			return d;
		}
	}
}