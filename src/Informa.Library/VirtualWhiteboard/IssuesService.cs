using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Library.VirtualWhiteboard.Models;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Models;


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
				var issueId = CreateIssueItem<IIssue, IIssue_Folder>(model.Title, Constants.VirtualWhiteboardIssuesFolder);
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

			return new VwbResponseModel { IsSuccess = true };
		}

		public Guid CreateIssueItem<I, F>(string newIssueName, string folderId) where I : class, IGlassBase
			where F : class, IGlassBase
		{
			var issuesFolder =
				_dependencies.SitecoreServiceMaster.GetItem<F>(folderId);

			var newIssue = _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				_dependencies.SitecoreServiceMaster.Create<I, F>(issuesFolder, newIssueName));

			return newIssue._Id;
			//Due to a limitation with Glass & RTE fields, this item must be refetched before editing.
			//Thus we only return the Guid.
		}

		public void UpdateIssueItem(IssueModel model, Guid issueId)
		{
			var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(issueId);

			if (issue == null)
			{
				return;
			}

			issue.Title = model.Title;
			issue.Published_Date = model.PublishedDate;
			issue.Notes = model.Notes;

			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				_dependencies.SitecoreServiceMaster.Save(issue));

			AddArticlesToIssue(issue._Id, model.ArticleIds);
		}

		public void AddArticlesToIssue(Guid issueId, IEnumerable<Guid> itemIds)
		{
			if (itemIds == null) return;

			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				itemIds.Each(id => _dependencies.SitecoreClonesWrapper.CreateClone(issueId, id)));
		}

		public VwbResponseModel ArchiveIssue(Guid issueId)
		{
			var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId);
			if (issue == null)
			{
				return new VwbResponseModel
				{
					DebugErrorMessage = "Issue is null.",
					FriendlyErrorMessage = "Can't find issue.",
					IsSuccess = false
				};
			}

			// Create archived issue item
			var newIssueId = CreateIssueItem<IArchived_Issue, IArchived_Issue_Folder>(issue._Name,
				Constants.VirtualWhiteboardIssuesFolder);
			var newIssue = _dependencies.SitecoreServiceMaster.GetItem<IArchived_Issue>(newIssueId);
			if (newIssue == null)
			{
				return new VwbResponseModel
				{
					DebugErrorMessage = "newIssue is null.",
					FriendlyErrorMessage = "Create archived issue failed.",
					IsSuccess = false
				};
			}

			// Set values
			newIssue.Title = issue.Title;
			newIssue.Published_Date = issue.Published_Date;
			newIssue.Notes = issue.Notes;
			newIssue.Issue_Articles = issue._ChildrenWithInferType;

			// Delete issue with children
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() => _dependencies.SitecoreServiceMaster.Delete(issue));

			return new VwbResponseModel
			{
				IsSuccess = true
			};
		}

		public void DeleteArticles(string ids)
		{
			if (string.IsNullOrWhiteSpace(ids))
			{
				return;
			}

			ids.Split('|')
				.Select(i => _dependencies.SitecoreServiceMaster.GetItem<IArticle>(new Guid(i)))
				.Each(x =>_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() => _dependencies.SitecoreServiceMaster.Delete(x)));
		}

		public void ReorderArticles(Guid issueId, string ids)
		{
			if (!string.IsNullOrWhiteSpace(ids))
			{
				var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId);
				issue.Articles_Order = ids;
				_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				_dependencies.SitecoreServiceMaster.Save(issue));
			}
		}
	}
}