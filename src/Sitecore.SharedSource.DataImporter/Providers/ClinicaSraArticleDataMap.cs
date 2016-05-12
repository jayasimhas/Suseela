using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class ClinicaSraArticleDataMap : EscenicAutonomyArticleDataMap
	{
		public ClinicaSraArticleDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
		}

		public override IEnumerable<object> GetImportData()
		{
			if (!Directory.Exists(this.Query))
			{
				Logger.Log("N/A", string.Format("the folder '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
				return Enumerable.Empty<object>();
			}

			List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();

			long artNumber = GetNextArticleNumber();

			string[] files = Directory.GetFiles(this.Query);
			foreach (string f in files)
			{
				Dictionary<string, string> ao = new Dictionary<string, string>();
				XmlDocument d = GetXmlDocument(f);
				if (d == null)
					continue;

				//generated field
				string curFileName = new FileInfo(f).Name;
				ao["ARTICLE NUMBER"] = $"{PublicationPrefix}{artNumber:D6}";

				//escenic field values
				string authorNode = "STORYAUTHORNAME";
				ao.Add(authorNode, AuthorHelper.Authors(GetXMLData(d, authorNode)));
				string bodyNode = "BODY";
				ao.Add(bodyNode, GetXMLData(d, bodyNode));
				string titleNode = "TITLE";
				string cleanTitleHtml = CleanTitleHtml(GetXMLData(d, titleNode));
				ao.Add(titleNode, cleanTitleHtml);
				ao.Add("FILENAME", cleanTitleHtml);
				ao.Add("META TITLE OVERRIDE", cleanTitleHtml);
				ao.Add("ARTICLEID", curFileName.Replace(".xml", ""));
				
				//autonomy fields
				string autFile = $@"{this.Query}\..\Autonomy\{curFileName}";

				List<string> autNodes = new List<string>() { "CATEGORY", "COMPANY", "STORYUPDATE", "SECTION", "COUNTRY", "KEYWORD", "THERAPY_SECTOR", "TREATABLE_CONDITION" };
				//if no autonomy file then fill fields with empty
				if (!File.Exists(autFile))
				{
					Logger.Log("N/A", "File not found", ProcessStatus.NotFoundError, "File", autFile);
					foreach (string n in autNodes)
						ao.Add(n, string.Empty);

					//default back to the date from escenic
					string dateVal = GetXMLData(d, "DATEPUBLISHED");
					DateTime date;
					if (!DateTimeUtil.ParseInformaDate(dateVal, out date))
						Logger.Log("N/A", "No Date to parse error", ProcessStatus.DateParseError, "Missing Autonomy File Name", autFile);
					else
						ao["STORYUPDATE"] = dateVal;

					continue;
				}

				XmlDocument d2 = GetXmlDocument(autFile);
				if (d2 != null)
				{
					foreach (string n in autNodes)
						ao.Add(n, GetXMLData(d2, n));
				}

				string categoryName = ao.ContainsKey("CATEGORY") ? ao["CATEGORY"] : string.Empty;
				if (categoryName.ToLower().Equals("pdfnewsletter")) continue;

				string sectionName = ao.ContainsKey("SECTION") ? ao["SECTION"] : string.Empty;
				if (sectionName.ToLower().Equals("pdf library")) continue;

				l.Add(ao);
				artNumber++;
			}

			return l;
		}
	}
}