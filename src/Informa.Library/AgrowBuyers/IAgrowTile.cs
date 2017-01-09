using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.AgrowBuyers
{
    interface IAgrowTile
    {
        Image AgrowLogo { get; set; }
        string AgrowTitle { get; set; }
        string AgrowShortDescription { get; set; }
        string AgrowDetailUrl { get; set; }
        string AgrowPageUrl { get; set; }
    }
}
