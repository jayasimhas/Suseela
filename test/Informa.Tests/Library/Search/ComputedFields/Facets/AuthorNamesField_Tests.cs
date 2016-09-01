using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Informa.Library.Search.ComputedFields.Facets;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

namespace Informa.Tests.Library.Search.ComputedFields.Facets
{
	[TestFixture]
	public class AuthorNamesField_Tests
	{
		private IStaff_Item staffItem;

		[SetUp]
		public void SetUp()
		{
			staffItem = Substitute.For<IStaff_Item>();
		}

		[Test]
		public void ToAuthorName_NullStaffItem_ReturnsEmptyString()
		{
			Assert.AreEqual(string.Empty, AuthorNamesField.ToAuthorName(null));
		}

		[Test]
		public void ToAuthorName_EmptyStaffItem_ReturnsEmptyString()
		{
			Assert.AreEqual(string.Empty, AuthorNamesField.ToAuthorName(staffItem));
		}

		[Test]
		public void ToAuthorName_OnlyFirstName_ReturnsFirstName()
		{
			staffItem.First_Name.Returns("John");
			Assert.AreEqual("John", AuthorNamesField.ToAuthorName(staffItem));
		}

		[Test]
		public void ToAuthorName_OnlyLastName_ReturnsLastName()
		{
			staffItem.Last_Name.Returns("Johnson");
			Assert.AreEqual("Johnson", AuthorNamesField.ToAuthorName(staffItem));
		}

		[Test]
		public void ToAuthorName_HasFullName_ReturnsFullName()
		{
			staffItem.First_Name.Returns("John");
			staffItem.Last_Name.Returns("Johnson");
			Assert.AreEqual("John Johnson", AuthorNamesField.ToAuthorName(staffItem));
		}
	}
}
