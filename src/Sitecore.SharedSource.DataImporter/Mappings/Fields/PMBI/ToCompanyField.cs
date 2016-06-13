using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Mappings.Fields.PMBI
{
	public class ToCompanyField : ToInformaCompanyField
	{
		public ToCompanyField(Item i) : base(i)
		{
		}

		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{
			// connect to the company database and get the ID to store
			if (importValue.Equals(string.Empty))
				return;

			//store the imported value as is
			Field f = newItem.Fields[NewItemField];
			if (f == null)
				return;

			//set ids in field as comma delim list
			f.Value = importValue;

			//try exact search first
			Dictionary<string, string> dbCompanies = GetDBCompaniesById(map, newItem.Paths.FullPath);

			IEnumerable<string> importCompanies = importValue.Split(new string[] { "," },
				StringSplitOptions.RemoveEmptyEntries).Distinct();

			foreach (string cID in importCompanies)
			{
				KeyValuePair<string, string> pair;
				if (dbCompanies.ContainsKey(cID))
				{
					pair = dbCompanies.First(a => a.Key.Equals(cID));
				}
				else
				{
					var ids = dbCompanies.Where(a => a.Key.Contains(cID));
					if (ids == null || !ids.Any())
					{
						map.Logger.Log(newItem.Paths.FullPath, "Company not found", ProcessStatus.FieldError, NewItemField, cID);
						continue;
					}
					else if (ids.Count() > 1)
					{ // format as (number-name)
						map.Logger.Log(newItem.Paths.FullPath, $"Company not selected. Possible matches '{string.Join(" ", ids.Select(m => $"{m.Key}-{m.Value}"))}'", ProcessStatus.FieldError, NewItemField, cID);
						continue;
					}
					pair = ids.First();
				}

				string companyName = pair.Value;
				if (string.IsNullOrEmpty(companyName))
					continue;

				Regex companyPattern = new Regex($"\\[C#({cID})\\]", RegexOptions.IgnoreCase);
				//the name should be the importValue and not the value from the database
				Field sf = newItem.Fields["Summary"];
				if (sf != null)
				{
					sf.Value = companyPattern.Replace(sf.Value, match => $"[C#{cID}:{companyName}]");
				}

				Field bf = newItem.Fields["Body"];
				if (bf != null)
				{
					bf.Value = companyPattern.Replace(bf.Value, match => $"[C#{cID}:{companyName}]");
				}
			}
		}
	}
}
