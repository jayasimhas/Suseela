using System;
using System.Text;
using System.Text.RegularExpressions;
using Castle.Core.Logging;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.SharedSource.DataImporter.PostProcess;

namespace Informa.Tests.DataImporter
{

    [TestFixture]
    public class InformaPostProcess_Tests
    {
        InformaPostProcess _classUnderTest;
        
        [SetUp]
        public void SetUp()
        {
            Sitecore.SharedSource.DataImporter.Logger.ILogger l = Substitute.For<Sitecore.SharedSource.DataImporter.Logger.ILogger>();
            _classUnderTest = new InformaPostProcess(l);
        }

        protected ImportSearchResultItem NullResult(Match match)
        {
            var result = new ImportSearchResultItem();
            result.NewArticleNumber = "";
            result.LegacyArticleNumber = "";
            result.ItemId = new ID("");

            return result;
        }

        [Test]
        public void UpdateArticleReferences_Null_Text()
        {
            // ARRANGE
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, null, string.Empty, NullResult);

            Assert.AreEqual(string.Empty, result.Key);
            Assert.AreEqual(string.Empty, result.Value);
        }
        
        [Test]
        public void UpdateArticleReferences_Null_References() {
            
            // ARRANGE
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, string.Empty, null, NullResult);

            Assert.AreEqual(string.Empty, result.Key);
            Assert.AreEqual(string.Empty, result.Value);
        }

        protected ImportSearchResultItem Result1
        {
            get
            {
                var result = new ImportSearchResultItem();
                result.NewArticleNumber = "SC0001";
                result.LegacyArticleNumber = "1111";
                result.ItemId = new ID("{11111111-1111-1111-1111-111111111111}");

                return result;
            }
        }
        protected ImportSearchResultItem Result2
        {
            get
            {
                var result = new ImportSearchResultItem();
                result.NewArticleNumber = "SC0002";
                result.LegacyArticleNumber = "2222";
                result.ItemId = new ID("{22222222-2222-2222-2222-222222222222}");

                return result;
            }
        }

        protected ImportSearchResultItem GetResult(Match match)
        {
            return (match.Value.Contains("2222"))
                ? Result2
                : Result1;
        }

        [Test]
        public void UpdateArticleReferences_SingleReference() {

            // ARRANGE
            string textValue = "[A#1111]";
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, textValue, string.Empty, GetResult);

            Assert.IsTrue(result.Key.Contains(string.Format(_classUnderTest.NewLinkFormat, Result1.NewArticleNumber)));
            Assert.IsTrue(!result.Key.Contains(Result1.LegacyArticleNumber));
            Assert.IsTrue(result.Value.Equals(Result1.ItemId.ToString()));
        }

        [Test]
        public void UpdateArticleReferences_DuplicateReferences() {

            // ARRANGE
            string textValue = "[A#1111] [A#1111]";
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, textValue, string.Empty, GetResult);

            Assert.IsTrue(result.Key.Contains(string.Format(_classUnderTest.NewLinkFormat, Result1.NewArticleNumber)));
            Assert.IsTrue(!result.Key.Contains(Result1.LegacyArticleNumber));
            Assert.IsTrue(result.Value.Equals(Result1.ItemId.ToString()));
        }
        
        [Test]
        public void UpdateArticleReferences_MultipleReferences()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("(<a>[A#1111]</a>)");
            sb.Append("(<a href=\"somelink.aspx?a=b\" target=\"_blank\">[A#1111]</a>)");
            sb.Append("<a>[A#1111]</a>");
            sb.Append("<a href=\"somelink.aspx?a=b\" target=\"_blank\">[A#1111]</a>");
            sb.Append("[A#1111]");
            sb.Append("[A#2222]");

            // ARRANGE
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, sb.ToString(), string.Empty, GetResult);

            //does contain new link
            Assert.IsTrue(result.Key.Contains(string.Format(_classUnderTest.NewLinkFormat, Result1.NewArticleNumber)));
            Assert.IsTrue(result.Key.Contains(string.Format(_classUnderTest.NewLinkFormat, Result2.NewArticleNumber)));

            //doesn't contain old number
            Assert.IsTrue(!result.Key.Contains(Result1.LegacyArticleNumber));
            Assert.IsTrue(!result.Key.Contains(Result2.LegacyArticleNumber));

            //the number of results is correct
            Assert.IsTrue(result.Value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Length.Equals(2));
        }

        protected ImportSearchResultItem GetNullMatch(Match match) {
            return null;
        }

        [Test]
        public void UpdateArticleReferences_Null_Match() {

            // ARRANGE
            string textValue = "[A#1111]";
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, textValue, string.Empty, GetNullMatch);

            Assert.AreEqual(textValue, result.Key);
            Assert.AreEqual(string.Empty, result.Value);
        }
        
        protected ImportSearchResultItem GetNullArticleNumber(Match match) {
            var result = new ImportSearchResultItem();
            result.NewArticleNumber = null;
            result.LegacyArticleNumber = "3333";
            result.ItemId = new ID("{33333333-3333-3333-3333-333333333333}");

            return result;
        }

        [Test]
        public void UpdateArticleReferences_Null_Match_ArticleNumber() {

            // ARRANGE
            string textValue = "[A#1111]";
            var result = _classUnderTest.UpdateArticleReferences(_classUnderTest.LinkPatterns, textValue, string.Empty, GetNullArticleNumber);

            Assert.AreEqual(textValue, result.Key);
            Assert.AreEqual(string.Empty, result.Value);
        }
    }
}