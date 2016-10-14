using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Services.NlmExport.Parser.Legacy.Link;
using Informa.Library.Utilities.References;
using Informa.Library.VirtualWhiteboard;
using Informa.Library.VirtualWhiteboard.Models;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using Jabberwocky.Glass.Models;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data.Items;

namespace Informa.Tests.Library.VirtualWhiteboard_Tests
{

    [TestFixture]
    public class IssuesService_Tests
    {
        IssuesService _issuesService;
        IssuesService.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<IssuesService.IDependencies>();
            _issuesService = new IssuesService(_dependencies);
        }

        private void SetUpFakeIssuesFolder()
        {
            var fakeIssuesFolder = Substitute.For<IIssue_Folder>();
            fakeIssuesFolder._Id.Returns(new Guid(Constants.VirtualWhiteboardIssuesFolder));
            _dependencies.SitecoreServiceMaster.GetItem<IIssue_Folder>(Constants.VirtualWhiteboardIssuesFolder)
                .Returns(fakeIssuesFolder);
        }

        [Test]
        public void CreateSitecoreItem_GetsIssueName_CallsCreateWithIssueFolderAsParent()
        {
            // ARRANGE
            SetUpFakeIssuesFolder();
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.InvokeDelegate<Func<IIssue>>());

            var model = new IssueModel
            {
                Title = "Antlers Up Vol 6"
            };

            // ACT
            _issuesService.CreateIssueItem<IIssue, IIssue_Folder>(model.Title, Constants.VirtualWhiteboardIssuesFolder);

