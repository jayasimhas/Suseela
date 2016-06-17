using System;
using System.Collections.Generic;
using AutoMapper.Internal;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Library.VirtualWhiteboard.Models;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.VirtualWhiteboard
{
    public interface IIssuesService
    {
        VwbResponseModel CreateIssueFromModel(IssueModel model);
    }

    [AutowireService]
    public class IssuesService : IIssuesService
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            ISitecoreServiceMaster SitecoreServiceMaster { get; }
            ISitecoreSecurityWrapper SitecoreSecurityWrapper { get; }
            ISitecoreClonesWrapper SitecoreClonesWrapper { get; }
        }

        public IssuesService(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public VwbResponseModel CreateIssueFromModel(IssueModel model)
        {
            model.Title = model.Title.HasContent() ? model.Title : "New Issue";

            try
            {
                var issueId = CreateIssueItem(model.Title);
                UpdateIssueItem(model, issueId);
            }
            catch (Exception ex)
            {
                return new VwbResponseModel
                {
                    IsSuccess = false,
                    FriendlyErrorMessage =
                        "Creation of new Issue failed.  Reload the Virtual Whiteboard and try again. "
                        + "If the problem persists, please contact your systems administrator.",
                    DebugErrorMessage = ex.Message
                };
            }

            return new VwbResponseModel {IsSuccess = true};
        }

        public Guid CreateIssueItem(string newIssueName)
        {
            var issuesFolder =
                _dependencies.SitecoreServiceMaster.GetItem<IIssue_Folder>(Constants.VirtualWhiteboardIssuesFolder);

            var newIssue = _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
                _dependencies.SitecoreServiceMaster.Create<IIssue, IIssue_Folder>(issuesFolder, newIssueName));

            return newIssue._Id;    
            //Due to a limitation with Glass & RTE fields, this item must be refetched before editing.
            //Thus we only return the Guid.
        }

        public void UpdateIssueItem(IssueModel model, Guid issueId)
        {
            var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId);

            if(issue == null) { return; }

            issue.Title = model.Title;
            issue.Published_Date = model.PublishedDate;
            issue.Notes = model.Notes;

            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
                _dependencies.SitecoreServiceMaster.Save(issue));

            AddArticlesToIssue(issue, model.ArticleIds);
        }

        public void AddArticlesToIssue(IIssue issue, IEnumerable<Guid> itemIds)
        {
            if (itemIds == null) return;

            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
                itemIds.Each(id => _dependencies.SitecoreClonesWrapper.CreateClone(issue._Id, id)));
        }
    }
}