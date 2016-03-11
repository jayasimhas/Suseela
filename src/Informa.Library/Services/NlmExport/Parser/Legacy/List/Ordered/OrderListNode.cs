
namespace Informa.Library.Services.NlmExport.Parser.Legacy.List.Ordered
{
    public class OrderListNode : BaseListNode
    {
        public override string ListType
        {
            get { return NlmGlobals.Output.OrderedListType; }
        }

        public override string ContentType
        {
            get { return NlmGlobals.Output.OrderListContentType; }
        }

        public override ListItemNodeType Type
        {
            get { return NlmGlobals.Output.OrderListNodeType; }
        }
    }
}
