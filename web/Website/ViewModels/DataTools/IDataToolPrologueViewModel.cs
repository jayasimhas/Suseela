using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.DataTools
{
    public interface IDataToolPrologueViewModel
    {
        IDataToolProloguePrintViewModel PrintViewModel { get; }

        IDataToolPrologueEmailViewModel EmailViewModel { get; }

        IDataToolPrologueShareViewModel ShareViewModel { get; }

        IDataToolTagsViewModel TagsViewModel { get; }
    }
}