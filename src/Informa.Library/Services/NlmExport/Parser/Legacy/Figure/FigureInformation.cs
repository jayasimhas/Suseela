
namespace Informa.Library.Services.NlmExport.Parser.Legacy.Figure
{
    public class FigureInformation
    {
        public string Number { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Source { get; set; }

        public bool IsFull
        {
            get
            {
                return !string.IsNullOrEmpty(Number) &&
                       !string.IsNullOrEmpty(Title) &&
                       !string.IsNullOrEmpty(Link) &&
                       !string.IsNullOrEmpty(Source);
            }
        }
    }
}
