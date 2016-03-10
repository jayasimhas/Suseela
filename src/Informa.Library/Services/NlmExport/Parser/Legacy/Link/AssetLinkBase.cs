using System.IO;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public interface IAssetLink
    {
        string GetLink(string linkId);
        string GetLinkText(string linkId);
        string LinkType { get; }
        bool UseItalics { get; }
    }

    public abstract class AssetLinkBase : IAssetLink
    {
        public static readonly string BaseUrl = string.Empty;

        public abstract string GetLink(string linkId);

        public abstract string GetLinkText(string linkId);

        public abstract string LinkType { get; }

        public virtual bool UseItalics
        {
            get { return true; }
        }

        public virtual void Write(StreamWriter writer, string link, string text)
        {
            var nodeLink = GetLink(link);

            var startTag = string.Format("{3}ext-link xlink:href=\"{0}\" ext-link-type=\"{1}\"{2}", nodeLink, LinkType, 
                LinkNode.GreaterThanTemporaryIdentifier, LinkNode.LessThanTemporaryIdentifier);
            writer.Write(startTag);

            string linkText = string.Empty;

            if (UseItalics)
            {
                linkText += LinkNode.LessThanTemporaryIdentifier + "italic" + LinkNode.GreaterThanTemporaryIdentifier;
            }

            linkText += text;

            if (UseItalics)
            {
                linkText += LinkNode.LessThanTemporaryIdentifier + "/italic" + LinkNode.GreaterThanTemporaryIdentifier;
            }

            writer.Write(linkText);

            writer.Write(LinkNode.LessThanTemporaryIdentifier + "/ext-link" + LinkNode.GreaterThanTemporaryIdentifier);
        }

        public void Write(StreamWriter writer, string linkId)
        {
            Write(writer, linkId, GetLinkText(linkId));
        }
    }
}
