using Glass.Mapper.Sc.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.JobsAndClassifieds
{
   public interface IJobTile
    {
        Image JobLogo { get; set; }
        string JobTitle { get; set; }
        string JobShortDescription { get; set; }
        DateTime JobPublishedDate { get; set; }
        string JobDetailUrl { get; set; }
    }
}
