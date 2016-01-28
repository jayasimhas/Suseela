using System;
using System.Collections.Generic;
using Informa.Web.Areas.Account.Models;   
using Informa.Models.FactoryInterface;

namespace Informa.Web.Controllers
{
	public static class SitecoreUtil
	{
		//TODO: Business logic for Article Number Generation
		/// <summary>
		/// This method Generates the Article Number
		/// </summary>
		/// <param name="article"></param>
		/// <param name="publication"></param>
		/// <param name="articleDate"></param>
		/// <returns></returns>
		public static string GetNextArticleNumber(string article, Guid publication, DateTime articleDate)
		{
			string yymmdd = $"{articleDate:yyMMdd}";
			string prefix = GetPublicationPrefix(publication) + yymmdd;
			string number = prefix + article;
			return number;
		}

		/// <summary>
		/// This method gets the Publication Prefix which is used in Article Number Generation.
		/// </summary>
		/// <param name="publicationGuid"></param>
		/// <returns></returns>
		public static string GetPublicationPrefix(Guid publicationGuid)
		{
			var publicationPrefixDictionary =
				new Dictionary<Guid, string>
					{
						{new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}"), "SC"},
					};

			string value;
			return publicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}		
	}
}