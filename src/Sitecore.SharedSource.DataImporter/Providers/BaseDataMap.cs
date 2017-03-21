using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.SharedSource.DataImporter.Mappings.Fields;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Mappings;
using System.Globalization;
using Sitecore.SharedSource.DataImporter.Mappings.Properties;
using System.Data;
using System.Collections;
using System.Web;
using Sitecore.SharedSource.DataImporter.Utility;
using Sitecore.Collections;
using System.IO;
using Sitecore.Globalization;
using Sitecore.Data.Managers;
using Sitecore.Configuration;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SecurityModel;
using System.Text.RegularExpressions;

namespace Sitecore.SharedSource.DataImporter.Providers
{
    /// <summary>
    /// The BaseDataMap is the base class for any data provider. It manages values stored in sitecore 
    /// and does the bulk of the work processing the fields
    /// </summary>
    public abstract class BaseDataMap : IDataMap
    {



        #region Static IDs

        public static readonly string FieldsFolderTemplateID = "{98EF4356-8BFE-4F6A-A697-ADFD0AAD0B65}";

        public static readonly string CommonFolderTemplateID = "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}";

        #endregion Static IDs

        #region Properties

        public Item ImportItem { get; set; }

        #endregion Properties

        #region IDataMap Properties

        public ILogger Logger { get; set; }

        /// <summary>
        /// the reference to the sitecore database that you'll import into
        /// </summary>
        public Database ToDB { get; set; }

        public int ItemNameMaxLength { get; set; }

        /// <summary>
        /// the definitions of fields to import
        /// </summary>
        public List<IBaseField> FieldDefinitions { get; set; }

        /// <summary>
        /// the connection string to the database you're importing from
        /// </summary>
        public string DatabaseConnectionString { get; set; }

        #endregion IDataMap Properties

        #region IDataMap Fields

        /// <summary>
        /// the query used to retrieve the data
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// the parent item where the new items will be imported into
        /// </summary>
        public Item ImportToWhere { get; set; }

        /// <summary>
        /// the template to create new items with
        /// </summary>
        public CustomItemBase ImportToWhatTemplate { get; set; }

        public string[] ItemNameFields { get; set; }

        public Language ImportToLanguage { get; set; }

        /// <summary>
        /// tells whether or not to folder new items by a date
        /// </summary>
        public bool FolderByDate { get; set; }

        /// <summary>
        /// tells whether or not to folder new items by first letter of their name
        /// </summary>
        public bool FolderByName { get; set; }

        /// <summary>
        /// the name of the field that stores a date to folder by
        /// </summary>
        public string DateField { get; set; }

        /// <summary>
        /// the template used to create the folder items
        /// </summary>
        public TemplateItem FolderTemplate { get; set; }

        #endregion IDataMap Fields

        #region Constructor

        public BaseDataMap(Database db, string connectionString, Item importItem, ILogger l)
        {
            if (l == null)
                throw new Exception("The provided Logger is null");

            //instantiate log
            Logger = l;

            //setup import details
            ToDB = db;
            DatabaseConnectionString = connectionString;
            ImportItem = importItem;

            //determine the item name max length
            ItemNameMaxLength = GetNameLength();

            //get query
            Query = ImportItem.GetItemField("Query", Logger);

            //get parent item
            ImportToWhere = GetImportToWhereItem();

            //get new item template
            ImportToWhatTemplate = GetImportToTemplate();

            //get item name field
            ItemNameFields = ImportItem.GetItemField("Pull Item Name from What Fields", Logger).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            //get import language
            ImportToLanguage = GetImportItemLanguage("Import To Language");

            //foldering information
            FolderByDate = ImportItem.GetItemBool("Folder By Date");
            FolderByName = ImportItem.GetItemBool("Folder By Name");
            DateField = ImportItem.GetItemField("Date Field", Logger);
            FolderTemplate = GetImportFolderTemplate();

            //populate field definitions
            FieldDefinitions = GetFieldDefinitions(ImportItem);
        }

        #endregion Constructor

        #region Constructor Helpers

        public int GetNameLength()
        {
            int i = 100;
            return (int.TryParse(Settings.GetSetting("MaxItemNameLength"), out i))
                ? i
                : 100;
        }

