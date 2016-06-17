using System;
using Informa.Library.Utilities.References;
using Informa.Library.VirtualWhiteboard;
using Informa.Library.VirtualWhiteboard.Models;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Virtual_Whiteboard;
using NSubstitute;
using NUnit.Framework;

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
            _issuesService.CreateIssueItem(model.Title);

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
            var result = _issuesService.CreateIssueItem("Snake Issssssssue");

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
            _dependencies.SitecoreClonesWrapper.Received(1).CreateClone(
                new Guid("{45D643CB-9F39-417E-900D-ECEAD18B2189}"), fakeId1);
            _dependencies.SitecoreClonesWrapper.Received(1).CreateClone(
                new Guid("{45D643CB-9F39-417E-900D-ECEAD18B2189}"), fakeId2);
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

    }
}