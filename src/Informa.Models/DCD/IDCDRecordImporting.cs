using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public interface IDCDRecordImportLog
    {
        System.Nullable<int> RecordId { get; set; }
        ImportLog ImportLog { get; set; }
        string Result { get; set; }
        string Operation { get; set; }
        string Notes { get; set; }
        DateTime TimeStamp { get; set; }

        void InsertRecordOnSubmit(DCDContext context);
        void DeleteRecordOnSubmit(DCDContext context);
    }
}
