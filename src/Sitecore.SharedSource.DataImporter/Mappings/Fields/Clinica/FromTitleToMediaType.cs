using System.Linq;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromTitleToMediaType : ListToGuid
	{
		public FromTitleToMediaType(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			var field = newItem.Fields[NewItemField];
			var item = newItem.Database.GetItem(SourceList);

			if (!string.IsNullOrWhiteSpace(importValue) && item != null)
			{
				string lowerVal = importValue.ToLower();

				if (lowerVal.Contains("podcast"))
				{
					var audioVal = item.GetChildren().FirstOrDefault(i => i.Fields["Item Name"].Value == "Audio")?.ID.ToString();
					field.Value = audioVal;
				}

				if (lowerVal.Contains("video interview"))
				{
					var videoVal = item.GetChildren().FirstOrDefault(i => i.Fields["Item Name"].Value == "Video")?.ID.ToString();
					field.Value = videoVal;
				}
			}
		}
	}
}