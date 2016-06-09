using System;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Issues;
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
    private readonly IItemReferences _itemReferences;
    private readonly ISitecoreContext _sitecoreContext;

    public IssueService(ISitecoreContext sitecoreContext, IItemReferences itemReferences)
    {
      _sitecoreContext = sitecoreContext;
      _itemReferences = itemReferences;
    }

    /// <summary>
    ///   Create an issue
    /// </summary>
    /// <param name="title"></param>
    /// <param name="date"></param>
    /// <param name="notes"></param>
    /// <returns></returns>
    public Item Create(string title, DateTime date, string notes)
    {
      var currentIssuesRoot = _sitecoreContext.GetItem<Item>(_itemReferences.IssuesRootCurrent);

      if (currentIssuesRoot == null)
      {
        Log.Error("Could not find Current Issues Root item", this);
        return null;
      }

      var issueItemName = ItemUtil.ProposeValidItemName(title);

      using (new SecurityDisabler())
      {
        var newItem = currentIssuesRoot.Add(issueItemName,
          new TemplateItem(_sitecoreContext.GetItem<Item>(IIssueConstants.TemplateId.ToString())));

        newItem.Editing.BeginEdit();

        newItem[IIssueConstants.TitleFieldName] = title;
        newItem[IIssueConstants.Publish_DateFieldName] = DateUtil.ToIsoDate(date);
        ;
        newItem[IIssueConstants.Issue_NotesFieldName] = notes;

        newItem.Editing.EndEdit();

        return newItem;
      }
    }

    /// <summary>
    ///   Delete an issue
    /// </summary>
    /// <param name="issueItemId"></param>
    /// <returns></returns>
    public bool Delete(string issueItemId)
    {
      if (!ID.IsID(issueItemId))
      {
        Log.Error("Invalid ID for Deletion: " + issueItemId, this);
        return false;
      }

      var issueItem = GetIssueItem(issueItemId);
      if (issueItem == null)
      {
        return false;
      }

      using (new SecurityDisabler())
      {
        issueItem.Delete();
      }

      return _sitecoreContext.GetItem<Item>(issueItemId) == null;
    }

    /// <summary>
    ///   Arhive an issue.  This will do the following
    ///   1) Move the issue item and change the template to archived issue
    ///   2) Remove the children cloned items and put their ids in the archived issue
    /// </summary>
    /// <param name="issueItemId"></param>
    /// <returns></returns>
    public Item Archive(string issueItemId)
    {
      var issueItem = GetIssueItem(issueItemId);
      if (issueItem == null)
      {
        return null;
      }
      using (new SecurityDisabler())
      {
        //Move the item to the archive folder
        var archiveRoot = _sitecoreContext.GetItem<Item>(_itemReferences.IssuesRootArchive);
        var year = issueItem.Statistics.Created.Year;
        var archivedFolder = GetOrCreateItem(_sitecoreContext.Database, archiveRoot, year.ToString(),
          _sitecoreContext.GetItem<Item>(IIssue_FolderConstants.TemplateId.ToString()));
        issueItem.MoveTo(archivedFolder);

        //Get all of the current article IDs and remove them
        var issueItemIds =
          issueItem.Axes.GetDescendants().Select(descendant => descendant.Source.ID.ToString()).ToList();

        //Change the template to archived issue template
        issueItem.ChangeTemplate(_sitecoreContext.GetItem<Item>(IArchived_IssueConstants.TemplateId.ToString()));

        //Save the list of article ids to the archived issue multi list
        issueItem.Editing.BeginEdit();
        issueItem[IArchived_IssueConstants.Issue_ContentsFieldName] = string.Join("|", issueItemIds);
        issueItem.Editing.EndEdit();

        //Delete any children items as they are now saved in the multilist
        issueItem.DeleteChildren();

        return issueItem;
      }
    }

    /// <summary>
    ///   Gets an issue and verifies that it is the correct type of item
    /// </summary>
    /// <param name="issueItemId"></param>
    /// <returns></returns>
    private Item GetIssueItem(string issueItemId)
    {
      var issueItem = _sitecoreContext.GetItem<Item>(issueItemId);

      if (issueItem == null)
      {
        Log.Error("Issue not found for ID: " + issueItemId, this);
        return null;
      }

      //if(issueItem.TemplateID != [Add ID])
      //{
      //  Log.Error("Item was not an issue: " + issueItem.Paths.Path, this);
      //  return null;
      //}

      return issueItem;
    }

    private Item GetOrCreateItem(Database database, Item root, string name, TemplateItem template)
    {
      if (string.IsNullOrEmpty(name))
      {
        return null;
      }

      if (database == null || root == null || template == null)
      {
        return null;
      }

      var cleanName = ItemUtil.ProposeValidItemName(name);

      var path = root.Paths.Path + "/" + cleanName;

      var item = database.GetItem(path);

      if (item == null)
      {
        var newContentItem = root.Add(cleanName, template);
        return newContentItem;
      }

      return item;
    }
  }
}