using Informa.Library.Services.AccountManagement;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.References;
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
        IItemReferences ItemReferences;
        IUserEntitlementsContext EntitlementsContext;
        ISiteRootsContext SiteRoots;
        I___BasePage BasePage;

        [SetUp]
        public void SetUp()
        {
            AuthUserContext = Substitute.For<IAuthenticatedUserContext>();
            ItemReferences = Substitute.For<IItemReferences>();
            EntitlementsContext = Substitute.For<IUserEntitlementsContext>();
            SiteRoots = Substitute.For<ISiteRootsContext>();
            AccountManagementService = new AccountManagementService(AuthUserContext, ItemReferences, EntitlementsContext, SiteRoots);
            GeneralPage = Substitute.For<IGeneral_Content_Page>();
        }

        [Test]
        public void GeneralPage_Restricted_Test()
        {
            GeneralPage.Restrict_Access = ItemReferences.FreeWithEntitlement;
            
            bool b = AccountManagementService.IsRestricted(GeneralPage);
            
            Assert.IsTrue(b);
        }

        [Test]
        public void GeneralPage_Unrestricted_Test() 
        {
            GeneralPage.Restrict_Access = ItemReferences.FreeWithRegistration;
            
            bool b = AccountManagementService.IsRestricted(GeneralPage);

            Assert.IsTrue(b);
        }

        [Test]
        public void NonGeneralPage_Test()
        {
            bool b = AccountManagementService.IsRestricted(BasePage);

            Assert.IsFalse(b);
        }
    }
}