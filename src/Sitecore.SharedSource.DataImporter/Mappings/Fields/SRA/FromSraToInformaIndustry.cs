using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.SRA
{
	public class FromSraToInformaIndustry : ToIndustry
	{
		public FromSraToInformaIndustry(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			if (string.IsNullOrEmpty(importValue))
				return;

			//get parent item of list to search
			Item i = newItem.Database.GetItem(SourceList);
			if (i == null)
				return;

			string lowerValue = importValue.ToLower();
			string transformValue = lowerValue.Equals("medical device") || lowerValue.Equals("medical devices") ? "Medical Device" : "BioPharmaceutical";
			if (string.IsNullOrEmpty(transformValue))
			{
				map.Logger.Log(newItem.Paths.FullPath, "Industry not converted", ProcessStatus.FieldError, NewItemField, importValue);
				return;
			}

			//loop through children and look for anything that matches by name
			string cleanName = StringUtility.GetValidItemName(transformValue, map.ItemNameMaxLength);
			IEnumerable<Item> t = i.Axes.GetDescendants().Where(c => c.DisplayName.Equals(cleanName));

			//if you find one then store the id
			if (!t.Any())
			{
				map.Logger.Log(newItem.Paths.FullPath, "Industry not matched", ProcessStatus.FieldError, NewItemField, importValue);
				return;
			}

			Field f = newItem.Fields[NewItemField];
			if (f == null)
				return;

			string ctID = t.First().ID.ToString();
			if (!f.Value.Contains(ctID))
				f.Value = (f.Value.Length > 0) ? $"{f.Value}|{ctID}" : ctID;
		}
	}
}