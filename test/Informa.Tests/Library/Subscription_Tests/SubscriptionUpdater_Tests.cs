using System.Collections.Generic;
using System.Linq;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Newsletter.EmailOptIns;
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
            _dependencies.SitePublicationContext.Name.Returns("MooseMagazine");
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
            _dependencies.SitePublicationContext.Name.Returns("MooseMagazine");
            _dependencies.NewsletterUserOptInFactory.Create("MooseMagazine", false)
                .Returns(new NewsletterUserOptIn { OptIn = false });

            // ACT
            var result = _optInManager.OptOut("Anthony", "newsletter", "MooseMagazine");

            // ASSERT
            _dependencies.UpdateNewsletterUserOptInsContext.Received(1)
                .Update(Arg.Is<IEnumerable<INewsletterUserOptIn>>(opts => opts.First().OptIn == false));
        }
    }
}