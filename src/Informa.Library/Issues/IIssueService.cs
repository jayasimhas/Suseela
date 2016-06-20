using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Informa.Library.Issues
{
  public interface IIssueService
  {
    Item Create(string title,DateTime date,string notes);
    bool Delete(string issueItemId); 
    Item Archive(string issueItemId);
  }
}