        public Item GetImportToWhereItem()
        {

            Item toWhere = null;

            //check field value
            string toWhereID = ImportItem.GetItemField("Import To Where", Logger);
            if (string.IsNullOrEmpty(toWhereID))
            {
                Logger.Log(ImportItem.Paths.FullPath, "the 'To Where' field is not set on the import item", ProcessStatus.ImportDefinitionError, "Import To Where");
                return null;
            }

            //check item
            toWhere = ToDB.Items[toWhereID];
            if (toWhere.IsNull())
                Logger.Log(ImportItem.Paths.FullPath, "the 'To Where' item is null on the import item", ProcessStatus.ImportDefinitionError, "Import To Where", toWhereID);

            return toWhere;
        }

        public CustomItemBase GetImportToTemplate()
        {

            CustomItemBase template = null;

            //check field value
            string templateID = ImportItem.GetItemField("Import To What Template", Logger);
            if (string.IsNullOrEmpty(templateID))
            {
                Logger.Log(ImportItem.Paths.FullPath, "the 'To What Template' field is not set", ProcessStatus.ImportDefinitionError, "Import To What Template");
                return null;
            }

            //check template item
            Item templateItem = ToDB.Items[templateID];
            if (templateItem.IsNull())
            {
                Logger.Log(ImportItem.Paths.FullPath, "the 'To What Template' item is null", ProcessStatus.ImportDefinitionError, "Import To What Template");
                return null;
            }

            //determine template item type
            if ((BranchItem)templateItem != null)
            {
                template = (BranchItem)templateItem;
            }
            else
            {
                template = (TemplateItem)templateItem;
            }

            return template;
        }

        public Language GetImportItemLanguage(string fieldName)
        {

            Language l = LanguageManager.DefaultLanguage;

            //check the field
            string langID = ImportItem.GetItemField(fieldName, Logger);
            if (string.IsNullOrEmpty(langID))
            {
                Logger.Log(ImportItem.Paths.FullPath, "The 'Import Language' field is not set on the import item", ProcessStatus.ImportDefinitionError, fieldName);
                return l;
            }

            //check item
            Item iLang = ToDB.GetItem(langID);
            if (iLang.IsNull())
            {
                Logger.Log(ImportItem.Paths.FullPath, "The 'Import Language' Item is null on the import item", ProcessStatus.ImportDefinitionError, fieldName);
                return l;
            }

            //check language
            l = LanguageManager.GetLanguage(iLang.Name);
            if (l == null)
            {
                Logger.Log(ImportItem.Paths.FullPath, "The 'Import Language' name is not valid on the import item", ProcessStatus.ImportDefinitionError, fieldName);
            }

            return l;
        }

        public TemplateItem GetImportFolderTemplate()
        {

            if (!FolderByName && !FolderByDate)
                return null;

            //setup a default type to an ordinary folder
            TemplateItem defaultTemplate = ToDB.Templates[CommonFolderTemplateID];

            //if they specify a type then use that
            string folderID = ImportItem.GetItemField("Folder Template", Logger);
            if (string.IsNullOrEmpty(folderID))
                return defaultTemplate;

            //check the folder template
            TemplateItem fTemplate = ToDB.Templates[folderID];
            return (fTemplate == null) ? defaultTemplate : fTemplate;
        }

        #endregion Constructor Helpers

        #region Methods

