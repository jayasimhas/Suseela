using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.SRA
{
	public class FromSectionToSubjects : ToInformaSubjects
	{
		public FromSectionToSubjects(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("advanced therapies", "Advanced Therapies");
			d.Add("analysis", "Analysis");
			d.Add("application process", "Application Process");
			d.Add("asia", "");
			d.Add("biologics", "Biologics");
			d.Add("clinical trials", "Clinical Trials");
			d.Add("clinical development trials", "Clinical Trials");
			d.Add("combination products", "Combination Products");
			d.Add("diagnostics", "Diagnostics");
			d.Add("europe", "");
			d.Add("events", "Events");
			d.Add("features", "Features");
			d.Add("healthcare systems", "Healthcare Systems");
			d.Add("home", "Home");
			d.Add("ip", "Intellectual Property");
			d.Add("labelling and packaging", "Labelling Packaging");
			d.Add("latin america", "");
			d.Add("medical devices", "Medical Devices");
			d.Add("multimedia", "Multimedia");
			d.Add("new articles", "New Articles");
			d.Add("news", "News");
			d.Add("north america", "");
			d.Add("pdf library", "");
			d.Add("people", "People");
			d.Add("pharmaceuticals", "Pharmaceuticals");
			d.Add("pharmacovigilance", "Pharmacovigilance");
			d.Add("postmarketing regulation", "Post Market Regulation");
			d.Add("premarketing regulation", "Pre Market Regulation");
			d.Add("post-market regulation", "Post Market Regulation");
			d.Add("pre-market regulation", "Pre Market Regulation");
			d.Add("pricing and reimbursement", "Pricing Reimbursement");
			d.Add("qse", "QSE");
			d.Add("randd", "Research Development");
			d.Add("reference", "Reference");
			d.Add("structures", "Structures");
			d.Add("trade", "Trade");
			d.Add("veterinary", "Veterinary");

			return d;
		}
	}
}