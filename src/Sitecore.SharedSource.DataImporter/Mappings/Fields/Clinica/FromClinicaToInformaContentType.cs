using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromClinicaToInformaContentType : ToInformaContentType
	{
		public FromClinicaToInformaContentType(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			if (string.IsNullOrEmpty(importValue))
				return;

			//get parent item of list to search
			var sourceItems = GetSourceItems(newItem.Database);
			if (sourceItems == null)
				return;

			Dictionary<string, string> d = GetMapping();

			string lowerValue = importValue.ToLower();
			string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : "News";
			if (string.IsNullOrEmpty(transformValue))
			{
				map.Logger.Log(newItem.Paths.FullPath, "Content Type not converted", ProcessStatus.FieldError, NewItemField, importValue);
				return;
			}

			//loop through children and look for anything that matches by name
			string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
			IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

			//if you find one then store the id
			if (!t.Any())
			{
				map.Logger.Log(newItem.Paths.FullPath, "Content Type not matched", ProcessStatus.FieldError, NewItemField, importValue);
				return;
			}

			Field f = newItem.Fields[NewItemField];
			if (f == null)
				return;

			string ctID = t.First().ID.ToString();
			if (!f.Value.Contains(ctID))
				f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
		}

		protected override Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("analysis", "Analysis");
			d.Add("blog", "Opinion");
			d.Add("briefstory", "News");
			d.Add("conference reports", "News");
			d.Add("company profiles", "News");
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