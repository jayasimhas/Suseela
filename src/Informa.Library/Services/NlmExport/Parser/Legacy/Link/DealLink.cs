using Informa.Model.DCD;
using Informa.Models.DCD;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class DealLink : AssetLinkBase
    {
        public override string GetLink(string linkId)
        {
            var finder = new DCDManager();
            Deal deal = finder.GetDealByRecordNumber(linkId);

            if (deal == null)
            {
                return string.Empty;
            }

            return string.Format("{1}/{0}", deal.RecordNumber, BaseUrl);
        }

        public override string GetLinkText(string linkId)
        {
            var finder = new DCDManager();
            Deal deal = finder.GetDealByRecordNumber(linkId);

            if (deal == null)
            {
                return string.Empty;
            }

            return deal.Title;
        }

        public override string LinkType
        {
            get { return "deal"; }
        }
    }
}
