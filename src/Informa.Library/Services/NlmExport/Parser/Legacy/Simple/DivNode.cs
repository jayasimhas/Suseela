using System;
using Informa.Library.Services.NlmExport.Parser.Legacy.Container;
using Informa.Library.Services.NlmExport.Parser.Legacy.Interface;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Simple
{
    public class DivNode : IDocumentNode
    {
        public void Convert(HtmlAgilityPack.HtmlNode node, System.Xml.XmlWriter writer)
        {
            var divClass = node.GetAttributeValue("class", null);

            if (!string.IsNullOrEmpty(divClass))
            {
                if (divClass.Equals("sidebox", StringComparison.CurrentCultureIgnoreCase))
                {
                    var sidebox = new SideboxNode();
                    sidebox.Convert(node, writer);
                }
            }
        }
    }
}
