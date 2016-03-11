
namespace Informa.Library.Services.NlmExport.Parser.Legacy.List.Unordered
{
    public class SideboxUnorderedList : UnorderedListNode
    {
        public override string ContentType
        {
            get { return NlmGlobals.Output.SideboxUnorderedListContentType; }
        }
    }
}
