using Informa.Library.Mail.ExactTarget;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Emails;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Mail_Tests
{

    [TestFixture]
    public class CampaignQueryBuilder_Tests
    {
        CampaignQueryBuilder _campaignQueryBuilder;
        CampaignQueryBuilder.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<CampaignQueryBuilder.IDependencies>();
            _campaignQueryBuilder = new CampaignQueryBuilder(_dependencies);
        }

        [Test]
        public void AddCampaignQuery_Null_EmptyString()
        {
            // ARRANGE

            // ACT
            var result = _campaignQueryBuilder.AddCampaignQuery(null);

            // ASSERT
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void GetQuery_GetCampaign()
        {
            // ARRANGE
            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name.Returns("Fizzbat");
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            // ACT
            var result = _campaignQueryBuilder.GetCampaignQuery();

            // ASSERT
            Assert.IsTrue(result.StartsWith($"{Constants.QueryString.UtmCampaign}=Fizzbat"));
        }

        [Test]
        public void GetQuery_GetFullQuery()
        {
            // ARRANGE
            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name.Returns("Fizzbat");
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var fakeItem = Substitute.For<IExactTarget_Email>();
            fakeItem.Source.Returns("TheSky");
            fakeItem.Medium.Returns("OilOnCanvas");
            _dependencies.SitecoreContext.GetCurrentItem<IExactTarget_Email>().Returns(fakeItem);

            // ACT
            var result = _campaignQueryBuilder.GetCampaignQuery();

            // ASSERT
            Assert.AreEqual(
                $"{Constants.QueryString.UtmCampaign}=Fizzbat&{Constants.QueryString.UtmSource}=TheSky&{Constants.QueryString.UtmMedium}=OilOnCanvas",
                result);
        }

        [Test]
        public void AddCampaignQuery_LinkNoQuestionMark_ResultWithQuestionMark()
        {
            // ARRANGE
            var url = "moose.com";

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name.Returns("Fizzbat");
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var fakeItem = Substitute.For<IExactTarget_Email>();
            fakeItem.Source.Returns("TheSky");
            fakeItem.Medium.Returns("OilOnCanvas");
            _dependencies.SitecoreContext.GetCurrentItem<IExactTarget_Email>().Returns(fakeItem);

            // ACT
            var result = _campaignQueryBuilder.AddCampaignQuery(url);

            // ASSERT
            var fullQuery =
                $"{Constants.QueryString.UtmCampaign}=Fizzbat&{Constants.QueryString.UtmSource}=TheSky&{Constants.QueryString.UtmMedium}=OilOnCanvas";
            Assert.AreEqual($"moose.com?{fullQuery}", result);
        }

        [Test]
        public void AddCampaignQuery_LinkWithQuestionMark_ResultWithAmpersand()
        {
            // ARRANGE
            var url = "moose.com?p=1";

            var fakeRoot = Substitute.For<ISite_Root>();
            fakeRoot.Publication_Name.Returns("Fizzbat");
            _dependencies.SiteRootContext.Item.Returns(fakeRoot);

            var fakeItem = Substitute.For<IExactTarget_Email>();
            fakeItem.Source.Returns("TheSky");
            fakeItem.Medium.Returns("OilOnCanvas");
            _dependencies.SitecoreContext.GetCurrentItem<IExactTarget_Email>().Returns(fakeItem);

            // ACT
            var result = _campaignQueryBuilder.AddCampaignQuery(url);

            // ASSERT
            var fullQuery =
                $"{Constants.QueryString.UtmCampaign}=Fizzbat&{Constants.QueryString.UtmSource}=TheSky&{Constants.QueryString.UtmMedium}=OilOnCanvas";
            Assert.AreEqual($"moose.com?p=1&{fullQuery}", result);
        }
    }
}