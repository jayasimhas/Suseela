using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.EmailOptIns;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Search;
using Informa.Library.Utilities.References;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Subscription_Tests
{

    [TestFixture]
    public class SubscriptionUpdater_Tests
    {
        OptInManager _optInManager;
        OptInManager.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<OptInManager.IDependencies>();
            _optInManager = new OptInManager(_dependencies);
        }

        [Test]
        public void Subscribe_GetCurrentPublication_UpdateOptInTrue()
        {
            // ARRANGE
            _dependencies.AuthenticatedUserContext.IsAuthenticated.Returns(true);
            _dependencies.SitePublicationNameContext.Name.Returns("MooseMagazine");
            _dependencies.NewsletterUserOptInFactory.Create("MooseMagazine", true)
                .Returns(new NewsletterUserOptIn {OptIn = true});

            // ACT
            var result = _optInManager.OptIn("MooseMagazine");

            // ASSERT
            _dependencies.UpdateNewsletterUserOptInsContext.Received(1)
                .Update(Arg.Is<IEnumerable<INewsletterUserOptIn>>(opts => opts.First().OptIn == true));
        }

        [Test]
        public void Unsubscribe_GetCurrentPublication_UpdateOptInFalse()
        {
            // ARRANGE
            _dependencies.AuthenticatedUserContext.IsAuthenticated.Returns(true);
            _dependencies.SitePublicationNameContext.Name.Returns("MooseMagazine");
            _dependencies.NewsletterUserOptInFactory.Create("MooseMagazine", false)
                .Returns(new NewsletterUserOptIn { OptIn = false });

            // ACT
            var result = _optInManager.OptOut("Anthony", "newsletter", "MooseMagazine");

            // ASSERT
            _dependencies.UpdateNewsletterUserOptInsContext.Received(1)
                .Update(Arg.Is<IEnumerable<INewsletterUserOptIn>>(opts => opts.First().OptIn == false));
        }

        [Test]
        public void ParseToken_GetsToken_ReturnsSavedSearchEntity()
        {
            // ARRANGE
            var token = "encryptoFun";
            _dependencies.Crypto.DecryptStringAes("encryptoFun", Constants.CryptoKey)
                .Returns("Edward J Moose|moose tennis");

            // ACT
            var result = _optInManager.ParseToken(token);

            // ASSERT
            Assert.AreEqual("Edward J Moose", result.Username);
            Assert.AreEqual("moose tennis", result.Name);
        }

        [Test]
        public void AnnonymousOptOut_GetToken_CallsSavedSearchService()
        {
            // ARRANGE
            var token = "encryptoFun";
            _dependencies.Crypto.DecryptStringAes("encryptoFun", Constants.CryptoKey)
                .Returns("Edward J Moose|moose tennis");

            // ACT
            var result = _optInManager.AnnonymousOptOut(token);

            // ASSERT
            _dependencies.SavedSearchEntityRepository.Received(1)
                .Delete(
                    Arg.Is<ISavedSearchEntity>(
                        arg => arg.Username.Equals("Edward J Moose") && arg.Name.Equals("moose tennis")));
        }
    }
}