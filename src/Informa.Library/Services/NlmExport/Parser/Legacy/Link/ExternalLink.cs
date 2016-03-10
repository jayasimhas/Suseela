namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class ExternalLink : AssetLinkBase
    {
        public override string GetLink(string linkId)
        {
            return linkId;
        }

        public override string GetLinkText(string linkId)
        {
            return linkId;
        }

        public override string LinkType
        {
            get { return "uri"; }
        }

        public override bool UseItalics
        {
            get
            {
                return false;
            }
        }
    }
}
