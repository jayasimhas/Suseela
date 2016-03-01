using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Web.ViewModels
{
    public interface ISearchViewModel
    {
        string PageFirstText { get; }
        string PageLastText { get; }
        string SearchTipsText { get; }
        string SearchTitleText { get; }
        string SearchViewHeadlinesOnlyText { get; }
    }

}
