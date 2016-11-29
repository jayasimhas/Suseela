﻿using System;
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

    }
    public enum PdfType
    {
        Static = 1,
        Manual = 2
    }
}
