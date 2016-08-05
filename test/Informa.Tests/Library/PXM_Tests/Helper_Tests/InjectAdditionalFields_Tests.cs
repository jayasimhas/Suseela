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

        [SetUp]
        public void SetUp()
        {
            _injectAdditionalFields = new InjectAdditionalFields();
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
                "< Div class ='root' data-test=\"7\"    ><pre><h1 class=\"title\">Moose attacks on the rise!</h1></pre><pre><h2 class=\"subtitle\">j/k, lolz</h2></pre><p>Only safe while gurgling!</p></div>",
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
            Assert.AreEqual("<pre><h1 class=\"title\">Moose attacks on the rise!</h1></pre>", result);
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
                "<pre><h1 class=\"title\">Moose attacks on the rise!</h1></pre><pre><h2 class=\"subtitle\">j/k, lolz</h2></pre>",
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

            var fakeHtml = "<div class='root'></div>";

            // ACT
            var result = _injectAdditionalFields.InjectAuthors(fakeHtml, fakeArticle);

            // ASSERT
            Assert.AreEqual("<div class='root'><pre><h3 class='authors'>Joe Faker joe.faker@example.com</h3></pre></div>", result);
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

            var fakeHtml = "<  DIV width='3' clASS = \"Root\" data-fun='moose' ><p>Other stuff!</p></div>";

            // ACT
            var result = _injectAdditionalFields.InjectAuthors(fakeHtml, fakeArticle);

            // ASSERT
            Assert.AreEqual(
                "<  DIV width='3' clASS = \"Root\" data-fun='moose' ><pre><h3 class='authors'>Joe Faker joe.faker@example.com</h3></pre><p>Other stuff!</p></div>",
                result);
        }

        [Test]
        public void GetAuthorsFormatted_OneAuthor_AuthorNameAndEmail()
        {
            // ARRANGE
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Joe";
            fakeAuthor1.Last_Name = "Faker";
            fakeAuthor1.Email_Address = "joe.faker@example.com";

            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Authors.Returns(new[] {fakeAuthor1});

            // ACT
            var result = _injectAdditionalFields.GetAuthorsFormatted(fakeArticle);

            // ASSERT
            Assert.AreEqual("Joe Faker joe.faker@example.com", result);
        }

        [Test]
        public void GetAuthorsFormatted_ThreeAuthors_AuthorsSpaceSeparated()
        {
            // ARRANGE
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Joe";
            fakeAuthor1.Last_Name = "Faker";
            fakeAuthor1.Email_Address = "joe.faker@example.com";

            var fakeAuthor2 = Substitute.For<IStaff_Item>();
            fakeAuthor2.First_Name = "a2";
            fakeAuthor2.Last_Name = "d2";
            fakeAuthor2.Email_Address = "a2d2@example.com";

            var fakeAuthor3 = Substitute.For<IStaff_Item>();
            fakeAuthor3.First_Name = "a3";
            fakeAuthor3.Last_Name = "you sunk my battleship";
            fakeAuthor3.Email_Address = "direct_hit@battleship.com";

            var fakeArticle = Substitute.For<IArticle>();
            fakeArticle.Authors.Returns(new[] {fakeAuthor1, fakeAuthor2, fakeAuthor3});

            // ACT
            var result = _injectAdditionalFields.GetAuthorsFormatted(fakeArticle);

            // ASSERT
            Assert.AreEqual(
                "Joe Faker joe.faker@example.com a2 d2 a2d2@example.com a3 you sunk my battleship direct_hit@battleship.com",
                result);
        }

        [Test]
        public void GetAuthorsFormatted_ZeroAuthorsOrNull_EmptyString()
        {
            // ARRANGE
            var fakeArticle1 = Substitute.For<IArticle>();
            fakeArticle1.Authors.Returns(new IStaff_Item[0]);
            var fakeArticle2 = Substitute.For<IArticle>();
            fakeArticle2.Authors.Returns((IStaff_Item[]) null);

            // ACT
            var result1 = _injectAdditionalFields.GetAuthorsFormatted(fakeArticle1);
            var result2 = _injectAdditionalFields.GetAuthorsFormatted(fakeArticle2);

            // ASSERT
            Assert.AreEqual(string.Empty, result1);
            Assert.AreEqual(string.Empty, result2);
        }
    }
}