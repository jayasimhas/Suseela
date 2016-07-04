using Informa.Library.Advertisement;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Advertisement_Tests
{

    [TestFixture]
    public class AdUrlBuilder_Tests
    {
        AdUrlBuilder _adUrlBuilder;
        AdUrlBuilder.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<AdUrlBuilder.IDependencies>();
            _adUrlBuilder = new AdUrlBuilder(_dependencies);
        }

        private IAdvertisement FakeAd() =>
            Substitute.For<IAdvertisement>()
                .Alter(ad => ad.Slot_ID = "123")
                .Alter(ad => ad.Zone = "cali");

        private void BindFakeSite()
        {
            var fakeSite = Substitute.For<ISite_Root>();
            fakeSite.Ad_Domain.Returns("www.moose4moose.com");
            _dependencies.SiteRootContext.Item.Returns(fakeSite);
        }

        [Test]
        public void GetLink_NoZone_EmptyResult()
        {
            // ARRANGE
            var fakeAd = FakeAd();
            fakeAd.Zone = null;

            BindFakeSite();

            // ACT
            var result = _adUrlBuilder.GetLink(fakeAd);

            // ASSERT
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void GetLink_NoSlotId_EmptyResult()
        {
            // ARRANGE
            var fakeAd = FakeAd();
            fakeAd.Slot_ID = "";

            BindFakeSite();

            // ACT
            var result = _adUrlBuilder.GetLink(fakeAd);

            // ASSERT
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void GetLink_GetImageSrc_LinkHasClick_ImageHasStream()
        {
            // ARRANGE
            var fakeAd = FakeAd();

            BindFakeSite();

            // ACT
            var link = _adUrlBuilder.GetLink(fakeAd);
            var imageSrc = _adUrlBuilder.GetImageSrc(fakeAd);

            // ASSERT
            Assert.IsTrue(link.Contains("click_nx.ads"));
            Assert.IsTrue(imageSrc.Contains("adstream_nx.ads"));
        }

        [Test]
        public void GetLink_NoAdDomain_EmptyResult()
        {
            // ARRANGE
            var fakeAd = FakeAd();

            var fakeSite = Substitute.For<ISite_Root>();
            fakeSite.Ad_Domain.Returns("");
            _dependencies.SiteRootContext.Item.Returns(fakeSite);

            // ACT
            var result = _adUrlBuilder.GetLink(fakeAd);

            // ASSERT
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void GetLink_GetImageSrc_AllVariable_FullLink()
        {
            // ARRANGE
            var fakeAd = FakeAd();

            BindFakeSite();

            // ACT
            var link = _adUrlBuilder.GetLink(fakeAd);
            var imageSrc = _adUrlBuilder.GetImageSrc(fakeAd);

            // ASSERT
            Assert.AreEqual(
                "http://oasc-eu1.247realmedia.com/RealMedia/ads/click_nx.ads/www.moose4moose.com/cali/11001%40123",
                link);
            Assert.AreEqual(
                "http://oasc-eu1.247realmedia.com/RealMedia/ads/adstream_nx.ads/www.moose4moose.com/cali/11001%40123",
                imageSrc);
        }
    }
}
