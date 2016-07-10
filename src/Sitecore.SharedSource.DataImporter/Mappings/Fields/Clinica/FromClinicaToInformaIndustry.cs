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

			if (!f.Value.Contains("{2EDCC1FF-26E4-4C6E-AF73-0F43F0F033B8}"))
			{
				f.Value = f.Value.Length == 0
					? "{2EDCC1FF-26E4-4C6E-AF73-0F43F0F033B8}"
					: string.Join("|", new [] {f.Value, "{2EDCC1FF-26E4-4C6E-AF73-0F43F0F033B8}" });
			}
		}
	}
}