﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;
namespace Sitecore.SharedSource.DataImporter.Mappings.Fields
{

	/// <summary>
	/// This uses imported values to match by name an existing content item in the list provided
	/// then stores the GUID of the existing item
	/// </summary>
	public class ListToGuid : ToText
	{

		#region Properties

		private static readonly MemoryCache Cache = new MemoryCache("SourceItems");

		/// <summary>
		/// This is the list that you will compare the imported values against
		/// </summary>
		public string SourceList { get; set; }

		public IEnumerable<Item> GetSourceItems(Database db)
		{
			if (db == null) return null;

			var item = db.GetItem(SourceList);

			if (item == null) return null;

			return Cache.AddOrGetExisting($"{db.Name}{SourceList}", item.Axes.GetDescendants(), DateTimeOffset.MaxValue) as IEnumerable<Item>;
		} 

		public string OldSourceList { get; set; }
		public string FieldName { get; set; }
		public bool SingleValue { get; set; }

		#endregion Properties

		#region Constructor

		public ListToGuid(Item i)
			: base(i)
		{
			//stores the source list value
			SourceList = GetItemField(i, "Source List");
			OldSourceList = GetItemField(i, "Old Source List");
			FieldName = GetItemField(i, "From What Fields");
			SingleValue = GetItemField(i, "Singe GUID Value") == "1";
		}

		#endregion Constructor

		#region IBaseField

		/// <summary>
		/// uses the import value to search for a matching item in the SourceList and then stores the GUID
		/// </summary>
		/// <param name="map">provides settings for the import</param>
		/// <param name="newItem">newly created item</param>
		/// <param name="importValue">imported value to match</param>
		public override void FillField(IDataMap map, ref Item newItem, string importValue, string id = null)
		{

			if (string.IsNullOrEmpty(importValue))
				return;

			//get parent item of list to search
			Item i = InnerItem.Database.GetItem(SourceList);
			if (i == null)
				return;

			//loop through children and look for anything that matches by name
			string cleanName = StringUtility.GetValidItemName(importValue, map.ItemNameMaxLength);
			IEnumerable<Item> t = i.Axes.GetDescendants().Where(c => c.DisplayName.Equals(cleanName));

			//if you find one then store the id
			if (!t.Any())
				return;

			Field f = newItem.Fields[NewItemField];
			if (f == null)
				return;

			f.Value = t.First().ID.ToString();
		}

		#endregion IBaseField

		#region Methods

		public void FillTaxonomyField(IDataMap map, ref Item newItem, string importValue, Dictionary<string, string> mappingDictionary)
		{
			// Get target Taxonomy Field
			var field = newItem.Fields[NewItemField];
			if (!string.IsNullOrWhiteSpace(importValue) && field != null)
			{
				// Get Taxonomy folder in target database
				var item = newItem.Database.GetItem(SourceList);
				if (item != null)
				{
					var mapping = mappingDictionary;
					var strs = importValue.Split('|');
					var transformedValue = string.Empty;
					var targetDescendants = item.Axes.GetDescendants();

					foreach (var str in strs)
					{
						// If we are mapping subject field, don't map Regulatory->Product Approvals->Securities Regulation
						if (str == "{27E8FA62-AC23-4280-B9D1-744341AD85EF}")
						{
							continue;
						}

						// Get taxonomy name from pmbi database
						var pmbiItem = Database.GetDatabase("pmbiContent").GetItem(new ID(str));
						var pmbiTaxonomyItemName = StringUtility.TrimInvalidChars(pmbiItem?.DisplayName);
						if (string.IsNullOrWhiteSpace(pmbiTaxonomyItemName))
						{
							map.Logger.Log(newItem.Paths.FullPath, $"Couldn't find {FieldName} in pmbi", ProcessStatus.FieldError, NewItemField, importValue);
							continue;
						}

						// If we are mapping Industry field, set pmbiTaxonomyItemName to main industry name
						if (FieldName == "Industries")
						{
							pmbiTaxonomyItemName = pmbiItem.Axes
								.GetAncestors()
								.FirstOrDefault(i => mappingDictionary.ContainsKey(i.Fields["Item Name"].Value))
								?.DisplayName;
						}

						if (pmbiTaxonomyItemName != null && !mapping.ContainsKey(pmbiTaxonomyItemName))
						{
							map.Logger.Log(newItem.Paths.FullPath, "Couldn't find correct mapping", ProcessStatus.FieldError, NewItemField, $"{str}--{pmbiTaxonomyItemName}");
							continue;
						}

						var mappedValue = mapping[pmbiTaxonomyItemName];
						// If taxonomy maps to empty, dismiss it
						if (string.IsNullOrWhiteSpace(mappedValue))
						{
							// If we are mapping article category field, set default to "News" for any article without a mapping for content type
							if (FieldName == "Print Category")
							{
								mappedValue = "News";
							}
							else
							{
								continue;
							}
						}

						// Get GUID of the new taxonomy item
						var val = targetDescendants.FirstOrDefault(i => StringUtility.TrimInvalidChars(i.Fields["Item Name"].Value) == mappedValue)?.ID.ToString();

						if (string.IsNullOrWhiteSpace(val))
						{
							map.Logger.Log(newItem.Paths.FullPath, $"Couldn't find {FieldName} in target DB", ProcessStatus.FieldError, NewItemField, $"{str}->{mappedValue}");
							continue;
						}

						// Avoid adding duplicate GUID
						if (!field.Value.Contains(val))
						{
							if (!SingleValue)
							{
								transformedValue = string.IsNullOrWhiteSpace(transformedValue) ? val : $"{transformedValue}|{val}";
							}
							else
							{
								transformedValue = val;
							}
						}
					}

					// Write value to field
					if (!SingleValue)
					{
						field.Value = string.IsNullOrWhiteSpace(field.Value) ? transformedValue : $"{field.Value}|{transformedValue}";
					}
					else
					{
						field.Value = transformedValue;
					}
				}
			}
		}

		#endregion
	}
}