        protected virtual List<IBaseField> GetFieldDefinitions(Item i)
        {

            List<IBaseField> l = new List<IBaseField>();

            //check for fields folder
            Item Fields = i.GetChildByTemplate(FieldsFolderTemplateID);
            if (Fields.IsNull())
            {
                Logger.Log(i.Paths.FullPath, "there is no 'Fields' folder on the import item", ProcessStatus.ImportDefinitionError);
                return l;
            }

            //check for any children
            if (!Fields.HasChildren)
            {
                Logger.Log(i.Paths.FullPath, "there are no fields to import on  on the import item", ProcessStatus.ImportDefinitionError);
                return l;
            }

            ChildList c = Fields.GetChildren();
            foreach (Item child in c)
            {
                //create an item to get the class / assembly name from
                BaseMapping bm = new BaseMapping(child);

                //check for assembly
                if (string.IsNullOrEmpty(bm.HandlerAssembly))
                {
                    Logger.Log(child.Paths.FullPath, "the field's Handler Assembly is not defined", ProcessStatus.ImportDefinitionError, child.Name, bm.HandlerAssembly);
                    continue;
                }

                //check for class
                if (string.IsNullOrEmpty(bm.HandlerClass))
                {
                    Logger.Log(child.Paths.FullPath, "the field's Handler Class is not defined", ProcessStatus.ImportDefinitionError, child.Name, bm.HandlerClass);
                    continue;
                }

                //create the object from the class and cast as base field to add it to field definitions
                IBaseField bf = null;
                try
                {
                    bf = (IBaseField)Sitecore.Reflection.ReflectionUtil.CreateObject(bm.HandlerAssembly, bm.HandlerClass, new object[] { child });
                }
                catch (FileNotFoundException fnfe)
                {
                    Logger.Log(child.Paths.FullPath, "the field's binary specified could not be found", ProcessStatus.ImportDefinitionError, child.Name, bm.HandlerAssembly);
                }

                if (bf != null)
                    l.Add(bf);
                else
                    Logger.Log(child.Paths.FullPath, "the field's class type could not be instantiated", ProcessStatus.ImportDefinitionError, child.Name, bm.HandlerClass);
            }

            return l;
        }

        /// <summary>
        /// retrieves all the import field values specified
        /// </summary>
        protected virtual IEnumerable<string> GetFieldValues(IEnumerable<string> fieldNames, object importRow)
        {
            List<string> list = new List<string>();
            foreach (string f in fieldNames)
            {
                try
                {
                    list.Add(GetFieldValue(importRow, f));
                }
                catch (ArgumentException ex)
                {
                    Logger.Log("N/A", (string.IsNullOrEmpty(f))
                        ? "the 'From' field name is empty"
                        : "the field doesn't exist in the import row",
                        ProcessStatus.FieldError, f);
                }
            }
            return list;
        }

        /// <summary>
        /// will begin looking for or creating date folders to get a parent node to create the new items in
        /// </summary>
        /// <param name="parentNode">current parent node to create or search folder under</param>
        /// <param name="dt">date time value to folder by</param>
        /// <param name="folderType">folder template type</param>
        /// <returns></returns>
        protected Item GetDateParentNode(Item parentNode, DateTime dt, TemplateItem folderType)
        {
            //get year folder
            Item year = GetChild(parentNode, dt.Year.ToString());
            if (year == null)
            {
                //build year folder if you have to
                year = parentNode.Add(dt.Year.ToString(), folderType);
            }
            //set the parent to year
            parentNode = year;

            //get month folder
            Item month = GetChild(parentNode, dt.ToString("MM"));
            if (month == null)
            {
                //build month folder if you have to
                month = parentNode.Add(dt.ToString("MM"), folderType);
            }
            //set the parent to year
            parentNode = month;

            //get day folder
            Item day = GetChild(parentNode, dt.ToString("dd"));
            if (day == null)
            {
                //build day folder if you have to
                day = parentNode.Add(dt.ToString("dd"), folderType);
            }
            //set the parent to year
            parentNode = day;

            return parentNode;
        }

        /// <summary>
        /// will begin looking for or creating letter folders to get a parent node to create the new items in
        /// </summary>
        /// <param name="parentNode">current parent node to create or search folder under</param>
        /// <param name="letter">the letter to folder by</param>
        /// <param name="folderType">folder template type</param>
        /// <returns></returns>
        protected Item GetNameParentNode(Item parentNode, string letter, TemplateItem folderType)
        {
            //get letter folder
            Item letterItem = GetChild(parentNode, letter);
            if (letterItem == null) //build year folder if you have to
                letterItem = parentNode.Add(letter, folderType);

            //set the parent to year
            return letterItem;
        }

        public static Item GetChild(Item i, string childName)
        {
            string childPath = string.Format("{0}/{1}", i.Paths.FullPath, childName);
            return i.Database.GetItem(childPath);
        }

