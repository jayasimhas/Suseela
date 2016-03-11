using Informa.Models.DCD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public interface IDCD
    {
        int RecordId { get; set; }
        string RecordNumber { get; set; }
        string Title { get; set; }
        DateTime LastModified { get; set; }
        DateTime Created { get; set; }
        DateTime Published { get; set; }
        string Content { get; set; }

        void InsertRecordOnSubmit(DCDContext context);
        void DeleteRecordOnSubmit(DCDContext context);
    }
}
