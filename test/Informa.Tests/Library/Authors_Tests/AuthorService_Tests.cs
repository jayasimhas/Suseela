using Informa.Library.Authors;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.Authors_Tests
{

    [TestFixture]
    public class AuthorService_Tests
    {
        AuthorService _authorService;
        AuthorService.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<AuthorService.IDependencies>();
            _authorService = new AuthorService(_dependencies);
        }

        [Test]
        public void ConvertAuthorNameToUrlName_GetsNameWithSpaces_ReturnsNameWithHyphensLower()
        {
            // ARRANGE

            // ACT
            var result = _authorService.ConvertAuthorNameToUrlName("Moose MacGuire");

            // ASSERT
            Assert.AreEqual("moose-macguire", result);
        }

        [Test]
        public void ConvertAuthorNameToUrlName_GetsWeirdName_ReturnsCleanUrlName()
        {
            // ARRANGE

            // ACT
            var result = _authorService.ConvertAuthorNameToUrlName("!!Ja7zzy!  !Br00ke...");

            // ASSERT
            Assert.AreEqual("ja-zzy-br-ke", result);
        }
    }
}