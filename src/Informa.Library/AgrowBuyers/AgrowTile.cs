using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.AgrowBuyers
{
    public class AgrowTile: IAgrowTile
    {
        public Image AgrowLogo { get; set; }
        public string AgrowTitle { get; set; }
        public string AgrowShortDescription { get; set; }
        public string AgrowDetailUrl { get; set; }
        public string AgrowPageUrl { get; set; }
    }
}
