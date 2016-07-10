using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.SRA
{
	public class FromSraToInformaContentType : ToInformaContentType
	{
		public FromSraToInformaContentType(Item i) : base(i)
		{
		}

		protected override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("analysis", "Analysis");
			d.Add("blog", "Opinion");
			d.Add("conference reports", "News");
			d.Add("country profiles", "News");
			d.Add("edcarticle", "News");
			d.Add("editorial", "Opinion");
			d.Add("event stories", "Analysis");
			d.Add("eventstory", "Analysis");
			d.Add("expert view", "Opinion");
			d.Add("feature", "Analysis");
			d.Add("guidelines & standards", "News");
			d.Add("guidelines &amp; standards", "News");
			d.Add("guidelinesandstandards", "News");
			d.Add("headline news", "News");
			d.Add("interview", "Analysis");
			d.Add("mainstory", "News");
			d.Add("news", "News");
			d.Add("people", "News");
			d.Add("webinar", "News");

			return d;
		}
	}
}