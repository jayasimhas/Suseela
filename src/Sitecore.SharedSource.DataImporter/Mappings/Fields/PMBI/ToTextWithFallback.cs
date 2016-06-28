using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.PMBI
{
	public class ToTextWithFallback : ToText
	{
		public ToTextWithFallback(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			if (string.IsNullOrEmpty(importValue))
				return;

			var values = importValue.Split(Delimiter[0]);

			//store the imported value as is
			Field f = newItem.Fields[NewItemField];
			foreach (var value in values)
			{
				if (string.IsNullOrEmpty(value)) continue;

				if (f != null)
				{
					f.Value = value;
					break;
				}	
			}
		}
	}
}