        /// <summary>
        /// Set fields to be or not to be updated
        /// </summary>
        /// <param name="fields">Properties to set</param>
        /// <param name="dict">Property name with value indicating whether to update</param>
        public void SetFieldUpdateFlags(List<IBaseField> fields, Dictionary<string, bool> dict)
        {
            foreach (var field in fields)
            {
                if (dict.Any() && dict.ContainsKey(field.InnerItem.Name))
                {
                    field.DoUpdate = dict[field.InnerItem.Name];
                }
                // If not specified set update to true
                else
                {
                    field.DoUpdate = true;
                }
            }
        }

        #endregion Methods

        #region IDataMap Methods

        /// <summary>
        /// gets the data to be imported
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<object> GetImportData(string site, string channel);

        public abstract IEnumerable<object> ImportImages(IDataMap map);

        public abstract IEnumerable<object> RemoveArticles(IDataMap map);

        /// <summary>
        /// this is used to process custom fields or fields
        /// </summary>
        public abstract void ProcessCustomData(ref Item newItem, object importRow);

        /// <summary>
        /// Defines how the subclass will retrieve a field value
        /// </summary>
        public abstract string GetFieldValue(object importRow, string fieldName);

        public virtual CustomItemBase GetNewItemTemplate(object importRow)
        {
            //Create new item
            if (ImportToWhatTemplate == null || ImportToWhatTemplate.InnerItem.IsNull())
                throw new NullReferenceException("The 'Import To What Template' item is null");
            return ImportToWhatTemplate;
        }

        public virtual List<IBaseField> GetFieldDefinitionsByRow(object importRow)
        {
            return FieldDefinitions;
        }

        public string BuildNewItemName(object importRow)
        {
            if (!ItemNameFields.Any())
                throw new NullReferenceException("there are no 'Name' fields specified");

            StringBuilder strItemName = new StringBuilder();
            foreach (string nameField in ItemNameFields)
            {
                try
                {
                    strItemName.Append(GetFieldValue(importRow, nameField));
                }
                catch (ArgumentException ex)
                {
                    throw new NullReferenceException(string.Format("the field name: '{0}' does not exist in the import row", nameField));
                }
            }

            string nameValue = strItemName.ToString();
            if (string.IsNullOrEmpty(nameValue))
                throw new NullReferenceException(string.Format("the name fields: '{0}' are empty in the import row", string.Join(",", ItemNameFields)));
            return StringUtility.GetValidItemName(strItemName.ToString().Trim(), this.ItemNameMaxLength).Trim();
        }

