using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromClinicaToInformaContentType : ToInformaContentType
	{
		public FromClinicaToInformaContentType(Item i) : base(i)
		{
		}

		protected override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("analysis", "Analysis");
			d.Add("blog", "Opinion");
			d.Add("briefstory", "News");
			d.Add("conference reports", "News");
			d.Add("country profiles", "News");
			d.Add("edcarticle", "News");
			d.Add("event stories", "News");
			d.Add("events", "News");
			d.Add("expert view", "Opinion");
			d.Add("feature", "Analysis");
			d.Add("headline news", "News");
			d.Add("highlight", "News");
			d.Add("highlightart", "News");
			d.Add("interview", "Analysis");
			d.Add("mainstory", "Analysis");
			d.Add("market insight", "News");
			d.Add("news", "News");
			d.Add("patent watch", "News");
			d.Add("people", "News");
			d.Add("scripinsight", "News");
			d.Add("supplements", "News");
			d.Add("webinar", "News");
			
			return d;
		}
	}
}