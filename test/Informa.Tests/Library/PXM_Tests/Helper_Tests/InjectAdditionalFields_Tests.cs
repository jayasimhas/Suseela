using Informa.Library.PXM.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace Informa.Tests.Library.PXM_Tests.Helper_Tests
{

    [TestFixture]
    public class InjectAdditionalFields_Tests
    {
        InjectAdditionalFields _injectAdditionalFields;
        InjectAdditionalFields.IDependencies _dependencies;

        [SetUp]
        public void SetUp()
        {
            _dependencies = Substitute.For<InjectAdditionalFields.IDependencies>();
            _injectAdditionalFields = new InjectAdditionalFields(_dependencies);
        }

        [Test]
        public void MethodName_Input_Expectation()
        {
            // ARRANGE

            // ACT

            // ASSERT

        }
    }
}