using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.PDF
{
    public class Pdf
    {
        public DateTime IssueDate { get; set; }
        public string IssueNumber { get; set; }
        public string PdfPageUrl { get; set; }
        public PdfType TypeOfPdf { get; set; }
        public string PdfTitle { get; set; }
        public DateTime? PubStartDate { get; set; }
        public DateTime? PubEndDate { get; set; }
        public int ArticleSize { get; set; }

    }
    /// <summary>
    /// Enum for PDF Type
    /// </summary>
    public enum PdfType
    {
        Static = 1,
        Manual = 2,
        Personalized = 3
    }
}
