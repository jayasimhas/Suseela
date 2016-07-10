using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.SRA
{
	public class FromTechnologySectionToSubjects : ToInformaSubjects
	{
		public FromTechnologySectionToSubjects(Item i) : base(i)
		{
		}

		public override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("application process", "Application Process");
			d.Add("clinical trials", "Clinical Development Trials");
			d.Add("randd", "Research Development");
			d.Add("pharmacovigilance", "Pharmacovigilance");
			d.Add("qse", "Product Safety::Manufacturing");
			d.Add("structures", "Structures");
			d.Add("trade", "Trade");
			d.Add("policy", "Policy");
			d.Add("premarketing regulation", "Pre-Market Regulation");
			d.Add("hta", "Health Technology Assessment");
			d.Add("promotion", "Advertising Promotion  Regulation");
			d.Add("postmarketing regulation", "Post-Market Regulation");
			d.Add("labellingandpackaging", "Labelling Packaging::Advertising Promotion  Regulation");
			d.Add("labelling & packaging", "Labelling Packaging::Advertising Promotion  Regulation");
			d.Add("ip", "Intellectual Property");
			d.Add("prescribing", "Prescribing");
			d.Add("pricingandreimbursement", "Pricing Strategies::Reimbursement");
			d.Add("variations", "Variations");
			d.Add("r&ampd", "Research Development");
			d.Add("labelling&amppackaging", "Labelling Packaging::Advertising Promotion  Regulation");
			d.Add("product liability", "Product Liability");
			d.Add("legal", "Legal Issues");
			d.Add("pricing&ampreimbursement", "Pricing Strategies::Reimbursement");
			d.Add("packaging&amplabelling", "Labelling Packaging::Advertising Promotion  Regulation");
			d.Add("packagingandlabelling", "Labelling Packaging::Advertising Promotion  Regulation");
			d.Add("competition", "Strategy");
			d.Add("safety", "Product Safety");
			d.Add("vigilance", "Pharmacovigilance");
			d.Add("insurance", "Insurance");
			d.Add("pricingandampreimbursement", "Pricing Strategies::Reimbursement");
			d.Add("r&ampampd", "Research Development");
			
			return d;
		}
	}
}