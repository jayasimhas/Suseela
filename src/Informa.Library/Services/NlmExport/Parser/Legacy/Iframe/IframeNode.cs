namespace Informa.Library.Services.NlmExport.Parser.Legacy.Iframe
{
	public class IframeNode : IframeBase
	{
        protected override void ConvertIframe(HtmlAgilityPack.HtmlNode node, System.Xml.XmlWriter writer)
        {
            using (writer.StartElement("ext-link"))
            {
                writer.WriteAttributeString("xmlns", "xlink",null, node.GetAttributeValue("src", string.Empty));
            }
        }
    }
}
