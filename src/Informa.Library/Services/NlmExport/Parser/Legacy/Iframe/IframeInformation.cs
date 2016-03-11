namespace Informa.Library.Services.NlmExport.Parser.Legacy.Iframe
{
	public class IframeInformation
	{
        public string Number { get; set; }

		public string Title { get; set; }

        public string Header { get; set; }

	    public string Caption { get; set; }

        public string Source { get; set; }

        public string Link { get; set; }

	    public bool IsFull
	    {
	        get
	        {
	            return !string.IsNullOrEmpty(Title) &&
	                   !string.IsNullOrEmpty(Header) &&
	                   !string.IsNullOrEmpty(Caption) &&
	                   !string.IsNullOrEmpty(Source) &&
	                   !string.IsNullOrEmpty(Link);
	        }
	    }

	}

}