        public Item CreateNewItem(Item parent, object importRow, string newItemName, string site, string publication)
        {
            string ArticleId = string.Empty;
            try
            {
                CustomItemBase nItemTemplate = GetNewItemTemplate(importRow);
                string mapLog = "Mapping ArticleId:";

                string errorLog = string.Empty;
                string EscenicIDLog = "EscenicIDMissingLog";
                string ContentTypeLog = "ContentTypeMissingLog";
                string MediaTypeLog = "MediaTypeMissingLog";
                string BodyTagLog = "BodyTagMissingLog";
                string PublishDateLog = "PublishDateMissingLog";
                string AgencyLog = "AgencyMissingLog";
                string CommercialLog = "CommercialMissingLog";
                string CommodityFactorLog = "CommodityFactorMissingLog";
                string CommodityLog = "CommodityMissingLog";
                string RegionLog = "RegionMissingLog";
                string CompanyLog = "CompanyMissingLog";
                string AuthorsMissingLog = "AuthorsMissingLog";

                Dictionary<string, string> ArticleData = (Dictionary<string, string>)importRow;

                using (new LanguageSwitcher(ImportToLanguage))
                {
                    //get the parent in the specific language
                    parent = ToDB.GetItem(parent.ID);

                    Item newItem;
                    //search for the child by name
                    newItem = GetChild(parent, newItemName);

                    var dict = new Dictionary<string, bool>();
                    // Check if item exists, flag article number field as not to update
                    if (newItem != null)
                    {
                        try
                        {
                            if (newItem.Fields["Legacy Sitecore ID"]?.Value == (importRow as Item)?.ID.ToString())
                            {
                                dict.Add("Article Number", false);
                            }
                        }
                        catch
                        {

                        }

                    }

                    //if not found then create one
                    if (newItem == null)
                    {
                        if (nItemTemplate is BranchItem)
                        {
                            newItem = parent.Add(newItemName, (BranchItem)nItemTemplate);
                        }
                        else
                        {
                            newItem = parent.Add(newItemName, (TemplateItem)nItemTemplate);
                        }
                    }

                    if (newItem == null)
                    {
                        throw new NullReferenceException("the new item created was null");
                    }

                    using (new EditContext(newItem, true, false))
                    {
                        //    if (!string.IsNullOrEmpty(newItem.Fields["Escenic ID"].Value))
                        //        ArticleId =  newItem.Fields["Escenic ID"].Value;

                        try
                        {
                            ArticleId = ArticleData["ARTICLEID"];
                        }
                        catch
                        {


                        }
                        //add in the field mappings
                        List<IBaseField> fieldDefs = GetFieldDefinitionsByRow(importRow);
                        SetFieldUpdateFlags(fieldDefs, dict);
                        fieldDefs = fieldDefs.Where(i => i.DoUpdate).ToList();
                        string taxanomyimportvalue = string.Empty;
                        int taxanomycount = 0;
                        string TaxonomyStr = "";

                        //Nooftableau
                        if (ArticleData.ContainsKey("Nooftableau"))
                        {
                            if (ArticleData["Nooftableau"] != null)
                            {
                                ListToGuid.DataLogger.Add("Tableau (Y/N)", "Y");
                                int numberofTableaus = System.Convert.ToInt32(ArticleData["Nooftableau"]);
                                string tableauname = string.Empty;
                                TemplateItem PageAssets = ToDB.GetItem("{EBEB3CE7-6437-4F3F-8140-F5C9A552471F}");
                                Item PageAssetsItem = newItem.Add("PageAssets", (TemplateItem)PageAssets);

                                for (int numberofTableau = 1; numberofTableau <= numberofTableaus; numberofTableau++)
                                {
                                    tableauname = "tableau" + numberofTableau.ToString();

                                    //get the parent in the specific language
                                    TemplateItem tebelu = ToDB.GetItem("{580A652A-EB37-446A-A16B-B3409C902FE5}");
                                    //search for the child by name

                                    Item tableauItem = PageAssetsItem.Add("tebleau", (TemplateItem)tebelu);
                                    try
                                    {
                                        using (new SecurityDisabler())
                                        {
                                            // string xx= (Dictionary<string, string>())importRow[""].value;
                                            tableauItem.Editing.BeginEdit();
                                            tableauItem.Fields["Authentication Required"].Value = ArticleData[tableauname + "authenticationrequired"] == "false" ? "0" : "1";
                                            tableauItem.Fields["Dashboard Name"].Value = ArticleData[tableauname + "dashboardname"];
                                            tableauItem.Fields["Mobile Dashboard Name"].Value = ArticleData[tableauname + "dashboardname"];
                                            tableauItem.Fields["Filter"].Value = ArticleData[tableauname + "filter"];
                                            tableauItem.Fields["Width"].Value = ArticleData[tableauname + "width"];
                                            tableauItem.Fields["Height"].Value = ArticleData[tableauname + "height"];
                                            tableauItem.Fields["Page Title"].Value = ArticleData[tableauname + "title"];
                                            tableauItem.Editing.EndEdit();
                                            string tableauToken = getTokenForTableau(tableauItem.ID.ToString(), ArticleData[tableauname + "-sourceid"]);


                                            if (!string.IsNullOrEmpty(ArticleData["STORYBODY"]))
                                            {
                                                string wordToFind = Regex.Match(ArticleData["STORYBODY"], @"<NEDIAREL\s*(.+?)\s*</NEDIAREL>").ToString();
                                                if (!string.IsNullOrEmpty(wordToFind))
                                                {
                                                    ArticleData["STORYBODY"] = Regex.Replace(ArticleData["STORYBODY"], wordToFind, tableauToken, RegexOptions.IgnoreCase);
                                                }
                                                else
                                                {
                                                    ArticleData["STORYBODY"] = ArticleData["STORYBODY"] + tableauToken;
                                                }
                                            }


                                        }
                                    }
                                    catch
                                    {

                                    }
                                }


                            }

                            else
                            {
                                ListToGuid.DataLogger.Add("Tableau (Y/N)", "N");
                            }
                        }
                        if (!ArticleData.ContainsKey("Nooftableau"))
                        {
                            ListToGuid.DataLogger.Clear();
                            ListToGuid.DataLogger.Add("Tableau (Y/N)", "N");
                        }
                        if (ArticleData.ContainsKey("dashboardname"))
                        {
                            //TemplateItem PageAssets = ToDB.GetItem("{EBEB3CE7-6437-4F3F-8140-F5C9A552471F}");
                            //Item PageAssetsItem = newItem.Add("PageAssets", (TemplateItem)PageAssets);
                            ////get the parent in the specific language
                            //TemplateItem tebelu = ToDB.GetItem("{580A652A-EB37-446A-A16B-B3409C902FE5}");
                            ////search for the child by name

                            //Item tableauItem = PageAssetsItem.Add("tebleau", (TemplateItem)tebelu);
                            //try
                            //{
                            //    using (new SecurityDisabler())
                            //    {
                            //        // string xx= (Dictionary<string, string>())importRow[""].value;
                            //        tableauItem.Editing.BeginEdit();
                            //        tableauItem.Fields["Authentication Required"].Value = ArticleData["authenticationrequired"] == "false" ? "0" : "1";
                            //        tableauItem.Fields["Dashboard Name"].Value = ArticleData["dashboardname"];
                            //        tableauItem.Fields["Mobile Dashboard Name"].Value = ArticleData["dashboardname"];
                            //        tableauItem.Fields["Filter"].Value = ArticleData["filter"];
                            //        tableauItem.Fields["Width"].Value = ArticleData["width"];
                            //        tableauItem.Fields["Height"].Value = ArticleData["height"];
                            //        tableauItem.Fields["Page Title"].Value = ArticleData["title"];
                            //        tableauItem.Editing.EndEdit();
                            //        string tableauToken = getTokenForTableau(tableauItem.ID.ToString());


                            //        if (!string.IsNullOrEmpty(ArticleData["STORYBODY"]))
                            //        {
                            //            string wordToFind = Regex.Match(ArticleData["STORYBODY"], @"<NEDIAREL\s*(.+?)\s*</NEDIAREL>").ToString();
                            //            if (!string.IsNullOrEmpty(wordToFind))
                            //            {
                            //                ArticleData["STORYBODY"] = Regex.Replace(ArticleData["STORYBODY"], wordToFind, tableauToken, RegexOptions.IgnoreCase);
                            //            }
                            //            else
                            //            {
                            //                ArticleData["STORYBODY"] = ArticleData["STORYBODY"] + tableauToken;
                            //            }
                            //        }


                            //    }
                            //}
                            //catch
                            //{

                            //}
                        }


                        foreach (IBaseField d in fieldDefs)
                        {
                            string importValue = string.Empty;

                            try
                            {
                                // fieldDefs.Add("");
                                IEnumerable<string> values = GetFieldValues(d.GetExistingFieldNames(), importRow);
                                if (values.Count(val => val != "") > 0)
                                {
                                    importValue = String.Join(d.GetFieldValueDelimiter(), values);
                                }

                                if (d.NewItemField == "Title" && values.Count(val => val != "") > 0)
                                {
                                    ListToGuid.DataLogger.Add("Article Headline", importValue);

                                }

                                //if (d.NewItemField == "Article Number" && values.Count(val => val != "") > 0)
                                //{
                                   // ListToGuid.DataLogger.Add("Article Number", importValue);

                             //   }

                                if (d.NewItemField == "Featured Image 16 9")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("Featured Image (Y/N)", "Y");
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("Featured Image (Y/N)", "N");
                                    }

                                }


                                if (d.NewItemField == "sectionref")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("SECTINREF", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("SECTINREF", "");
                                    }

                                }

                                if (d.NewItemField == "hottopics")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("HOTTOPICS", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("HOTTOPICS", "");
                                    }

                                }


                                if (d.NewItemField == "regulars")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("REGULARS", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("REGULARS", "");
                                    }

                                }
                                if (d.NewItemField == "sectors")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("SECTORS", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("SECTORS", "");
                                    }

                                }
                                if (d.NewItemField == "market")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("MARKET", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("MARKET", "");
                                    }

                                }
                                if (d.NewItemField == "topic")
                                {
                                    if (values.Count(val => val != "") > 0)
                                        ListToGuid.DataLogger.Add("TOPICS", importValue);
                                    else
                                    {
                                        ListToGuid.DataLogger.Add("TOPICS", "");
                                    }

                                }



