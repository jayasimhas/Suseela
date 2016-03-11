
namespace Informa.Library.Services.NlmExport.Parser.Legacy.List.Ordered
{
    public class SideboxOrderedList : OrderListNode
    {
        public override string ContentType
        {
            get { return NlmGlobals.Output.SideboxOrderListContentType; }
        }
    }
}
