using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.DCD.XMLImporting
{
    public enum ImportResult
    {
        InProgress,
        Success,
        PartialSuccess,
        Failure
    }

    public enum RecordImportResult
    {
        Success,
        Skipped,
        Failure
    }

    public enum RecordImportOperation
    {
        Update,
        Insert,
        Delete
    }
}
