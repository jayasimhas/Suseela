using System;
using FuelSDK;
using Informa.Library.Mail.ExactTarget;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Mail_Tests
{
    [TestFixture]
    public class ExactTargetClient_Tests
    {
        ExactTargetClient _exactTargetClient;
        ExactTargetClient.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<ExactTargetClient.IDependencies>();
            _exactTargetClient = new ExactTargetClient(_dependencies);
        }

        [Test]
        public void PushEmail_NullInput_LogsExitOnNull()
        {
            // ARRANGE

            // ACT
            _exactTargetClient.PushEmail(null);

            // ASSERT
            _dependencies.LogWrapper.Received(1).SitecoreWarn(Arg.Any<string>());
        }

        [Test]
        public void PopulateEtModel_GlassModelNoHtml_ReturnsEtModelSameData()
        {
            // ARRANGE
            var fakeEmail = Substitute.For<IExactTarget_Email>();
            fakeEmail._Parent = null;   //Needed to prevent Glass issue with Ancestor calls
            fakeEmail._Name = "EmailItem";
            fakeEmail.Subject = "Welcome to Mooseville";
            fakeEmail.Category_Id = 321;
            fakeEmail.Text_Body = "Dummy text";
            fakeEmail.Character_Set = "UTF-1960";
            _dependencies.WebClientWrapper.DownloadString(Arg.Any<string>()).Returns(string.Empty);

            // ACT
            FuelSDK.Email result = _exactTargetClient.PopulateEtModel(fakeEmail);

            // ASSERT
            Assert.AreEqual("EmailItem", result.Name);
            Assert.AreEqual("Welcome to Mooseville", result.Subject);
            Assert.AreEqual(321, result.CategoryID);
            Assert.AreEqual("Dummy text", result.TextBody);
            Assert.AreEqual("UTF-1960", result.CharacterSet);
            Assert.AreEqual(true, result.CategoryIDSpecified);

        }

        private IExactTarget_Email Sub_EmailItem()
        {
            var fakeEmailItem = Substitute.For<IExactTarget_Email>();
            fakeEmailItem._Id = new Guid("{2847ABC9-068C-412F-9D77-20F19A1A852B}");
            fakeEmailItem._Path.Returns("/sitecore/content/Mooseville/home/emails/MeetTheMoose");

            var fakeRootNode = Substitute.For<ISite_Root>();
            fakeRootNode._TemplateId.Returns(ISite_RootConstants.TemplateId.ToGuid());
            fakeRootNode._Path.Returns("/sitecore/content/Mooseville");
            var fakeParent = Substitute.For<IGlassBase>();
            fakeParent._Parent.Returns(fakeRootNode);
            fakeEmailItem._Parent.Returns(fakeParent);
            fakeRootNode._Parent = null;

            var fakeHomeNode = Substitute.For<IHome_Page>();
            fakeHomeNode._TemplateId.Returns(IHome_PageConstants.TemplateId.ToGuid());
            fakeHomeNode._Path.Returns("/sitecore/content/Mooseville/home");
            fakeRootNode._ChildrenWithInferType.Returns(new[] { fakeHomeNode });

            return fakeEmailItem;
        }

        private void Sub_CreateResult()
        {
            var fakeCreateResponse = new ExactTargetResponse
            {
                ExactTargetEmailId = 101,
                Success = true,
                Message = "fun was had by all"

            };
            _dependencies.ExactTargetWrapper.CreateEmail(Arg.Any<ET_Email>()).Returns(fakeCreateResponse);
        }

        [Test]
        public void PushEmail_EmailItemHasNoETEmailId_CreateNewEtEmail()
        {
            // ARRANGE
            var fakeEmailItem = Sub_EmailItem();
            fakeEmailItem.Exact_Target_External_Key = 0;
            Sub_CreateResult();

            _dependencies.SitecoreUrlWrapper.GetItemUrl(fakeEmailItem)
                .Returns("http://Mooseville.com/emails/MeetTheMoose");
            _dependencies.WebClientWrapper.DownloadString("http://Mooseville.com/emails/MeetTheMoose")
                .Returns("<div><h1>Moose</h1></div>");
            _dependencies.Premailer.InlineCss(Arg.Any<string>()).Returns(x => x.Arg<string>());

            // ACT
            _exactTargetClient.PushEmail(fakeEmailItem);

            // ASSERT
            _dependencies.ExactTargetWrapper.Received(1)
                .CreateEmail(Arg.Is<ET_Email>(email => email.HTMLBody == "<div><h1>Moose</h1></div>"));
        }

        [Test]
        public void PushEmail_EmailItemHasETEmailId_CallUpdateEmail()
        {
            // ARRANGE
            var fakeEmailItem = Sub_EmailItem();
            fakeEmailItem.Exact_Target_External_Key = 77;

            var fakeUpdateResponse = new ExactTargetResponse
            {
                ExactTargetEmailId = 77,
                Success = true,
                Message = "fun was had by all"
            };
            _dependencies.ExactTargetWrapper.UpdateEmail(Arg.Any<ET_Email>()).Returns(fakeUpdateResponse);
            _dependencies.SitecoreUrlWrapper.GetItemUrl(fakeEmailItem)
                .Returns("http://Mooseville.com/emails/MeetTheMoose");
            _dependencies.Premailer.InlineCss(Arg.Any<string>()).Returns(x => x.Arg<string>());

            _dependencies.WebClientWrapper.DownloadString("http://Mooseville.com/emails/MeetTheMoose")
                .Returns("<div><h1>Moose Update!</h1></div>");

            // ACT
            _exactTargetClient.PushEmail(fakeEmailItem);

            // ASSERT
            _dependencies.ExactTargetWrapper.Received(1)
                .UpdateEmail(Arg.Is<ET_Email>(email => email.HTMLBody == "<div><h1>Moose Update!</h1></div>"));
        }

        [Test]
        public void PushEmail_EmailItemHasEtEmailId_SaveEtEmailIdAfterCreate()
        {
            // ARRANGE
            var fakeEmailItem = Sub_EmailItem();
            fakeEmailItem.Exact_Target_External_Key = 0;
            Sub_CreateResult();

            _dependencies.SitecoreUrlWrapper.GetItemUrl(fakeEmailItem).Returns("http://Mooseville.com/emails/MeetTheMoose");
            _dependencies.WebClientWrapper.DownloadString("http://Mooseville.com/emails/MeetTheMoose")
                .Returns("<div><h1>Moose</h1></div>");

            _dependencies.Premailer.InlineCss(Arg.Any<string>()).Returns(x => x.Arg<string>());
            _dependencies.SitecoreSecurityWrapper.WithSecurityDisabled(Arg.Invoke());


            // ACT
            _exactTargetClient.PushEmail(fakeEmailItem);

            // ASSERT
            _dependencies.SitecoreSecurityWrapper.Received(1).WithSecurityDisabled(Arg.Any<Action>());
            _dependencies.SitecoreServiceMaster.Received(1)
                .Save(Arg.Is<IExactTarget_Email>(email => email.Exact_Target_External_Key == 101));
        }

        [Test]
        public void PushEmail_UpdateEmailFails_WarnWithMessage()
        {
            // ARRANGE
            var fakeEmailItem = Sub_EmailItem();
            fakeEmailItem.Exact_Target_External_Key = 77;

            var fakeUpdateResponse = new ExactTargetResponse
            {
                ExactTargetEmailId = 77,
                Success = false,
                Message = "nobody had fun!"
            };
            _dependencies.ExactTargetWrapper.UpdateEmail(Arg.Any<ET_Email>()).Returns(fakeUpdateResponse);

            _dependencies.SitecoreUrlWrapper.GetItemUrl(fakeEmailItem).Returns("http://Mooseville.com/emails/MeetTheMoose");
            _dependencies.WebClientWrapper.DownloadString("http://Mooseville.com/emails/MeetTheMoose")
                .Returns("<div><h1>Moose Update!</h1></div>");

            _dependencies.Premailer.InlineCss(Arg.Any<string>()).Returns(x => x.Arg<string>());

            // ACT
            _exactTargetClient.PushEmail(fakeEmailItem);

            // ASSERT
            _dependencies.LogWrapper.Received().SitecoreWarn(Arg.Is<string>(s => s.Contains("nobody had fun!")));
        }

        [Test]
        public void GetEmailHtml_DownloadStringHasContent_CssInlinerIsCalled()
        {
            // ARRANGE
            var fakeEmailItem = Sub_EmailItem();
            fakeEmailItem.Exact_Target_External_Key = 77;

            var fakeUpdateResponse = new ExactTargetResponse
            {
                ExactTargetEmailId = 77,
                Success = false,
                Message = "nobody had fun!"
            };

            _dependencies.SitecoreUrlWrapper.GetItemUrl(fakeEmailItem)
                .Returns("http://Mooseville.com/emails/MeetTheMoose");

            _dependencies.WebClientWrapper.DownloadString("http://Mooseville.com/emails/MeetTheMoose")
                .Returns("<div><h1>Moose Update!</h1></div>");

            // ACT
            _exactTargetClient.GetEmailHtml(fakeEmailItem);

            // ASSERT
            _dependencies.Premailer.Received().InlineCss(Arg.Is<string>(s => s.Contains("Moose Update")));
        }
    }
}