                                if (d.NewItemField == "Body")
                                {
                                    if (values.Count(val => val == "Y") > 0)
                                        ListToGuid.DataLogger.Add("Body Image (Y/N)", "Y");
                                    else if (values.Count(val => val == "N") > 0)
                                    {
                                        ListToGuid.DataLogger.Add("Body Image (Y/N)", "N");
                                    }
                                }





                                string id = string.Empty;
                                if (importRow is Item)
                                {
                                    id = (importRow as Item).ID.ToString();
                                }
                                //if(d.NewItemField == "Taxonomy")
                                //{
                                //    taxanomycount++;
                                //    taxanomyimportvalue = taxanomyimportvalue + "," + importValue;
                                //}

                                /*
                                 * Modified by: Siddharth Bajaj
                                 * Modified On: 21-Oct-2016
                                 * Related Story / Task ID:  
                                 * Reason: <Code changes to implement logging of missing data while mapping Xml file data with Itemtemplate fields>
                                * */
                                if (d.NewItemField == "Escenic ID")
                                {
                                    if (values.First() != null)
                                    {
                                        string Id = values.First();
                                        mapLog += Id;
                                    }
                                    else
                                    {
                                        errorLog += "||" + "Escenic ID N/A";
                                        XMLDataLogger.WriteLog(ArticleId, EscenicIDLog);
                                    }
                                }

