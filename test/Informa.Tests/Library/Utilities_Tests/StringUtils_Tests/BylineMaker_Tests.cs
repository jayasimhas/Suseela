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
	}
}