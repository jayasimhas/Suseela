using Informa.Library.Services.AccountManagement;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Services.AccountManagement
{

    [TestFixture]
    public class AccountManagementService_Tests
    {
        AccountManagementService AccountManagementService;
        IAuthenticatedUserContext AuthUserContext;
        IGeneral_Content_Page GeneralPage;
        I___BasePage BasePage;

        [SetUp]
        public void SetUp()
        {
            AuthUserContext = Substitute.For<IAuthenticatedUserContext>();
            AccountManagementService = new AccountManagementService(AuthUserContext);
            GeneralPage = Substitute.For<IGeneral_Content_Page>();
        }

        [Test]
        public void GeneralPage_Restricted_Test()
        {
            GeneralPage.Restrict_To_Registered_Users = true;
            
            bool b = AccountManagementService.IsRestricted(GeneralPage);
            
            Assert.IsTrue(b);
        }

        [Test]
        public void GeneralPage_Unrestricted_Test() {
            GeneralPage.Restrict_To_Registered_Users = false;

            bool b = AccountManagementService.IsRestricted(GeneralPage);

            Assert.IsFalse(b);
        }

        [Test]
        public void NonGeneralPage_Test()
        {
            bool b = AccountManagementService.IsRestricted(BasePage);

            Assert.IsFalse(b);
        }
    }
}