using System;

namespace Informa.Library.Services.NlmExport.Logging
{
    public class NlmLogMessage
    {
        public string Username { get; set; }
        public string ArticlePath { get; set; }
        public string ArticleId { get; set; }
        public DateTime ExportTime { get; set; }

        public ExportResult ExportResult { get; set; }
    }
}