                                if (d.NewItemField == "Content Type" && importValue == "")
                                {
                                    errorLog += "||" + "ContentType N/A";
                                    XMLDataLogger.WriteLog(ArticleId, ContentTypeLog);
                                }

                                if (d.NewItemField == "Media Type" && importValue == "")
                                {
                                    errorLog += "||" + "MediaType N/A";
                                    XMLDataLogger.WriteLog(ArticleId, MediaTypeLog);
                                }

                                if (d.NewItemField == "Actual Publish Date" && importValue == "")
                                {
                                    errorLog += "||" + "PublishDate N/A";
                                    XMLDataLogger.WriteLog(ArticleId, PublishDateLog);
                                }

                                if (d.NewItemField == "Body" && importValue == "")
                                {
                                    errorLog += "||" + "Body N/A";
                                    XMLDataLogger.WriteLog(ArticleId, BodyTagLog);
                                }

                                if (d.NewItemField == "Authors" && importValue == "")
                                {
                                    errorLog += "||" + "Authors N/A";
                                    XMLDataLogger.WriteLog(ArticleId, AuthorsMissingLog);
                                }

                                if (d.NewItemField == "Taxonomy")
                                {

                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("COMMODITY1") && importValue == "")
                                    {
                                        errorLog += "||" + "Commodities N/A";
                                        XMLDataLogger.WriteLog(ArticleId, CommodityLog);
                                    }

                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("COUNTRY1") && importValue == "")
                                    {
                                        errorLog += "||" + "Region N/A";
                                        XMLDataLogger.WriteLog(ArticleId, RegionLog);
                                    }

                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("COMMODITYFACTOR1") && importValue == "")
                                    {
                                        errorLog += "||" + "CommodityFactor N/A";
                                        XMLDataLogger.WriteLog(ArticleId, CommodityFactorLog);
                                    }

                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("COMMERCIAL1") && importValue == "")
                                    {
                                        errorLog += "||" + "Commercial N/A";
                                        XMLDataLogger.WriteLog(ArticleId, CommercialLog);
                                    }
                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("AGENCY1") && importValue == "")
                                    {
                                        errorLog += "||" + "AGENCY N/A";
                                        XMLDataLogger.WriteLog(ArticleId, AgencyLog);

                                    }
                                    if ((((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName).Contains("COMPANIES1") && importValue == "")
                                    {
                                        errorLog += "||" + "COMPANIES N/A";
                                        XMLDataLogger.WriteLog(ArticleId, CompanyLog);

                                    }
                                }
                                if (d.NewItemField == "Taxonomy")
                                {
                                    taxanomycount++;
                                }

                                if (d.NewItemField != "Authors" && d.NewItemField != "Featured Image 16 9" && d.NewItemField != "Body" && d.NewItemField != "Taxonomy")
                                {
                                    d.FillField(this, ref newItem, importValue, id);
                                }
                                else if (d.NewItemField == "Taxonomy")
                                {
                                    d.FillField(this, ref newItem, importValue, site + "," + publication + "," + ((Sitecore.SharedSource.DataImporter.Mappings.Fields.ListToGuid)d).FieldName);
                                }

                                else
                                {
                                    d.FillField(this, ref newItem, importValue, ArticleId,publication);
                                }



                                if (taxanomycount == fieldDefs.Count(n => n.NewItemField == "Taxonomy"))
                                {

                                    Field f = newItem.Fields["Taxonomy"];
                                    foreach (string taxonomy in ListToGuid.TaxonomyList.Distinct())
                                    {

                                        TaxonomyStr = TaxonomyStr + taxonomy + "|";
                                    }

                                    f.Value = TaxonomyStr.Substring(0, TaxonomyStr.Length - 1);
                                    ListToGuid.TaxonomyList.Clear();
                                }

                            }
                            catch (Exception ex)
                            {
                                
                                    continue;

                                
                                Logger.Log(newItem.Paths.FullPath, "the FillField failed", ProcessStatus.FieldError, d.ItemName(), importValue);
                            }
                        }


                        LogIntoExcel.CMCReport(ArticleId, ListToGuid.DataLogger, publication);
                        ListToGuid.DataLogger.Clear();
                        XMLDataLogger.WriteLog(mapLog + errorLog, "");
                        //calls the subclass method to handle custom fields and fields
                        ProcessCustomData(ref newItem, importRow);
                    }

                    // Dictionary<string, string> tableau = (Dictionary<string, string>)importRow;


                    return newItem;

                }
            }
            catch (Exception ex)
            {
                XMLDataLogger.WriteLog(ArticleId, "basedatamaplog");
                return null;
            }
        }

        private string getTokenForTablo()
        {
            return string.Empty;

        }

        /// <summary>
        /// gets the parent of the new item created. will create folders based on name or date if configured to
        /// </summary>
        public virtual Item GetParentNode(object importRow, string newItemName)
        {
            Item thisParent = ImportToWhere;
            if (FolderByDate)
            {
                DateTime date = DateTime.Now;
                string dateValue = string.Empty;

                try
                {
                    dateValue = GetFieldValue(importRow, DateField);
                }
                catch (ArgumentException ex)
                {
                    Logger.Log(newItemName, (string.IsNullOrEmpty(DateField))
                        ? "the date name field is empty"
                        : "the field name does not exist in the import row", ProcessStatus.DateParseError, DateField);
                }

                if (string.IsNullOrEmpty(dateValue))
                {
                    string autFile = GetFieldValue(importRow, "ARTICLEID");
                    Logger.Log(newItemName, "Couldn't folder by date. The date value was empty", ProcessStatus.DateParseError, "Missing Autonomy Article ID", autFile);
                    return thisParent;
                }

                if (!DateTimeUtil.ParseInformaDate(dateValue, out date))
                {
                    Logger.Log(newItemName, "date could not be parsed", ProcessStatus.DateParseError, DateField, dateValue);
                    return thisParent;
                }

                thisParent = GetDateParentNode(ImportToWhere, date, FolderTemplate);
            }
            else if (FolderByName)
            {
                thisParent = GetNameParentNode(ImportToWhere, newItemName.Substring(0, 1), FolderTemplate);
            }
            return thisParent;
        }


        private string getTokenForTableau(string tableau, string sourceid)
        {
            tableau = tableau.Replace("{", String.Empty).Replace("}", String.Empty);
            //< tr >< td >< strong >[T#:c6cc76d3-bc75-43a7-bd86-06d85ff49852]</strong></td></tr>
            //<p xmlns="" class="article-paragraph"> 
            //< strong >[T#:c6cc76d3-bc75-43a7-bd86-06d85ff49852]</strong>
            //</ p >
            //return "< tr >< td >< strong >[T#:" + tableau + "]</strong></td></tr>";

            return "<strong id=" + sourceid + ">[T#:" + tableau + "]</strong>";

        }

        #endregion IDataMap Methods
    }
}