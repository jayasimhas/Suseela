using System;
using System.Web.Mvc;
using Informa.Library.Utilities.Extensions;
using Informa.Library.VirtualWhiteboard;
using Informa.Library.VirtualWhiteboard.Models;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Informa.Web.ViewModels.VWB;
using Jabberwocky.Autofac.Attributes;
using Sitecore.Web;
using Sitecore.Configuration;
using Informa.Library.Services.Article;

namespace Informa.Web.Areas.VWB.Controllers
{
	[System.Web.Mvc.Route]
	[AutowireService]
	public class AddIssueController : Controller
	{
		private readonly IDependencies _dependencies;

		[AutowireService(IsAggregateService = true)]
		public interface IDependencies
		{
			IIssuesService IssuesService { get; }
			ISitecoreServiceMaster SitecoreServiceMaster { get; }
		}
		public AddIssueController(IDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		[HttpGet]
		public ActionResult Get(string id)
		{
            if (!Sitecore.Context.User.IsAuthenticated) 
                return Redirect($"{WebUtil.GetFullUrl(Factory.GetSiteInfo("shell").LoginPage)}?returnUrl={Request.RawUrl}");
            
            Guid issueId;
			if (Guid.TryParse(id, out issueId))
			{
				var issue = _dependencies.SitecoreServiceMaster.GetItem<IIssue>(issueId);
                var articleService = DependencyResolver.Current.GetService<IArticleService>();

				if (issue != null)
				{
                    var model = new AddIssueViewModel {
                        Issue = issue,
                        Articles = _dependencies.IssuesService.GetArticles(issueId),
                        ArticleService = articleService
					};
					return View("~/Areas/VWB/Views/AddIssue.cshtml", model);
				}               												
			}

			return Redirect("/vwb");
		}

		[HttpPost]
		public JsonResult ArchiveIssue(string id)
		{
			Guid issueId;
			if (Guid.TryParse(id, out issueId))
			{
				var result = _dependencies.IssuesService.ArchiveIssue(issueId);
				return new JsonResult { Data = result };
			}

			return new JsonResult()
			{
				Data = new
				{
					DebugErrorMessage = "Issue id parse failed.",
					FriendlyErrorMessage = "Cannot find the correct issue.",
					IsSuccess = false
				}
			};
		}

		[HttpPost]
		public ActionResult Save(string issue, string order, string todelete, string title, string date, string notes)
		{
			Guid issueId;
			if (Guid.TryParse(issue, out issueId))
			{
				if (!string.IsNullOrWhiteSpace(todelete))
				{
					_dependencies.IssuesService.DeleteArticles(todelete);
				}

				if (!string.IsNullOrWhiteSpace(order))
				{
					_dependencies.IssuesService.ReorderArticles(order);
				}

			    var model = new IssueModel
			    {
			        Title = title,
                    PublishedDate = date.ToDate(),
			        Notes = notes
			    };

				_dependencies.IssuesService.UpdateIssueItem(model, issueId);
				return Redirect($"/vwb/addissue?id={issueId.ToString("D")}");
			}
			return Redirect(Request?.Url?.ToString());
		}
	}
}