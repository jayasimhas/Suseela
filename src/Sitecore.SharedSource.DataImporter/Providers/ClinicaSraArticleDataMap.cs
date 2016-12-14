using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.DataContext;
using Sitecore.SharedSource.DataImporter.DataContext.Entities;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Utility;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public class ClinicaSraArticleDataMap : EscenicAutonomyArticleDataMap
	{
		public ClinicaSraArticleDataMap(Database db, string connectionString, Item importItem, ILogger l) : base(db, connectionString, importItem, l)
		{
		}

		public override IEnumerable<object> GetImportData(string site, string channel)
		{
			if (!Directory.Exists(this.Query))
			{
				Logger.Log("N/A", string.Format("the folder '{0}' could not be found. Try moving the folder under the webroot.", this.Query), ProcessStatus.ImportDefinitionError);
				return Enumerable.Empty<object>();
			}

			List<Dictionary<string, string>> l = new List<Dictionary<string, string>>();

			string[] files = Directory.GetFiles(this.Query);
			var filteredFiles = new List<Tuple<string, string, string, XmlDocument>>();
			using (var context = new EscenicIdMappingContext())
			{
				foreach (var f in files)
				{
					string curFileName = new FileInfo(f).Name;
					string articleId = curFileName.Replace(".xml", "");

					//autonomy fields
					XmlDocument dAut = null;
					string autFile = $@"{this.Query}\..\Autonomy\{curFileName}";
					if (File.Exists(autFile))
					{
						dAut = GetXmlDocument(autFile);

						if (dAut != null)
						{
							// ABORT IF OF THIS TYPE
							string categoryName = GetXMLData(dAut, "CATEGORY") ?? string.Empty;
							if (categoryName.ToLower().Equals("pdfnewsletter")) continue;

							string sectionName = GetXMLData(dAut, "SECTION") ?? string.Empty;
							if (sectionName.ToLower().Equals("pdf library")) continue;
						}
					}
					
					string artNumber = SetArticleNumber(context, articleId);

					filteredFiles.Add(new Tuple<string, string, string, XmlDocument>(curFileName, artNumber, f, dAut));
				}

				context.SaveChanges();
			}

			foreach (Tuple<string, string, string, XmlDocument> pair in filteredFiles)
			{
				Dictionary<string, string> ao = new Dictionary<string, string>();
				XmlDocument d = GetXmlDocument(pair.Item3);
				if (d == null) continue;

				//generated field
				string curFileName = pair.Item1;
				string articleId = curFileName.Replace(".xml", "");

				ao["ARTICLE NUMBER"] = pair.Item2;

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
				ao.Add("ARTICLEID", articleId);

				//autonomy fields
				XmlDocument d2 = pair.Item4;

				List<string> autNodes = new List<string>() { "CATEGORY", "COMPANY", "STORYUPDATE", "SECTION", "COUNTRY", "KEYWORD", "THERAPY_SECTOR", "TREATABLE_CONDITION", "TECHNOLOGYSECTION", "COUNTRYSECTION" };
				//if no autonomy file then fill fields with empty
				if (d2 == null)
				{
					Logger.Log("N/A", "File not found", ProcessStatus.NotFoundError, "File", $@"{this.Query}\..\Autonomy\{curFileName}");
					foreach (string n in autNodes)
						ao.Add(n, string.Empty);

					//default back to the date from escenic
					string dateVal = GetXMLData(d, "DATEPUBLISHED");
					DateTime date;
					if (!DateTimeUtil.ParseInformaDate(dateVal, out date))
						Logger.Log("N/A", "No Date to parse error", ProcessStatus.DateParseError, "Missing Autonomy File Name", $@"{this.Query}\..\Autonomy\{curFileName}");
					else
						ao["STORYUPDATE"] = dateVal;
				}
				else
				{
					foreach (string n in autNodes)
						ao.Add(n, GetXMLData(d2, n));
				}
				
				l.Add(ao);
			}

			return l;
		}

		private long _articleNumber = -1;

		protected long ArticleNumber
		{
			get
			{
				if (_articleNumber == -1)
				{
					_articleNumber = GetNextArticleNumber();
				}

				return _articleNumber;
			}
			set { _articleNumber = value; }
		}

		private Dictionary<string, string> _idMap;

		protected Dictionary<string, string> IdMap
		{
			get
			{
				if (_idMap == null)
				{
					_idMap = new Dictionary<string, string>();
					using (var context = new EscenicIdMappingContext())
					{
						foreach (var mapping in context.EscenicIdMappings.Where(x => x.ArticleNumber.StartsWith(PublicationPrefix)).ToArray())
						{
							_idMap[mapping.EscenicId] = mapping.ArticleNumber;
						}
					}
				}

				return _idMap;
			}
		}  

		protected virtual string SetArticleNumber(EscenicIdMappingContext context, string escenicId)
		{
			var map = IdMap.ContainsKey(escenicId) ? IdMap[escenicId] : null;

			if (string.IsNullOrEmpty(map))
			{
				var addMap = new EscenicIdMapping
				{
					EscenicId = escenicId,
					ArticleNumber = $"{PublicationPrefix}{ArticleNumber++:D6}"
				};

				map = addMap.ArticleNumber;
				context.EscenicIdMappings.Add(addMap);
			}

			return map;
		}

		protected override int GetNextArticleNumber()
		{
			var sitecoreId = base.GetNextArticleNumber();
			int mappingId = 1;
			using (var context = new EscenicIdMappingContext())
			{
				var mapping =
					context.EscenicIdMappings.OrderByDescending(x => x.ArticleNumber)
						.FirstOrDefault(x => x.ArticleNumber.StartsWith(PublicationPrefix));
				if (mapping != null)
				{
					mappingId = int.Parse(mapping.ArticleNumber.Replace(PublicationPrefix, string.Empty));
				}
			}

			return sitecoreId > mappingId ? sitecoreId : mappingId;
		}
	}
}