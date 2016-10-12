using System.Collections.Generic;
using Informa.Library.PXM.Helpers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.PXM_Tests.Helper_Tests
{

    [TestFixture]
    public class InjectAdditionalFields_Tests
    {
        InjectAdditionalFields _injectAdditionalFields;
        private InjectAdditionalFields.IDependencies _dependencies;


        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<InjectAdditionalFields.IDependencies>();
            _injectAdditionalFields = new InjectAdditionalFields(_dependencies);
        }

        [Test]
        public void InjectAfterRegex_OriginalAndInjectedAndRegex_InjectedWithinOriginal()
        {
            // ARRANGE
            var original = "Moose deemed ruling population of Canada";
            var inject = "- so majestic - ";
            var regex = "Moose\\s";

            // ACT
            var result = _injectAdditionalFields.InjectAfterRegex(regex, original, inject);

            // ASSERT
            Assert.AreEqual("Moose - so majestic - deemed ruling population of Canada", result);
        }

        [Test]
        public void InjectTitles_ArticleAndWeirdHtml_TitlesInjected()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Title = "Moose attacks on the rise!";
            fakeArticle.Sub_Title = "j/k, lolz";

            var initialHtml = "< Div class ='root' data-test=\"7\"    ><p>Only safe while gurgling!</p></div>";

            // ACT
            var result = _injectAdditionalFields.InjectTitles(initialHtml, fakeArticle);

            // ASSERT
            Assert.AreEqual(
                "< Div class ='root' data-test=\"7\"    ><h1 class=\"title\">Moose attacks on the rise!</h1><h2 class=\"subtitle\">j/k, lolz</h2><p>Only safe while gurgling!</p></div>",
                result);
        }

        [Test]
        public void GetTitlesHtml_TitleOnly_PreWithTitle()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Title = "Moose attacks on the rise!";

            // ACT
            var result = _injectAdditionalFields.GetTitlesHtml(fakeArticle);

            // ASSERT
            Assert.AreEqual("<h1 class=\"title\">Moose attacks on the rise!</h1>", result);
        }

        [Test]
        public void GetTitlesHtml_TitleAndSubtitle_TwoPres()
        {
            // ARRANGE
            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Title = "Moose attacks on the rise!";
            fakeArticle.Sub_Title = "j/k, lolz";

            // ACT
            var result = _injectAdditionalFields.GetTitlesHtml(fakeArticle);

            // ASSERT
            Assert.AreEqual(
                "<h1 class=\"title\">Moose attacks on the rise!</h1><h2 class=\"subtitle\">j/k, lolz</h2>",
                result);
        }

        [Test]
        public void InjectAuthors_HtmlWithRoot_PlacesAuthorAfterRoot()
        {
            // ARRANGE
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Joe";
            fakeAuthor1.Last_Name = "Faker";
            fakeAuthor1.Email_Address = "joe.faker@example.com";

            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Authors.Returns(new[] { fakeAuthor1 });

            _dependencies.BylineMaker.MakePrintByLine(Arg.Any<IEnumerable<IStaff_Item>>())
                .Returns("Joe Faker joe.faker@example.com");

            var fakeHtml = "<div class='root'></div>";

            // ACT
            var result = _injectAdditionalFields.InjectAuthors(fakeHtml, fakeArticle);

            // ASSERT
            Assert.AreEqual("<div class='root'><h3 class='authors'>Joe Faker joe.faker@example.com</h3></div>", result);
        }

        [Test]
        public void InjectAuthors_WeirderHtml_PlacesAuthorAfterRoot()
        {
            // ARRANGE
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Joe";
            fakeAuthor1.Last_Name = "Faker";
            fakeAuthor1.Email_Address = "joe.faker@example.com";

            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Authors.Returns(new[] { fakeAuthor1 });

            _dependencies.BylineMaker.MakePrintByLine(Arg.Any<IEnumerable<IStaff_Item>>())
                .Returns("Joe Faker joe.faker@example.com");

            var fakeHtml = "<  DIV width='3' clASS = \"Root\" data-fun='moose' ><p>Other stuff!</p></div>";

            // ACT
            var result = _injectAdditionalFields.InjectAuthors(fakeHtml, fakeArticle);

            // ASSERT
            Assert.AreEqual(
                "<  DIV width='3' clASS = \"Root\" data-fun='moose' ><h3 class='authors'>Joe Faker joe.faker@example.com</h3><p>Other stuff!</p></div>",
                result);
        }
    }
}