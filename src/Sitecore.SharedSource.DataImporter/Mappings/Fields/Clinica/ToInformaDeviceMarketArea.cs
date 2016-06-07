using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class ToInformaDeviceMarketArea : ListToGuid
	{
		public ToInformaDeviceMarketArea(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			if (string.IsNullOrEmpty(importValue))
				return;

			var sourceItems = GetSourceItems(newItem.Database);
			if (sourceItems == null)
				return;

			Field f = newItem.Fields[NewItemField];
			if (f == null)
				return;

			Dictionary<string, string> d = GetMapping();

			var values = importValue.Split(GetFieldValueDelimiter()?[0] ?? ',');

			foreach (var val in values)
			{
				string lowerValue = val.ToLower();
				string transformValue = (d.ContainsKey(lowerValue)) ? d[lowerValue] : string.Empty;
				if (string.IsNullOrEmpty(transformValue))
				{
					map.Logger.Log(newItem.Paths.FullPath, "Device Market Area(s) not converted", ProcessStatus.FieldError, NewItemField, val);
					continue;
				}

				string[] parts = transformValue.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
				
				//loop through children and look for anything that matches by name
				foreach (string area in parts)
				{
					string cleanName = StringUtility.GetValidItemName(area, map.ItemNameMaxLength);
					IEnumerable<Item> t = sourceItems.Where(c => c.DisplayName.Equals(cleanName));

					//if you find one then store the id
					if (!t.Any())
					{
						map.Logger.Log(newItem.Paths.FullPath, "Device Market Area(s) not found in list", ProcessStatus.FieldError, NewItemField, area);
						continue;
					}

					string ctID = t.First().ID.ToString();
					if (!f.Value.Contains(ctID))
						f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
				}
			}
		}

		public virtual Dictionary<string, string> GetMapping()
		{
			Dictionary<string, string> d = new Dictionary<string, string>();

			d.Add("cancer", "Cancer");
			d.Add("cardiovascular", "Cardiology");
			d.Add("cns", "Neurology");
			d.Add("cellularandgenetic", "Cellular & Genetic");
			d.Add("dermatological", "Dermatology");
			d.Add("diagnostic", "In Vitro Diagnotics");
			d.Add("gastrointestinal", "Gastroenterology");
			d.Add("genitourinary", "Gynecology and Urology");
			d.Add("hormonal", "Metabolic");
			d.Add("imaging&it", "Diagnostic Imaging");
			d.Add("imagingandit", "Diagnostic Imaging");
			d.Add("imminological", "Immunology");
			d.Add("infection", "");
			d.Add("ivds", "In Vitro Diagnostics");
			d.Add("metabolic", "Metabolic");
			d.Add("musculoskeletal", "Orthopedics");
			d.Add("neurological", "Neurology");
			d.Add("open surgery", "Surgery");
			d.Add("orthopedics", "Orthopedics");
			d.Add("surgicalandwound care", "Wound Management");
			d.Add("poisoning", "");
			d.Add("respiratory", "Respiratory");
			d.Add("sensory", "");

			d.Add("analysis", "");
			d.Add("asia pacific", "Asia");
			d.Add("blog open surgery", "");
			d.Add("business", "");
			d.Add("deals", "");
			d.Add("ece_incoming", "");
			d.Add("europe", "Europe");
			d.Add("events", "");
			d.Add("features", "");
			d.Add("financials", "");
			d.Add("frontpage", "");
			d.Add("funding", "");
			d.Add("home", "");
			d.Add("hta", "");
			d.Add("inside china: investing in medtech companies", "China");
			d.Add("latin america", "Americas");
			d.Add("legal", "");
			d.Add("live", "");
			d.Add("manda", "");
			d.Add("market monitor", "");
			d.Add("market sector", "");
			d.Add("medtech ventures", "");
			d.Add("new articles", "");
			d.Add("news", "");
			d.Add("other", "");
			d.Add("pdf library", "");
			d.Add("people", "");
			d.Add("policy", "");
			d.Add("policy&regulation", "");
			d.Add("policyandregulation", "");
			d.Add("regulation", "");
			d.Add("reimbursement", "");
			d.Add("start-upsandsmes", "");
			d.Add("supplements", "");
			d.Add("us", "United States");
			d.Add("videos&podcasts", "");
			d.Add("vigilance", "");
			d.Add("world", "");

			return d;
		}
	}
}