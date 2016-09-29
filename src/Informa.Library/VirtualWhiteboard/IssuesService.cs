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
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Models;
using Newtonsoft.Json;
using Sitecore.Mvc.Extensions;
using Velir.Core.Extensions.System.Collections.Generic;
using EnumerableExtensions = Informa.Library.Utilities.Extensions.EnumerableExtensions;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Data;

namespace Informa.Library.VirtualWhiteboard
{
	public interface IIssuesService
	{
		VwbResponseModel CreateIssueFromModel(IssueModel model);
		IEnumerable<IArticle> GetArticles(Guid issueId);
		VwbResponseModel ArchiveIssue(Guid issueId);
		void ReorderArticles(string ids);
		void DeleteArticles(string ids);
	    void UpdateIssueItem(IssueModel model, Guid issueId);
        IEnumerable<IIssue> GetActiveIssues();
		bool DoesIssueContains(Guid issueId, string articleId);
		void AddArticlesToIssue(Guid issueId, IEnumerable<Guid> itemIds);
	    void UpdateEditorialNotes(string notesJson);
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
			ICacheProvider CacheProvider { get; }
		}

		public IssuesService(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public VwbResponseModel CreateIssueFromModel(IssueModel model)
		{
			model.Title = model.Title.HasContent() ? model.Title : "New Issue";
			Guid issueId;
			try
			{
				issueId = CreateIssueItem<IIssue, IIssue_Folder>(model.Title, Constants.VirtualWhiteboardIssuesFolder);
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

			return new VwbResponseModel { IsSuccess = true, IssueId = issueId };
		}

		public Guid CreateIssueItem<I, F>(string newIssueName, string folderId) where I : class, IGlassBase
			where F : class, IGlassBase
		{
			var issuesFolder =
				_dependencies.SitecoreServiceMaster.GetItem<F>(folderId);

			var newIssue = _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				_dependencies.SitecoreServiceMaster.Create<I, F>(issuesFolder, ItemUtil.ProposeValidItemName(newIssueName)));

			return newIssue._Id;
			//Due to a limitation with Glass & RTE fields, this item must be refetched before editing.
			//Thus we only return the Guid.
		}

		public void UpdateIssueItem(IssueModel model, Guid issueId)
		{
			var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(issueId);

			if (issue == null)
			{
				throw new Exception($"Failed to fetch IIssue__Raw with id = {issueId}");
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
			if (itemIds == null)
			{
				return;
			}

			var enumerable = itemIds as IList<Guid> ?? itemIds.ToList();
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
			{
				enumerable.ForEach(id => _dependencies.SitecoreClonesWrapper.CreateClone(id, issueId));
			});
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
				Constants.VirtualWhiteboardArchivedIssuesFolder);
			var newIssue = _dependencies.SitecoreServiceMaster.GetItem<IArchived_Issue__Raw>(newIssueId);
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
			newIssue.Issue_Articles =
				issue._ChildrenWithInferType.Cast<IArticle>()
					.Each(i => _dependencies.SitecoreServiceMaster.GetItem<IArticle>(new Guid(i.SourceId.ExtractGuidString())));
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() => _dependencies.SitecoreServiceMaster.Save(newIssue));

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
			ids.Split('|').Select(i => new Guid(i)).ForEach(i =>
				_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
					_dependencies.SitecoreServiceMaster.Delete(_dependencies.SitecoreServiceMaster.GetItem<IArticle>(i))
				));
		}

	    public void ReorderArticles(string ids)
		{
			if (!string.IsNullOrWhiteSpace(ids))
			{
				float startOrderNumber = 100;
				ids.Split('|').Select(i => new Guid(i)).ForEach(i =>
				{
					_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
					{
						var article = _dependencies.SitecoreServiceMaster.GetItem<IArticle__Raw>(i);
						if (article != null)
						{
							article.Sortorder = startOrderNumber++;
							_dependencies.SitecoreServiceMaster.Save(article);
						}
					});
				});
			}
		}

		public void UpdateIssueInfo(Guid issueId, string title, string date, string notes)
		{
			var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(issueId);
			issue.Title = title;
		    issue.Notes = notes;
			DateTime publishDate;
			if (DateTime.TryParse(date, out publishDate))
			{
				issue.Published_Date = publishDate;
			}
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(() =>
				_dependencies.SitecoreServiceMaster.Save(issue));
		}

		public IEnumerable<IArticle> GetArticles(Guid issueId)
		{
			var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId);
			return issue == null || !issue._ChildrenWithInferType.Any()
				? Enumerable.Empty<IArticle>()
				: issue._ChildrenWithInferType.Cast<IArticle>();
		}

		public IEnumerable<IIssue> GetActiveIssues()
		{
			return
				_dependencies.SitecoreServiceMaster.GetItem<IIssue_Folder>(new Guid(Constants.VirtualWhiteboardIssuesFolder))
					._ChildrenWithInferType.Cast<IIssue>();
		}

		public HashSet<string> GetIssueArticlesSet(Guid issueId)
		{
			var cacheKey = $"Issue-{issueId}";
			return _dependencies.CacheProvider.GetFromCache(cacheKey, () => BuildIssueDictionary(issueId));
		}

		public HashSet<string> BuildIssueDictionary(Guid issueId)
		{
			var dict = new HashSet<string>();
            IEnumerable<IGlassBase> items = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId)._ChildrenWithInferType;
            string idFormat = "B";
            foreach(var i in items) {
                Guid id = ReturnSourceID(i);
                if (Guid.Empty.Equals(id))
                    continue;

                if (dict.Contains(id.ToString(idFormat).ToLower()))
                    continue;

                dict.Add(id.ToString(idFormat));
            }

			return dict;
		}

        public Guid ReturnSourceID(IGlassBase i) {
            if (i == null)
                return Guid.Empty;

            Item itm = _dependencies.SitecoreServiceMaster.GetItem<Item>(i._Id);
            if (itm == null)
                return Guid.Empty;

            Field f = itm.Fields[Sitecore.FieldIDs.Source];
            if (f == null)
                return Guid.Empty;

            if (!ItemUri.IsItemUri(f.Value))
                return Guid.Empty;

            return ItemUri.Parse(f.Value).ItemID.Guid;
        }

		public bool DoesIssueContains(Guid issueId, string articleId)
		{
			return GetIssueArticlesSet(issueId).Contains(articleId.ToLower());
		}

	    public void UpdateEditorialNotes(string notesJson)
	    {
            var idToNoteDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(notesJson);

	        foreach (var kvPair in idToNoteDict)
	        {
                Guid id;
                if(!Guid.TryParse(kvPair.Key, out id)) { continue; }

	            var article = _dependencies.SitecoreServiceMaster.GetItem<IArticle__Raw>(id);
                if(article == null) { continue; }

	            article.Editorial_Notes = kvPair.Value;
                _dependencies.SitecoreServiceMaster.Save(article);
	        }
        }
	}
}