            // ASSERT
            _dependencies.SitecoreServiceMaster.Received(1).Create<IIssue, IIssue_Folder>(
                Arg.Is<IIssue_Folder>(x => x._Id == new Guid(Constants.VirtualWhiteboardIssuesFolder)), "Antlers Up Vol 6");
        }

        [Test]
        public void CreateSitecoreItem_GetsIssueName_ReturnsNewId()
        {
            // ARRANGE
            SetUpFakeIssuesFolder();

            var fakeIssue = Substitute.For<IIssue>();
            fakeIssue._Id = new Guid("{67864C37-12DE-42FF-8F1E-52ECECAE9214}");
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Any<Func<IIssue>>())
                .Returns(fakeIssue);

            // ACT
            var result = _issuesService.CreateIssueItem<IIssue, IIssue_Folder>("Snake Issssssssue", Constants.VirtualWhiteboardIssuesFolder);

            // ASSERT
            Assert.AreEqual(new Guid("{67864C37-12DE-42FF-8F1E-52ECECAE9214}"), result);
        }

        [Test]
        public void UpdateIssueModel_GetModel_CallsSCSaveWithModelProperties()
        {
            // ARRANGE
            var model = new IssueModel
            {
                Title = "Antlers Up Vol 6",
                PublishedDate = new DateTime(123456789),
                Notes = "This issue is all about what household appliance moose might store in their antlers."
            };
            SetUpFakeIssuesFolder();
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());

            var id = new Guid("{5773462b-b659-4f2f-91ae-b65598da54a8}");
            var fakeIssue = Substitute.For<IIssue__Raw>();
            _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(id).Returns(fakeIssue);

            // ACT
            _issuesService.UpdateIssueItem(model, id);

            // ASSERT
            _dependencies.SitecoreServiceMaster.Received(1).Save(Arg.Is<IIssue__Raw>(issue =>
                issue.Title == "Antlers Up Vol 6"));
            _dependencies.SitecoreServiceMaster.Received(1).Save(Arg.Is<IIssue__Raw>(issue =>
                issue.Published_Date == new DateTime(123456789)));
            _dependencies.SitecoreServiceMaster.Received(1).Save(Arg.Is<IIssue__Raw>(issue =>
                issue.Notes == "This issue is all about what household appliance moose might store in their antlers."));
        }

	    [Test]
	    public void UpdateIssueItem_NoMatchingIssueFound_ThrowsExceptionWithBadId()
	    {
		    // ARRANGE
            var badGuid = new Guid("E6008282-9ABC-4FF8-B618-5862CD2955A3");
		    _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(Arg.Any<Guid>()).Returns((IIssue__Raw)null);

		    // ACT & ASSERT
	        var ex = Assert.Throws<Exception>(() => _issuesService.UpdateIssueItem(null, badGuid));
            Assert.IsTrue(ex.Message.Contains(badGuid.ToString()));
	    }	

        [Test]
        public void AddArticlesToIssue_GetsArticleGuids_CallsCloneForEach()
        {
            // ARRANGE
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());

            var fakeIssue = Substitute.For<IIssue>();
            fakeIssue._Id.Returns(new Guid("{45D643CB-9F39-417E-900D-ECEAD18B2189}"));
            var fakeId1 = new Guid("{F1F1C2C5-A4B4-49C4-BC68-D155B309D2F8}");
            var fakeId2 = new Guid("{EA514FAA-522A-4CC3-9117-98DC7A2A0425}");

            var articles = new[] {fakeId1, fakeId2};

            // ACT
            _issuesService.AddArticlesToIssue(fakeIssue._Id, articles);

            // ASSERT
            _dependencies.SitecoreClonesWrapper.Received(1).CreateClone(fakeId1,
                new Guid("{45D643CB-9F39-417E-900D-ECEAD18B2189}"));
            _dependencies.SitecoreClonesWrapper.Received(1).CreateClone(fakeId2,
                new Guid("{45D643CB-9F39-417E-900D-ECEAD18B2189}"));
        }

        [Test]
        public void CreateIssueFromModel_GetFullModel_CreatesItemAndClones()
        {
            // ARRANGE
            SetUpFakeIssuesFolder();
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.InvokeDelegate<Func<IIssue>>());
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());

            var model = new IssueModel
            {
                Title = "Moose Monthly",
                PublishedDate = new DateTime(123456789),
                ArticleIds = new[]
                {
                    new Guid("{F1F1C2C5-A4B4-49C4-BC68-D155B309D2F8}"),
                    new Guid("{EA514FAA-522A-4CC3-9117-98DC7A2A0425}")
                }
            };

            // ACT
            var response = _issuesService.CreateIssueFromModel(model);

            // ASSERT
            _dependencies.SitecoreServiceMaster.Received(1)
                .Create<IIssue, IIssue_Folder>(Arg.Any<IIssue_Folder>(), Arg.Any<string>());
            _dependencies.SitecoreServiceMaster.Received(1).Save(Arg.Any<IIssue__Raw>());
            _dependencies.SitecoreClonesWrapper.Received(2).CreateClone(Arg.Any<Guid>(), Arg.Any<Guid>());

            Assert.IsTrue(response.IsSuccess);
        }

        [Test]
        public void CreateIssueFromModel_GetModelThatErrors_ReturnsExceptionMessage()
        {
            // ARRANGE
            SetUpFakeIssuesFolder();
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.InvokeDelegate<Func<IIssue>>());
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());

            var model = new IssueModel
            {
                Title = "Moose Monthly",
                PublishedDate = new DateTime(123456789),
                ArticleIds = new[]
                {
                    new Guid("{F1F1C2C5-A4B4-49C4-BC68-D155B309D2F8}"),
                    new Guid("{EA514FAA-522A-4CC3-9117-98DC7A2A0425}")
                }
            };

            _dependencies.SitecoreServiceMaster.GetItem<IIssue_Folder>(Constants.VirtualWhiteboardIssuesFolder)
                .Returns(x => { throw new Exception("Moose have eaten all the data."); });

            // ACT
            var response = _issuesService.CreateIssueFromModel(model);

            // ASSERT
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("Moose have eaten all the data.", response.DebugErrorMessage);
        }

	    [Test]
	    public void ArchiveIssue_NoIssueFound_ReturnFail()
	    {
			// ARRANGE
			var id = new Guid("2603A6E4-6591-4D1E-80E0-05A6FA63FA22");
			_dependencies.SitecoreServiceMaster.GetItem<IIssue>(id).Returns((IIssue)null);

			// ACT
			var result = _issuesService.ArchiveIssue(id);

		    // ASSERT
			Assert.IsFalse(result.IsSuccess);
	    }

	    [Test]
	    public void ArchiveIssue_NoIssueCreated_ReturnFail()
	    {
		    // ARRANGE
		    _dependencies.SitecoreServiceMaster.GetItem<IArchived_Issue__Raw>(Arg.Any<Guid>()).Returns((IArchived_Issue__Raw) null);

		    // ACT
		    var result = _issuesService.ArchiveIssue(Arg.Any<Guid>());

		    // ASSERT
			Assert.IsFalse(result.IsSuccess);
	    }

	    [Test]
	    public void ArchiveIssue_IssueArchived_ReturnSuccess()
	    {
		    // ARRANGE

		    // ACT
		    var result = _issuesService.ArchiveIssue(Arg.Any<Guid>());

		    // ASSERT
			Assert.IsTrue(result.IsSuccess);
	    }

	    [Test]
	    public void DeleteArticles_IdString_DeleteArticles()
	    {
			// ARRANGE
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
			var idString =
			    "4137ED60-1ABE-42CB-B305-119C05B696D3|E94F2832-A519-48AA-9413-FF823BF710E9|8C268BA6-9AE0-4D1E-A16A-24F5B22648F8";

			// ACT
			_issuesService.DeleteArticles(idString);

		    // ASSERT
			_dependencies.SitecoreServiceMaster.Received(3).Delete(Arg.Any<IArticle>());
	    }

		[Test]
		public void DeleteArticles_SingleIdString_DeleteSpecificId()
		{
			// ARRANGE
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
			var fakeArticle = Substitute.For<IArticle>();
			fakeArticle._Id.Returns(new Guid("4137ED60-1ABE-42CB-B305-119C05B696D3"));
			_dependencies.SitecoreServiceMaster.GetItem<IArticle>(Arg.Any<Guid>()).Returns(fakeArticle);
			var idString = "4137ED60-1ABE-42CB-B305-119C05B696D3";

			// ACT
			_issuesService.DeleteArticles(idString);

			// ASSERT
			_dependencies.SitecoreServiceMaster.Received(1).Delete(Arg.Is<IArticle>(a => a._Id == new Guid("4137ED60-1ABE-42CB-B305-119C05B696D3")));
		}

	    [Test]
	    public void DeleteArticles_EmptyString_Returned()
	    {
			// ARRANGE
			_dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
			var idString = "";

			// ACT
			_issuesService.DeleteArticles(idString);

			// ASSERT
			_dependencies.SitecoreServiceMaster.DidNotReceive().Delete(Arg.Any<IArticle>());
		}

	    

	    [Test]
	    public void GetArticles_IssueIsNull_GetEmptyResult()
	    {
		    // ARRANGE
		    _dependencies.SitecoreServiceMaster.GetItem<IIssue>(Arg.Any<Guid>()).Returns((IIssue)null);

		    // ACT
		    var result = _issuesService.GetArticles(Arg.Any<Guid>());

		    // ASSERT
			Assert.IsEmpty(result);
	    }

		[Test]
		public void GetArticles_IssueIsEmpty_GetEmptyResult()
		{
			// ARRANGE
			var issue = Substitute.For<IIssue>();
			_dependencies.SitecoreServiceMaster.GetItem<IIssue>(Arg.Any<Guid>()).Returns(issue);

			// ACT
			var result = _issuesService.GetArticles(Arg.Any<Guid>());

			// ASSERT
			Assert.IsEmpty(result);
		}

	    [Test]
	    public void GetArticles_EmptyArticlesOrder_ReturnArticles()
	    {
		    // ARRANGE
		    var issue = Substitute.For<IIssue>();
		    _dependencies.SitecoreServiceMaster.GetItem<IIssue>(Arg.Any<Guid>()).Returns(issue);
		    var children = Substitute.For<IEnumerable<IGlassBase>>();
		    issue._ChildrenWithInferType.Returns(children);

		    // ACT
		    var result = _issuesService.GetArticles(Arg.Any<Guid>());

		    // ASSERT
			Assert.IsInstanceOf<IEnumerable<IArticle>>(result);
	    }

	    [Test]
	    public void UpdateIssueInfo_AllValidInput_UpdateTitleAndDateForIssue()
	    {
		    // ARRANGE
		    var issue = Substitute.For<IIssue__Raw>();
		    _dependencies.SitecoreServiceMaster.GetItem<IIssue__Raw>(Arg.Any<Guid>()).Returns(issue);
		    var title = "title";
		    var date = new DateTime(2016,6,21);
		    _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
		    // ACT
			_issuesService.UpdateIssueInfo(Arg.Any<Guid>(), title, date.ToString(), string.Empty);

		    // ASSERT
			Assert.AreEqual(issue.Title, title);
			Assert.AreEqual(issue.Published_Date, date);
			_dependencies.SitecoreServiceMaster.Received(1).Save(issue);
	    }

	    [Test]
	    public void ReorderArticles_ArticleOrderedIdsAndCouldntFindMatchingArticles_NothingHappened()
	    {
		    // ARRANGE
		    _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
		    _dependencies.SitecoreServiceMaster.GetItem<IArticle__Raw>(Arg.Any<Guid>()).Returns((IArticle__Raw) null);

		    // ACT
			_issuesService.ReorderArticles(string.Empty);

		    // ASSERT
			_dependencies.SitecoreServiceMaster.DidNotReceive().Save(Arg.Any<IArticle__Raw>());
	    }

	    [Test]
	    public void ReorderArticles_OrderedIdsAndArticlesAreFoundInSitecore_SetSortOrderAndSave()
	    {
			// ARRANGE
			var idString = "5CCCFC14-DAAC-42ED-99BA-289DA08742C0|6831C1D6-5089-44E8-8158-A6CE55E4AE8C";
		    float order = 100;
		    var article1 = Substitute.For<IArticle__Raw>();
		    article1.Sortorder = 800;
		    var article2 = Substitute.For<IArticle__Raw>();
		    article2.Sortorder = 900;
			_dependencies.SitecoreServiceMaster.GetItem<IArticle__Raw>(new Guid("5CCCFC14-DAAC-42ED-99BA-289DA08742C0")).Returns(article1);
			_dependencies.SitecoreServiceMaster.GetItem<IArticle__Raw>(new Guid("6831C1D6-5089-44E8-8158-A6CE55E4AE8C")).Returns(article2);
		    _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());
			// ACT
			_issuesService.ReorderArticles(idString);

			// ASSERT
			Assert.AreEqual(order,article1.Sortorder);
			Assert.AreEqual(++order,article2.Sortorder);
		}
	}
}