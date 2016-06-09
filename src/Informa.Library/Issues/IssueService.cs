using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glass.Mapper.Sc;
using Informa.Library.Navigation;
using Informa.Library.Services.Global;
using Informa.Library.Utilities.References;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Log = Sitecore.Diagnostics.Log;

namespace Informa.Library.Issues
{
  //[AutowireService(LifetimeScope.SingleInstance)]
  public class IssueService : IIssueService
  {
    private readonly ISitecoreContext _sitecoreContext;
    private readonly IItemReferences _itemReferences;

    public IssueService(ISitecoreContext sitecoreContext, IItemReferences itemReferences)
    {
      _sitecoreContext = sitecoreContext;
      _itemReferences = itemReferences;
    }


    public Item Create(string title, DateTime date, string notes)
    {
      Item currentIssuesRoot = _sitecoreContext.GetItem<Item>(_itemReferences.IssuesRootCurrent);

      if (currentIssuesRoot == null)
      {
        Log.Error("Could not find Current Issues Root item",this);
        return null;
      }
      string issueItemName = ItemUtil.ProposeValidItemName(title);
     

      using (new SecurityDisabler())
      {
        Item newItem = currentIssuesRoot.Add(issueItemName,
       new TemplateItem(_sitecoreContext.GetItem<Item>(_itemReferences.IssueTemplate)));

        newItem.Editing.BeginEdit();

        newItem["Title"] = title;
        newItem["Publish Date"] = DateUtil.ToIsoDate(date); ;
        newItem["Issue Notes"] = notes;

        newItem.Editing.EndEdit();

        return newItem;
      }

      
    }

    public bool Delete(string issueItemId)
    {
      if (!ID.IsID(issueItemId))
      {
        Log.Error("Invalid ID for Deletion: " + issueItemId,this);
        return false;
      }

      Item issueItem = GetIssueItem(issueItemId);
      if (issueItem == null)
      {
        return false;
      }

      using (new SecurityDisabler())
      {
        issueItem.Delete();
      }

      return (_sitecoreContext.GetItem<Item>(issueItemId) == null);
    }

    private Item GetIssueItem(string issueItemId)
    {
      Item issueItem = _sitecoreContext.GetItem<Item>(issueItemId);

      if (issueItem == null)
      {
        Log.Error("Issue not found for ID: " + issueItemId, this);
        return null;
      }

      return issueItem;
    }

    public Item Archive(string issueItemId)
    {

      Item issueItem = GetIssueItem(issueItemId); 
      if (issueItem == null)
      {
        return null; 
      }
      using (new SecurityDisabler())
      {
        //Move the item to the archive folder
        issueItem.MoveTo(_sitecoreContext.GetItem<Item>(_itemReferences.IssuesRootArchive));

        //Get all of the current article IDs and remove them
        List<string> issueItemIds =
          issueItem.Axes.GetDescendants().Select(descendant => descendant.Source.ID.ToString()).ToList();
        
        //Change the template to archived issue template
        issueItem.ChangeTemplate(new TemplateItem(_sitecoreContext.GetItem<Item>(_itemReferences.IssueArchivedTemplate)));

        //Save the list of article ids to the archived issue multi list
        issueItem.Editing.BeginEdit();
        issueItem["Issue Content"] = string.Join("|", issueItemIds);
        issueItem.Editing.EndEdit();

        //Delete any children items as they are now saved in the multilist
        issueItem.DeleteChildren();

        return issueItem;
      }
    }
  } 
}
