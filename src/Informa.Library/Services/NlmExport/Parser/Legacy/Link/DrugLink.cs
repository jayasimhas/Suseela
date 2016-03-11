using Informa.Model.DCD;
using Informa.Models.DCD;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class DrugLink : AssetLinkBase
    {
        public override string GetLink(string linkId)
        {
            var finder = new DCDManager();
            Drug drug = finder.GetDrugByRecordNumber(linkId);

            if (drug == null)
            {
                return string.Empty;
            }

            return string.Format("{1}/d/{0}", drug.RecordNumber, BaseUrl);
        }

        public override string GetLinkText(string linkId)
        {
            var finder = new DCDManager();
            Drug drug = finder.GetDrugByRecordNumber(linkId);

            if (drug == null)
            {
                return string.Empty;
            }

            return drug.Title;
        }

        public override string LinkType
        {
            get { return "drug"; }
        }
    }
}
