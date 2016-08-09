using System.Linq;
using Informa.Library.Utilities.StringUtils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Utilities_Tests.StringUtils_Tests
{

    [TestFixture]
    public class BylineMaker_Tests
    {
        BylineMaker _bylineMaker;
        BylineMaker.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<BylineMaker.IDependencies>();
            _bylineMaker = new BylineMaker(_dependencies);
        }

        [Test]
        public void MakeByline_NoAuthors_ReturnsEmptyString()
        {
            // ARRANGE
            var authors = Enumerable.Empty<IStaff_Item>();

            // ACT
            var result = _bylineMaker.MakeByline(authors);

            // ASSERT
            Assert.AreEqual(result, string.Empty);
        }

        [Test]
        public void MakeByline_OneAuthor_ReturnsByAuthor()
        {
            // ARRANGE
            _dependencies.TextTranslator.Translate("Article.By").Returns("THESE FOLKS:");
            var authorMcAuthorface = Substitute.For<IStaff_Item>();
            authorMcAuthorface.First_Name.Returns("Author");
            authorMcAuthorface.Last_Name.Returns("McAuthorface");
            var authors = new[] {authorMcAuthorface};

            // ACT
            var result = _bylineMaker.MakeByline(authors);

            // ASSERT
            Assert.AreEqual("THESE FOLKS: <a href=''>Author McAuthorface</a>", result);
        }

        [Test]
        public void MakeByline_TwoAuthors_ReturnsByAuthorAndAuthor()
        {
            // ARRANGE
            _dependencies.TextTranslator.Translate("Article.By").Returns("THESE FOLKS:");

            var authorMcAuthorface = Substitute.For<IStaff_Item>();
            authorMcAuthorface.First_Name.Returns("Author");
            authorMcAuthorface.Last_Name.Returns("McAuthorface");

            var bob = Substitute.For<IStaff_Item>();
            bob.First_Name.Returns("Bob");

            var authors = new[] { authorMcAuthorface, bob };

            // ACT
            var result = _bylineMaker.MakeByline(authors);

            // ASSERT
            Assert.AreEqual("THESE FOLKS: <a href=''>Author McAuthorface</a> and <a href=''>Bob </a>", result);
        }

        [Test]
        public void MakeByline_ThreeAuthors_ReturnsByAllDem()
        {
            // ARRANGE
            _dependencies.TextTranslator.Translate("Article.By").Returns("THESE FOLKS:");

            var authorMcAuthorface = Substitute.For<IStaff_Item>();
            authorMcAuthorface.First_Name.Returns("Author");
            authorMcAuthorface.Last_Name.Returns("McAuthorface");

            var bob = Substitute.For<IStaff_Item>();
            bob.First_Name.Returns("Bob");

            var alice = Substitute.For<IStaff_Item>();
            alice.First_Name.Returns("Alice");
            alice.Last_Name.Returns("in Wonderland");

            var authors = new[] { authorMcAuthorface, bob, alice };

            // ACT
            var result = _bylineMaker.MakeByline(authors);

            // ASSERT
            Assert.AreEqual("THESE FOLKS: <a href=''>Author McAuthorface</a>, <a href=''>Bob </a> and <a href=''>Alice in Wonderland</a>", result);
        }

		[Test]
		public void MakeByline_MultiAuthorsWithSuffixInLastNameField_ReturnsByAllDem()
		{
			// ARRANGE
			_dependencies.TextTranslator.Translate("Article.By").Returns("THESE FOLKS:");

			var authorMcAuthorface = Substitute.For<IStaff_Item>();
			authorMcAuthorface.First_Name.Returns("Author");
			authorMcAuthorface.Last_Name.Returns("McAuthorface");

			var bob = Substitute.For<IStaff_Item>();
			bob.First_Name.Returns("Bob");
			bob.Last_Name.Returns("McBob, Sr.");

			var authors = new[] { authorMcAuthorface, bob };

			// ACT
			var result = _bylineMaker.MakeByline(authors);

			// ASSERT
			Assert.AreEqual("THESE FOLKS: <a href=''>Author McAuthorface</a> and <a href=''>Bob McBob, Sr.</a>", result);
		}

        [Test]
        public void MakePrintByLine_OneAuthor_AuthorNameAndEmail()
        {
            // ARRANGE
            var fakeAuthor1 = Substitute.For<IStaff_Item>();
            fakeAuthor1.First_Name = "Joe";
            fakeAuthor1.Last_Name = "Faker";
            fakeAuthor1.Email_Address = "joe.faker@example.com";

            // ACT
            var result = _bylineMaker.MakePrintByLine(new[] {fakeAuthor1});

            // ASSERT
            Assert.AreEqual("Joe Faker joe.faker@example.com", result);
        }

        [Test]
        public void MakePrintByLine_ThreeAuthors_AuthorsSpaceSeparated()
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

            // ACT
            var result = _bylineMaker.MakePrintByLine(new[] { fakeAuthor1, fakeAuthor2, fakeAuthor3 });

            // ASSERT
            Assert.AreEqual(
                "Joe Faker joe.faker@example.com a2 d2 a2d2@example.com a3 you sunk my battleship direct_hit@battleship.com",
                result);
        }

        [Test]
        public void MakePrintByLine_ZeroAuthorsOrNull_EmptyString()
        {
            // ARRANGE

            // ACT
            var result1 = _bylineMaker.MakePrintByLine(new IStaff_Item[0]);
            var result2 = _bylineMaker.MakePrintByLine((IStaff_Item[])null);

            // ASSERT
            Assert.AreEqual(string.Empty, result1);
            Assert.AreEqual(string.Empty, result2);
        }
    }
}