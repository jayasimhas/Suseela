
namespace Informa.Library.Services.NlmExport.Parser.Legacy.List.Unordered
{
    public class UnorderedListNode : BaseListNode
    {
        public override string ListType
        {
            get { return NlmGlobals.Output.UnorderedListType; }
        }

        public override string ContentType
        {
            get { return NlmGlobals.Output.UnorderedListContentType; }
        }

        public override ListItemNodeType Type
        {
            get { return NlmGlobals.Output.UnorderedListNodeType; }
        }
    }
}
