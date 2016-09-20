using Informa.Library.Utilities.CMSHelpers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.Clinica
{
	public class FromClinicaToInformaIndustry : ListToGuid
	{
		public FromClinicaToInformaIndustry(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			Field f = newItem.Fields[NewItemField];

			if (!f.Value.Contains(ItemIdResolver.GetItemIdByKey("MedicalDeviceItem")))
			{
				f.Value = f.Value.Length == 0
					? ItemIdResolver.GetItemIdByKey("MedicalDeviceItem")
                    : string.Join("|", new [] {f.Value, ItemIdResolver.GetItemIdByKey("MedicalDeviceItem") });
			}
		}
	}
}