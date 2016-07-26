using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Pipelines;
using Sitecore.PrintStudio.Configuration;
using Sitecore.PrintStudio.InDesignConnectorService;
using Sitecore.PrintStudio.PublishingEngine;
using Sitecore.PrintStudio.PublishingEngine.Helpers;
using Sitecore.PrintStudio.PublishingEngine.Pipelines;
using Sitecore.PrintStudio.PublishingEngine.Rendering;
using Sitecore.PrintStudio.PublishingEngine.Scripting;
using Sitecore.Resources.Media;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using Sitecore.Security.Domains;
using Sitecore.SecurityModel;
using Sitecore.SecurityModel.License;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using Sitecore.Workflows;
using License = Sitecore.SecurityModel.License.License;

namespace Informa.Library.PXM
{
	[ToolboxItem(false)]
	[WebService(Namespace = "http://sitecore.net/indesignconnectorservice/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class InDesignConnectorService : WebService
	{
		public InDesignConnectorService()
		{
		}

		[WebMethod]
		public string AddContentMergeItem(string username, string parentID, string itemName, string itemDisplayName, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(parentID);
								TemplateItem templateItem = database.GetItem("/sitecore/templates/Print Studio Templates/Content Merge/ContentMerge");
								Item item1 = item.Add(ServiceHelper.ProposeValidName(itemName, templateItem.Name), templateItem);
								item1.Editing.BeginEdit();
								item1.Fields[FieldIDs.DisplayName].Value = itemDisplayName;
								item1.Editing.EndEdit();
								empty = item1.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("AddContentMergeItem", exception);
			}
			return empty;
		}

		[WebMethod]
		public string AddItem(string username, string mastername, string parentID, string itemName, string itemDisplayName, int languageIndex)
		{
			Item item;
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item1 = database.GetItem(parentID);
								if (!mastername.Contains("P_"))
								{
									BranchItem branchItem = database.Items[database.Items.GetItem(string.Concat("/sitecore/templates/Branches/Print Studio Branches/", mastername)).ID];
									item1.Editing.BeginEdit();
									item = item1.Add(ServiceHelper.ProposeValidName(itemName, branchItem.Name), branchItem);
									item.Editing.BeginEdit();
									item.Fields[FieldIDs.DisplayName].Value = itemDisplayName;
									item.Editing.EndEdit();
									SitecoreHelper.MoveItem(username, item.ID.ToString(), 4, languageIndex);
									empty = item.ID.ToString();
									item1.Editing.EndEdit();
								}
								else
								{
									TemplateItem templateItem = SitecoreHelper.FetchPrintEngineTemplate(mastername, database);
									item1.Editing.BeginEdit();
									if (mastername == "P_Pattern")
									{
										try
										{
											if (item1.Template.Name == "P_TextFrame")
											{
												item1.Fields["StaticContent"].Value = "0";
											}
										}
										catch (Exception exception)
										{
											Logger.Error("HasMergeItemAssigned", exception);
										}
									}
									item = item1.Add(ServiceHelper.ProposeValidName(itemName, templateItem.Name), templateItem);
									item.Editing.BeginEdit();
									item.Fields[FieldIDs.DisplayName].Value = itemDisplayName;
									item.Editing.EndEdit();
									SitecoreHelper.MoveItem(username, item.ID.ToString(), 4, languageIndex);
									empty = item.ID.ToString();
									item1.Editing.EndEdit();
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Logger.Error("AddItem", exception1);
			}
			return empty;
		}

		[WebMethod]
		public bool AddItemFromBranch(string username, string branchType, string branchName, string newItemName, string newItemDisplayName, string parentID, int languageIndex)
		{
			bool flag = false;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(WebConfigHandler.PrintStudioEngineSettings.InDesignConnectorDefaultSettings);
								Item item1 = database.GetItem(item.Fields[branchType].Value);
								BranchItem branchItem = database.GetItem(string.Concat(item1.Paths.LongID, "/", branchName));
								Item item2 = database.GetItem(parentID);
								item2.Editing.BeginEdit();
								Item item3 = item2.Add(ServiceHelper.ProposeValidName(newItemName, branchItem.Name), branchItem);
								item3.Editing.BeginEdit();
								item3.Fields[FieldIDs.DisplayName].Value = newItemDisplayName;
								item3.Editing.EndEdit();
								SitecoreHelper.MoveItem(username, item3.ID.ToString(), 4, languageIndex);
								item2.Editing.EndEdit();
								if (item2.TemplateName == "Pages folder")
								{
									try
									{
										int num = System.Convert.ToInt32(ServiceHelper.FetchStartNumberFirstPage(username, languageIndex, parentID));
										if (num > 0)
										{
											ServiceHelper.RenumberPagesInFolder(username, languageIndex, parentID, num.ToString());
										}
									}
									catch (Exception exception)
									{
										Logger.Error("AddItemFromBranch tryng to set the page number", exception);
									}
								}
								flag = true;
							}
						}
					}
				}
			}
			catch (Exception exception2)
			{
				Exception exception1 = exception2;
				flag = false;
				Logger.Error("AddItemFromBranch", exception1);
			}
			return flag;
		}

		[WebMethod]
		public string AddLibraryFolder(string username, string parentFolderItemId, string newItemName, string newDisplayName, string templateName, int languageIndex)
		{
			string str;
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								string str1 = templateName;
								string str2 = str1;
								if (str1 != null)
								{
									if (str2 == "Folder")
									{
										str = "/sitecore/templates/Common/Folder";
										goto Label0;
									}
									else if (str2 == "Media folder")
									{
										str = "/sitecore/templates/System/Media/Media folder";
										goto Label0;
									}
									else
									{
										if (str2 != "Template Folder")
										{
											goto Label2;
										}
										str = "/sitecore/templates/System/Templates/Template Folder";
										goto Label0;
									}
								}
								Label2:
								str = string.Concat("/sitecore/templates/Print Studio Templates/System/Folder types/", templateName);
								Label0:
								TemplateItem item = database.GetItem(str);
								Item item1 = database.GetItem(parentFolderItemId);
								Item item2 = item1.Add(ServiceHelper.ProposeValidName(newItemName, item.Name), item);
								item2.Editing.BeginEdit();
								item2.Fields[FieldIDs.DisplayName].Value = newDisplayName;
								item2.Editing.EndEdit();
								empty = item2.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("AddLibraryFolder", exception);
			}
			return empty;
		}

		[WebMethod]
		public string AddMasterSnippetItem(string username, string parentID, string itemName, string itemDisplayName, int languageIndex, string diffX, string diffY)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.SelectSingleItem(parentID);
								TemplateItem templateItem = SitecoreHelper.FetchPrintEngineTemplate("P_MasterSnippet", database);
								Item item1 = item.Add(ServiceHelper.ProposeValidName(itemName, templateItem.Name), templateItem);
								item1.Editing.BeginEdit();
								item1.Fields[FieldIDs.DisplayName].Value = itemDisplayName;
								item1.Fields["DiffX"].Value = diffX;
								item1.Fields["DiffY"].Value = diffY;
								item1.Editing.EndEdit();
								empty = item1.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("AddMasterSnippetItem", exception);
			}
			return empty;
		}

		[WebMethod]
		public string AddMediaDocumentByteArray(string username, byte[] file, string fileName, int languageIndex, string parentFolderItemId)
		{
			string empty = string.Empty;
			if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
			{
				try
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						string longID = database.GetItem(parentFolderItemId).Paths.LongID;
						LanguageCollection languages = LanguageManager.GetLanguages(Factory.GetDatabase(WebConfigHandler.CommonSettings.Database));
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							empty = ServiceHelper.CreateMediaItem(file, fileName, database, longID, username);
						}
					}
				}
				catch (Exception exception)
				{
					Logger.Error("AddMediaDocument", exception);
				}
			}
			return empty;
		}

		[WebMethod]
		public string AddSoftTextItem(string username, string parentID, string itemName, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								bool flag = true;
								string str = "Content";
								string empty1 = string.Empty;
								bool flag1 = false;
								ServiceHelper.FetchStaticTextsDefaults(ref flag, ref str, ref empty1, ref flag1, database);
								TemplateItem item = database.GetItem(empty1);
								Item item1 = database.GetItem(parentID);
								Item item2 = item1.Add(ServiceHelper.ProposeValidName(itemName, item.Name), item);
								empty = item2.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("AddSoftTextItem", exception);
			}
			return empty;
		}

		[WebMethod]
		public string AddStickyNote(string username, string parentID, string itemName, string itemDisplayName, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(parentID);
								TemplateItem templateItem = SitecoreHelper.FetchPrintEngineTemplate("P_StickyNote", database);
								Item item1 = item.Add(ServiceHelper.ProposeValidName(itemName, templateItem.Name), templateItem);
								SitecoreHelper.MoveItem(username, item1.ID.ToString(), 4, languageIndex);
								item1.Editing.BeginEdit();
								item1.Fields[FieldIDs.DisplayName].Value = itemDisplayName;
								item1.Fields["Content"].Value = "<ParagraphStyle Style=\"Sticky\"></ParagraphStyle>";
								item1.Editing.EndEdit();
								empty = item1.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("AddStickyNote", exception);
			}
			return empty;
		}

		[WebMethod]
		public string BuildPublishXml(string username, string itemID, int languageIndex, int versionNumber, string useHighRes, bool fromLibraryBrowser)
		{
			Item item;
			string str;
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			using (DatabaseSwitcher databaseSwitcher = new DatabaseSwitcher(database))
			{
				Domain domain = Factory.GetDomain(WebConfigHandler.CommonSettings.Domain);
				string fullName = domain.GetFullName(username);
				Sitecore.Security.Accounts.User user = Sitecore.Context.User;
				if (Sitecore.Security.Accounts.User.Exists(fullName))
				{
					user = Sitecore.Security.Accounts.User.FromName(fullName, true);
				}
				user = user ?? Sitecore.Context.User;
				using (UserSwitcher userSwitcher = new UserSwitcher(user))
				{
					LanguageCollection languages = LanguageManager.GetLanguages(database);
					if (languageIndex > -1 && languages[languageIndex] != null)
					{
						Language language = languages[languageIndex];
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(language))
						{
							item = (versionNumber >= 0 ? database.GetItem(ID.Parse(itemID.Replace("[WB]", string.Empty)), language, Sitecore.Data.Version.Parse(versionNumber)) : database.GetItem(ID.Parse(itemID.Replace("[WB]", string.Empty)), language));
							if (item != null)
							{
								try
								{
									PrintOptions printOption = new PrintOptions()
									{
										UseHighRes = useHighRes.ToLower() == "true",
										IsClient = true,
										RootCacheFolder = WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder,
										PrintExportType = PrintExportType.Flash
									};
									object[] guid = new object[] { item.ID.ToGuid(), "_", null, null };
									guid[2] = DateTime.Now.Ticks;
									guid[3] = ".swf";
									printOption.ResultFileName = string.Concat(guid);
									PrintOptions printOption1 = printOption;
									PrintPipelineArgs printPipelineArg = new PrintPipelineArgs(item, printOption1)
									{
										RenderPartial = fromLibraryBrowser
									};
									CorePipeline.Run("renderXml", printPipelineArg);
									string xmlResultFile = printPipelineArg.XmlResultFile;
									xmlResultFile = string.Concat(WebConfigHandler.CommonSettings.WebHost, "/PrintStudio/Handlers/PrintCacheHandler.ashx?fileName=", printOption1.FormatResourceLink(xmlResultFile));
									str = xmlResultFile;
									return str;
								}
								catch (Exception exception1)
								{
									Exception exception = exception1;
									Logger.Error(string.Concat("BuildPublishXml ", exception.Message), null);
								}
							}
						}
					}
				}
				return string.Empty;
			}
			return str;
		}

		[WebMethod]
		public bool CheckIfContentItem(string username, string itemID, int languageIndex)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						flag = ServiceHelper.CheckIfContentItem(database.GetItem(itemID));
					}
				}
			}
			return flag;
		}

		[WebMethod]
		public bool CheckIfItemCanBeEdited(string username, string itemID, int languageIndex)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						flag = ServiceHelper.CheckIfItemCanBeEdited(username, itemID, languageIndex);
					}
				}
			}
			return flag;
		}

		[WebMethod]
		public bool CheckIfItemExist(string username, string itemPath, int languageIndex)
		{
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								if (database.GetItem(itemPath) != null)
								{
									return true;
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("DoesPathExist", exception);
			}
			return false;
		}

		[WebMethod]
		public string CheckIfPageItemsCanBeEdited(string username, string pageItemID, int languageIndex)
		{
			string empty = string.Empty;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						try
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								foreach (Item child in database.GetItem(pageItemID).Children)
								{
									if (child.TemplateName != "P_Snippet")
									{
										continue;
									}
									foreach (Item item in child.Children)
									{
										if (string.IsNullOrEmpty(item.Fields["Item Reference"].Value) || ServiceHelper.CheckIfReferencedItemCanBeEdited(username, item.ID.ToString(), languageIndex))
										{
											continue;
										}
										string value = item.Fields["Item Reference"].Value;
										Item item1 = database.GetItem(value);
										string owner = item1.Locking.GetOwner();
										if (!string.IsNullOrEmpty(owner))
										{
											string str = empty;
											string[] displayName = new string[] { str, item1.DisplayName, " (owned by: ", owner, "), " };
											empty = string.Concat(displayName);
										}
										else
										{
											empty = string.Concat(empty, item1.DisplayName, ", ");
										}
									}
								}
								if (empty.EndsWith(", "))
								{
									empty = string.Concat("The content of following items could not be saved:\n", empty.Substring(0, empty.Length - 2).Replace(",", ",\n"));
								}
							}
						}
						catch (Exception exception)
						{
							Logger.Error("CheckIfPageItemsCanBeEdited", exception);
						}
					}
				}
			}
			return empty;
		}

		[WebMethod]
		public bool CheckInPageItem(string username, string pageItemID, int languageIndex)
		{
			bool flag;
			bool flag1 = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						try
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(pageItemID);
								if (ServiceHelper.CheckInItem(username, pageItemID))
								{
									foreach (Item child in item.Children)
									{
										if (child.TemplateName != "P_Snippet")
										{
											if (child.TemplateName != "P_StickyNote")
											{
												continue;
											}
											ServiceHelper.CheckInItem(username, child.ID.ToString());
										}
										else
										{
											string str = child.ID.ToString();
											ServiceHelper.CheckInItem(username, str);
											string value = child.Fields["Item Reference"].Value;
											if (string.IsNullOrEmpty(value))
											{
												continue;
											}
											ServiceHelper.CheckInItem(username, value);
										}
									}
									flag1 = true;
								}
								else
								{
									flag = false;
									return flag;
								}
							}
						}
						catch (Exception exception)
						{
							Logger.Error("CheckOutPageItem", exception);
						}
						return flag1;
					}
					return flag;
				}
			}
			return flag1;
		}

		[WebMethod]
		public bool CheckItemIsCheckedOut(string username, string itemID, int languageIndex)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							Item item = database.GetItem(itemID);
							try
							{
								if ((!item.State.CanEdit() || item.Locking.IsLocked()) && item.Locking.GetOwner() == apiUser.Name)
								{
									flag = true;
								}
							}
							catch (Exception exception)
							{
								Logger.Error("CheckOutItem", exception);
							}
						}
					}
				}
			}
			return flag;
		}

		[WebMethod]
		public bool CheckOutPageItem(string username, string pageItemID, int languageIndex)
		{
			bool flag;
			bool flag1 = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						try
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								string str = string.Concat(pageItemID, "|");
								Item item = database.GetItem(pageItemID);
								if (ServiceHelper.CheckOutItem(username, pageItemID))
								{
									flag1 = true;
									foreach (Item child in item.Children)
									{
										string str1 = child.ID.ToString();
										if (!ServiceHelper.CheckOutItem(username, str1))
										{
											flag1 = false;
											break;
										}
										else
										{
											str = string.Concat(str, str1, "|");
											if (child.TemplateName != "P_Snippet")
											{
												continue;
											}
											string value = child.Fields["Item Reference"].Value;
											if (string.IsNullOrEmpty(value))
											{
												continue;
											}
											if (!ServiceHelper.CheckOutItem(username, value))
											{
												flag1 = false;
												break;
											}
											else
											{
												str = string.Concat(str, value, "|");
											}
										}
									}
									if (!flag1)
									{
										string[] strArrays = str.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
										for (int i = 0; i < (int)strArrays.Length; i++)
										{
											ServiceHelper.CheckInItem(username, strArrays[i]);
										}
									}
								}
								else
								{
									flag = false;
									return flag;
								}
							}
						}
						catch (Exception exception)
						{
							Logger.Error("CheckOutPageItem", exception);
						}
						return flag1;
					}
					return flag;
				}
			}
			return flag1;
		}

		[WebMethod]
		public bool CheckRequestedPrivelege(string username, string itemID, string requestedPrivalige)
		{
			bool flag;
			bool flag1 = false;
			if (this.ValidateRequest(username))
			{
				try
				{
					if (!string.IsNullOrEmpty(itemID))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						Item item = database.GetItem(itemID);
						Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
						if (apiUser != null)
						{
							using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
							{
								if (!item.Access.CanRead())
								{
									flag = false;
									return flag;
								}
								else if (requestedPrivalige.ToLower().Equals("write") && item.Access.CanWrite())
								{
									flag1 = true;
								}
								else if (requestedPrivalige.ToLower().Equals("rename") && item.Access.CanRename())
								{
									flag1 = true;
								}
								else if (requestedPrivalige.ToLower().Equals("create") && item.Access.CanCreate())
								{
									flag1 = true;
								}
								else if (requestedPrivalige.ToLower().Equals("delete") && item.Access.CanDelete())
								{
									flag1 = true;
								}
								else if (requestedPrivalige.ToLower().Equals("admin") && item.Access.CanAdmin())
								{
									flag1 = true;
								}
							}
						}
					}
					return flag1;
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					Logger.Error(string.Concat("CheckRequestedPrivelege itemID: ", itemID, " request: ", requestedPrivalige), exception);
					return flag1;
				}
				return flag;
			}
			return flag1;
		}

		[WebMethod]
		public string CollectDataStringCaller(string username, string itemID, int languageIndex, bool fromWorkBox, bool fromImagesViewer, int caller)
		{
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			return ServiceHelper.CollectDataString(username, itemID, languageIndex, fromWorkBox, fromImagesViewer, caller);
		}

		[WebMethod]
		public bool CreateStaticTextItem(string itemID, int languageIndex, string username, string value, string itemName, string folderID)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				using (SecurityDisabler securityDisabler = new SecurityDisabler())
				{
					Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
					LanguageCollection languages = LanguageManager.GetLanguages(database);
					using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
					{
						try
						{
							Item empty = database.SelectSingleItem(itemID);
							if (empty != null && empty.Template.Name == "P_TextFrame")
							{
								TemplateItem templateItem = SitecoreHelper.FetchPrintEngineTemplate("P_Text", database);
								Item item = empty.Add(ServiceHelper.ProposeValidName(itemName, templateItem.Name), templateItem);
								if (item != null)
								{
									item.Editing.BeginEdit();
									item.Fields["Content"].Value = value;
									item.Editing.EndEdit();
									empty.Editing.BeginEdit();
									empty.Fields["Item Reference"].Value = string.Empty;
									empty.Fields["Item Field"].Value = string.Empty;
									empty.Editing.EndEdit();
									flag = true;
								}
							}
						}
						catch (Exception exception)
						{
							Logger.Error("CreateStaticTextItem", exception);
							flag = false;
						}
					}
				}
			}
			return flag;
		}

		[WebMethod]
		public string DeleteItem(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								string templateName = item.TemplateName;
								Item parent = item.Parent;
								string str = parent.ID.ToString();
								int count = parent.Children.Count;
								if (item.Access.CanDelete())
								{
									if (!Settings.RecycleBinActive)
									{
										item.Delete();
									}
									else
									{
										item.Recycle();
									}
								}
								empty = "SUCCESS";
								if (templateName == "P_MasterDocument")
								{
									ItemLink[] referrers = Globals.LinkDatabase.GetReferrers(item);
									for (int i = 0; i < (int)referrers.Length; i++)
									{
										ItemLink itemLink = referrers[i];
										Item sourceItem = itemLink.GetSourceItem();
										sourceItem.Editing.BeginEdit();
										sourceItem.Fields[itemLink.SourceFieldID].Value = string.Empty;
										sourceItem.Editing.EndEdit();
									}
								}
								if (templateName == "P_Page")
								{
									int num = System.Convert.ToInt32(ServiceHelper.FetchStartNumberFirstPage(username, languageIndex, str));
									if (num > 0)
									{
										ServiceHelper.RenumberPagesInFolder(username, languageIndex, str, num.ToString());
									}
								}
								if (templateName == "P_Pattern" && parent.TemplateName.Equals("P_TextFrame") && count == 1)
								{
									parent.Editing.BeginEdit();
									parent.Fields["OverruleChilds"].Value = "0";
									parent.Editing.EndEdit();
								}
								if (templateName == "P_Text" && parent.TemplateName.Equals("P_TextFrame"))
								{
									parent.Editing.BeginEdit();
									parent.Fields["Item Reference"].Value = parent.Parent.Fields["Item Reference"].Value;
									parent.Editing.EndEdit();
								}
							}
							return empty;
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("DeleteItem", exception);
			}
			return empty;
		}

		[WebMethod]
		public string DownloadCountryFlag(string username, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						object[] webHost = new object[] { WebConfigHandler.CommonSettings.WebHost, "/", null, null, null };
						string cacheFolder = Settings.Icons.CacheFolder;
						char[] chrArray = new char[] { '/' };
						webHost[2] = cacheFolder.Trim(chrArray);
						webHost[3] = '/';
						webHost[4] = languages[languageIndex].GetIcon(database);
						empty = string.Concat(webHost);
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("DownloadCountryFlag", exception);
			}
			return empty;
		}

		[WebMethod]
		public string DownloadMaster(string username, string masterItemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							MediaItem item = database.GetItem(masterItemID);
							MediaUrlOptions mediaUrlOption = new MediaUrlOptions()
							{
								AbsolutePath = true,
								AlwaysIncludeServerUrl = true,
								Database = database,
								UseItemPath = false
							};
							empty = MediaManager.GetMediaUrl(item, mediaUrlOption);
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = "FAILED";
				Logger.Error("DownloadMaster", exception);
			}
			return empty;
		}

		[WebMethod]
		public string DownloadMediaItem(string username, string mediaID, int languageIndex)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(mediaID))
			{
				return empty;
			}
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								MediaItem item = database.GetItem(mediaID);
								MediaUrlOptions mediaUrlOption = new MediaUrlOptions()
								{
									AbsolutePath = true,
									AlwaysIncludeServerUrl = true,
									Database = database,
									BackgroundColor = Color.Transparent,
									UseItemPath = false
								};
								empty = MediaManager.GetMediaUrl(item, mediaUrlOption);
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = string.Empty;
				Logger.Error("DownloadMediaItem", exception);
			}
			return empty;
		}

		[WebMethod]
		public string DownloadMediaItemByFieldName(string username, string contentItemID, string fieldName, int languageIndex)
		{
			string str;
			string empty = string.Empty;
			if (string.IsNullOrEmpty(contentItemID) || string.IsNullOrEmpty(fieldName))
			{
				return empty;
			}
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(contentItemID);
								Field field = item.Fields[fieldName];
								if (field == null)
								{
									str = empty;
									return str;
								}
								else if (field.Type.ToLower() == "qr code image")
								{
									PrintOptions printOption = new PrintOptions()
									{
										IsClient = true,
										RootCacheFolder = WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder
									};
									PrintOptions printOption1 = printOption;
									string str1 = ImageRendering.CreateQrOnServer(printOption1, item, field.Value);
									if (!string.IsNullOrEmpty(str1))
									{
										empty = string.Concat(WebConfigHandler.CommonSettings.WebHost, "/PrintStudio/Handlers/PrintCacheHandler.ashx?fileName=", printOption1.FormatResourceLink(str1));
									}
									else
									{
										str = empty;
										return str;
									}
								}
								else if (field.Type.ToLower() == "image")
								{
									ImageField imageField = field;
									if (imageField.MediaItem != null)
									{
										MediaUrlOptions mediaUrlOption = new MediaUrlOptions()
										{
											AbsolutePath = true,
											AlwaysIncludeServerUrl = true,
											Database = database,
											BackgroundColor = Color.Transparent,
											UseItemPath = false
										};
										empty = MediaManager.GetMediaUrl(imageField.MediaItem, mediaUrlOption);
									}
								}
							}
						}
					}
				}
				return empty;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = string.Empty;
				Logger.Error("DownloadMediaItemByFieldName", exception);
				return empty;
			}
			return str;
		}

		[WebMethod]
		public string DownloadMediaItemThumbNail(string username, string mediaID, int languageIndex)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(mediaID))
			{
				return empty;
			}
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								MediaItem item = database.GetItem(mediaID);
								MediaUrlOptions mediaUrlOption = new MediaUrlOptions()
								{
									AbsolutePath = true,
									AlwaysIncludeServerUrl = true,
									Database = database,
									IgnoreAspectRatio = false,
									Width = 120,
									Height = 120,
									BackgroundColor = Color.White,
									Thumbnail = true,
									UseItemPath = false
								};
								empty = MediaManager.GetMediaUrl(item, mediaUrlOption);
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = string.Empty;
				Logger.Error("DownloadMediaItemThumbNail", exception);
			}
			return empty;
		}

		[WebMethod]
		public void DuplicateItem(string username, string sourceID, string targetName, bool includeChildren, int amountOfCopies, int languageIndex)
		{
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							Item item = database.GetItem(sourceID);
							bool flag = item.TemplateName.Equals("P_Page", StringComparison.InvariantCultureIgnoreCase);
							int num = (int.TryParse(item.Parent.Children.Max<Item, string>((Item o) => o["Number"]), out num) ? num + 1 : 1);
							for (int i = 1; i <= amountOfCopies; i++)
							{
								Item value = item.Duplicate(item.Name);
								if (flag)
								{
									value.Editing.BeginEdit();
									value.Fields[FieldIDs.DisplayName].Value = item.Fields[FieldIDs.DisplayName].Value;
									value.Fields["Number"].Value = num.ToString(CultureInfo.InvariantCulture);
									value.Editing.EndEdit();
									num++;
								}
								SitecoreHelper.MoveItem(username, value.ID.ToString(), 4, languageIndex);
								if (!includeChildren)
								{
									value.DeleteChildren();
								}
								else if (value.TemplateName.Equals("P_Snippet", StringComparison.InvariantCultureIgnoreCase) || value.TemplateName.Equals("P_Page", StringComparison.InvariantCultureIgnoreCase))
								{
									SetGroupValue(value);
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("DuplicateItem", exception);
			}
		}
		internal static void SetGroupValue(Item added)
		{
			StringDictionary stringDictionary = new StringDictionary();
			foreach (Item item in
				from d in (IEnumerable<Item>)added.Axes.GetDescendants()
				where d.Fields["GroupID"] != null
				select d)
			{
				string str = item["GroupID"];
				if (string.IsNullOrEmpty(str))
				{
					continue;
				}
				if (!stringDictionary.ContainsKey(str))
				{
					long ticks = DateTime.Now.Ticks;
					stringDictionary.Add(str, ticks.ToString("d"));
				}
				item.Editing.BeginEdit();
				item.Fields["GroupID"].Value = stringDictionary[str];
				item.Editing.EndEdit();
			}
		}

		[WebMethod]
		public string ExecuteTask(string username, string itemID, int languageIndex, string projectPanelItem, string contentBrowserItem, string libraryBrowserItem, string imageViewerItem, string workBoxItem)
		{
			string str = "FAILED";
			if (this.ValidateRequest(username))
			{
				Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
				Item item = database.GetItem(itemID);
				if (item.Template != null & (item.Template == null ? false : item.Template.Name == "Task"))
				{
					try
					{
						Script script = new Script()
						{
							Arguments = new Dictionary<string, object>()
						};
						script.Arguments.Add("ItemID", item.ID.Guid);
						script.Arguments.Add("LanguageIndex", languageIndex);
						script.Arguments.Add("CurrentUserName", username);
						script.Arguments.Add("ci_projectPanel", projectPanelItem);
						script.Arguments.Add("ci_contentBrowser", contentBrowserItem);
						script.Arguments.Add("ci_libraryBrowser", libraryBrowserItem);
						script.Arguments.Add("ci_imageViewer", imageViewerItem);
						script.Arguments.Add("ci_workBox", workBoxItem);
						if (item.Fields["Assembly"].Value != string.Empty)
						{
							str = script.InvokeAMethod(item.Fields["Assembly"].Value, item.Fields["TypeName"].Value, item.Fields["MethodName"].Value, false);
						}
					}
					catch (Exception exception)
					{
						Logger.Error("ExecuteTask", exception);
					}
				}
			}
			return str;
		}

		[WebMethod]
		public string ExecuteWorkflowCommand(string username, string itemId, int languageIndex, string commandId, string comments)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemId);
								IWorkflow workflow = database.WorkflowProvider.GetWorkflow(item);
								WorkflowState state = workflow.GetState(item);
								WorkflowCommand workflowCommand = ((IEnumerable<WorkflowCommand>)workflow.GetCommands(state.StateID)).FirstOrDefault<WorkflowCommand>((WorkflowCommand c) => c.CommandID == commandId);
								if (workflowCommand != null)
								{
									using (SecurityDisabler securityDisabler = new SecurityDisabler())
									{
										string siteName = Sitecore.Context.GetSiteName();
										Sitecore.Context.SetActiveSite("shell");
										WorkflowResult workflowResult = workflow.Execute(workflowCommand.CommandID, item, comments, false, new object[0]);
										empty = workflowResult.Message;
										Sitecore.Context.SetActiveSite(siteName);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = "ERROR";
				Logger.Error("ExecuteWorkflowCommand", exception);
			}
			return empty;
		}

		[WebMethod]
		public string FetchBranchesFromFolder(string username, int languageIndex, string branchType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				if (this.ValidateRequest(username))
				{
					Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
					if (SitecoreHelper.GetApiUser(username) != null)
					{
						using (SecurityDisabler securityDisabler = new SecurityDisabler())
						{
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(WebConfigHandler.PrintStudioEngineSettings.InDesignConnectorDefaultSettings);
								foreach (Item child in database.GetItem(item.Fields[branchType].Value).Children)
								{
									stringBuilder.AppendFormat("{0}|{1}|", child.Name, child.DisplayName.RemoveSpecialCharacters());
								}
								if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '|')
								{
									stringBuilder.Remove(stringBuilder.Length - 1, 1);
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("FetchBranchesFromFolder", exception);
			}
			return stringBuilder.ToString();
		}

		[WebMethod]
		public string FetchImageScalingMethods(string username, int languageIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				if (this.ValidateRequest(username))
				{
					Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
					if (SitecoreHelper.GetApiUser(username) != null)
					{
						using (SecurityDisabler securityDisabler = new SecurityDisabler())
						{
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								foreach (Item child in database.GetItem("/sitecore/templates/Print Studio Templates/System/ListValues/ImageScalingTypes").Children)
								{
									stringBuilder.AppendFormat("{0}|{1}|", child.Name, child.DisplayName.RemoveSpecialCharacters());
								}
								if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '|')
								{
									stringBuilder.Remove(stringBuilder.Length - 1, 1);
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("FetchImageScalingMethods", exception);
			}
			return stringBuilder.ToString();
		}

		[WebMethod]
		public string FetchOwnerOfItem(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							try
							{
								Item item = database.GetItem(itemID);
								if (!item.State.CanEdit() || item.Locking.IsLocked())
								{
									empty = item.Locking.GetOwner();
								}
							}
							catch (Exception exception)
							{
								Logger.Error("FetchOwnerOfItem", exception);
							}
						}
					}
				}
			}
			return empty;
		}

		[WebMethod]
		public string FetchStartNumberFirstPage(string username, int languageIndex, string itemId)
		{
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			return ServiceHelper.FetchStartNumberFirstPage(username, languageIndex, itemId);
		}

		[WebMethod]
		public string FetchTextItemPattern(string username, string itemID, int languageIndex, string destination)
		{
			string empty = string.Empty;
			if (this.ValidateRequest(username))
			{
				Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
				LanguageCollection languages = LanguageManager.GetLanguages(database);
				Item item = database.GetItem(itemID, languages[languageIndex]);
				SitecoreHelper.ScanLanguageVersions(username, item, languageIndex, database);
				XmlWriter xmlTextWriter = null;
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							string sessionID = WebUtil.GetSessionID();
							string str = string.Concat(Sitecore.Context.User.Name, (!string.IsNullOrEmpty(sessionID) ? string.Concat("_", sessionID.Substring(0, 8)) : string.Empty), "\\");
							string str1 = Path.Combine(WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder, str);
							str1 = Path.Combine(str1, Sitecore.Context.Language.Name);
							Guid guid = item.ID.ToGuid();
							str1 = Path.Combine(str1, guid.ToString());
							try
							{
								if (!Directory.Exists(str1))
								{
									Directory.CreateDirectory(str1);
								}
								str1 = Path.Combine(str1, string.Concat(item.ID, ".xml"));
								xmlTextWriter = new XmlTextWriter(str1, Encoding.UTF8);
								xmlTextWriter.WriteStartDocument(true);
								xmlTextWriter.WriteStartElement("Temp");
								xmlTextWriter.WriteRaw(SitecoreHelper.FetchFieldValue(item, "Content", database));
								xmlTextWriter.WriteEndElement();
								empty = string.Concat(WebConfigHandler.CommonSettings.WebHost, "/PrintStudio/Handlers/PrintCacheHandler.ashx?fileName=", str1.Replace(RenderItemHelper.EnsureFolderPath(WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder), string.Empty));
							}
							catch (Exception exception)
							{
								Logger.Error("FetchTextItemPattern: ", exception);
								empty = string.Empty;
							}
						}
					}
				}
				if (xmlTextWriter != null)
				{
					xmlTextWriter.Close();
				}
			}
			return empty;
		}

		[WebMethod]
		public string GetDataFieldsValues(string userName, string itemID, string fieldnames, int languageIndex, string fieldSeperator)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(userName))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(userName);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								string[] strArrays = fieldnames.Split(fieldSeperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
								for (int i = 0; i < (int)strArrays.Length; i++)
								{
									string str = strArrays[i];
									empty = (!string.IsNullOrEmpty(SitecoreHelper.FetchFieldValue(item, str, database)) ? string.Concat(empty, SitecoreHelper.FetchFieldValue(item, str, database).Replace(fieldSeperator, string.Empty)) : string.Concat(empty, "null"));
									empty = string.Concat(empty, fieldSeperator);
								}
								if (empty.EndsWith(fieldSeperator))
								{
									empty = empty.Remove(empty.Length - 1, 1);
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetDataFieldValue", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetDataFieldValue(string username, string itemID, string fieldname, int languageIndex)
		{
			string str;
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item != null)
								{
									empty = SitecoreHelper.FetchFieldValue(item, fieldname, database);
								}
								else
								{
									str = empty;
									return str;
								}
							}
						}
					}
					return empty;
				}
				else
				{
					str = empty;
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetDataFieldValue", exception);
				return empty;
			}
			return str;
		}

		[WebMethod]
		public string GetDefaultSettingRootItemId(string username, string fieldName)
		{
			return SitecoreHelper.GetDefaultSettingRootItemId(fieldName);
		}

		[WebMethod]
		public string GetFrameChildId(string username, string itemId, string itemType, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemId);
								if (item != null)
								{
									foreach (Item child in item.Children)
									{
										if (!child.TemplateName.Equals(itemType, StringComparison.CurrentCultureIgnoreCase))
										{
											continue;
										}
										empty = child.ID.ToString();
										break;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetFrameChildID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetImageFrameChildID(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item.TemplateName.Equals("P_ImageFrame"))
								{
									foreach (Item child in item.Children)
									{
										if (!child.TemplateName.Equals("P_Image"))
										{
											continue;
										}
										empty = child.ID.ToString();
										break;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetImageFrameChildID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetImagesIDsNames(string username, string parentID, int languageIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								foreach (Item child in database.GetItem(parentID).Children)
								{
									if (!(string.Compare(child.Template.Name, "Print Studio Image", StringComparison.InvariantCultureIgnoreCase) == 0 | (child.Template.Name.ToLower() == "jpeg") | (child.Template.Name.ToLower() == "image")))
									{
										continue;
									}
									stringBuilder.AppendFormat("{0}|{1};", child.ID, child.DisplayName.RemoveSpecialCharacters());
								}
								if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == ';')
								{
									stringBuilder.Remove(stringBuilder.Length - 1, 1);
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				stringBuilder.Remove(0, stringBuilder.Length);
				Logger.Error("GetImageIdsAndNames", exception);
			}
			return stringBuilder.ToString();
		}

		[WebMethod]
		public string GetImagesLibraryStartNodeId(string username, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						Item item = database.GetItem(WebConfigHandler.PrintStudioEngineSettings.InDesignConnectorDefaultSettings);
						Item item1 = database.GetItem(item["Images viewer start node"]);
						if (item1.Template.Name.ToLower() == "Media folder".ToLower())
						{
							empty = item1.ID.ToString();
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetImagesLibraryStartNodeId", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetItemDisplayNameByID(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item != null)
								{
									empty = item.Fields[FieldIDs.DisplayName].Value;
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetItemDisplayNameByID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetItemNameByID(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item != null)
								{
									empty = item.Name;
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetItemNameByID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetItemTemplateID(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item != null && item.Template != null)
								{
									empty = item.Template.ID.ToString();
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetItemTemplateID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetItemTemplateName(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								if (item != null && item.Template != null)
								{
									empty = item.Template.Name;
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetItemTemplateName", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetItemWorkflowInfo(string username, string itemId, int languageIndex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							try
							{
								Item item = database.GetItem(itemId);
								IWorkflow workflow = database.WorkflowProvider.GetWorkflow(item);
								stringBuilder.Append(workflow.Appearance.DisplayName.RemoveSpecialCharacters());
								stringBuilder.Append("|");
								string displayName = "null";
								WorkflowState state = null;
								if (!string.IsNullOrEmpty(item[FieldIDs.WorkflowState]))
								{
									state = workflow.GetState(item[FieldIDs.WorkflowState]);
									if (state != null)
									{
										displayName = state.DisplayName;
									}
								}
								stringBuilder.Append(displayName.RemoveSpecialCharacters());
								stringBuilder.Append("|");
								try
								{
									WorkflowEvent[] history = workflow.GetHistory(item);
									int length = (int)history.Length;
									if (length <= 0)
									{
										stringBuilder.Append("null");
										stringBuilder.Append("|");
										stringBuilder.Append("null");
										stringBuilder.Append("|");
										stringBuilder.Append("null");
										stringBuilder.Append("|");
										stringBuilder.Append("null");
										stringBuilder.Append("|");
										stringBuilder.Append("null");
										stringBuilder.Append("|");
									}
									else
									{
										WorkflowEvent workflowEvent = history[length - 1];
										Item item1 = database.GetItem(workflowEvent.OldState);
										Item item2 = database.GetItem(workflowEvent.NewState);
										stringBuilder.Append(item1.DisplayName.RemoveSpecialCharacters());
										stringBuilder.Append("|");
										stringBuilder.Append(item2.DisplayName.RemoveSpecialCharacters());
										stringBuilder.Append("|");
										stringBuilder.Append(workflowEvent.User.RemoveSpecialCharacters());
										stringBuilder.Append("|");
										stringBuilder.Append(workflowEvent.Date.ToShortDateString());
										stringBuilder.Append("|");
										stringBuilder.Append((workflowEvent.Text != null ? workflowEvent.Text.RemoveSpecialCharacters() : "null"));
										stringBuilder.Append("|");
									}
								}
								catch (Exception exception1)
								{
									Exception exception = exception1;
									stringBuilder.Append("null");
									stringBuilder.Append("|");
									stringBuilder.Append("null");
									stringBuilder.Append("|");
									stringBuilder.Append("null");
									stringBuilder.Append("|");
									stringBuilder.Append("null");
									stringBuilder.Append("|");
									stringBuilder.Append("null");
									stringBuilder.Append("|");
									Logger.Error("GetItemWorkflowInfo", exception);
								}
								if (!item.State.CanEdit() || item.Locking.IsLocked())
								{
									stringBuilder.Append(item.Locking.GetOwner().RemoveSpecialCharacters());
									stringBuilder.Append("|");
								}
								else if (item.State.CanEdit() || !item.Locking.IsLocked())
								{
									stringBuilder.Append("null");
									stringBuilder.Append("|");
								}
								string empty = "null";
								try
								{
									if (state != null)
									{
										WorkflowCommand[] commands = workflow.GetCommands(state.StateID);
										if ((int)commands.Length > 0)
										{
											empty = string.Empty;
											empty = (
												from c in (IEnumerable<WorkflowCommand>)commands
												where !c.DisplayName.Contains("__")
												select c).Aggregate<WorkflowCommand, string>(empty, (string current, WorkflowCommand c) => string.Concat(current, string.Format("{0}|{1}|", c.CommandID.RemoveSpecialCharacters(), c.DisplayName.RemoveSpecialCharacters())));
											if (empty.EndsWith("|"))
											{
												try
												{
													empty = empty.Substring(0, empty.Length - 1);
												}
												catch
												{
													Logger.Error("GetItemWorkflowInfo: Substring failed", null);
												}
											}
										}
									}
								}
								catch
								{
									empty = string.Concat(empty, "null");
								}
								stringBuilder.Append(empty);
							}
							catch
							{
								Logger.Error("GetItemWorkflowInfo: item does not have any workflow attached", null);
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		[WebMethod]
		public string GetLanguageAsString(string username)
		{
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							foreach (Language language in LanguageManager.GetLanguages(Factory.GetDatabase(WebConfigHandler.CommonSettings.Database)))
							{
								string displayName = language.CultureInfo.DisplayName;
								char[] chrArray = new char[] { '|' };
								stringBuilder.AppendFormat("{0}|", displayName.Trim(chrArray));
							}
							if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '|')
							{
								stringBuilder.Remove(stringBuilder.Length - 1, 1);
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetLanguageAsString", exception);
			}
			return stringBuilder.ToString();
		}

		[WebMethod]
		public string GetLanguageByIndex(string username, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							empty = languages[languageIndex].CultureInfo.DisplayName;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = exception.Message;
				Logger.Error("GetLanguageByIndex", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetLibrariesSettingsItemId(string username, int languageIndex, int callerType)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						if (callerType == 1)
						{
							empty = database.GetItem(SitecoreHelper.GetBrowserSettingsItemPath(database, "Library browser")).ID.ToString();
						}
						if (callerType == 2)
						{
							empty = database.GetItem(SitecoreHelper.GetBrowserSettingsItemPath(database, "Content browser")).ID.ToString();
						}
						if (callerType == 3)
						{
							empty = database.GetItem(SitecoreHelper.GetBrowserSettingsItemPath(database, "Extensions browser")).ID.ToString();
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetLibrariesSettingsItemId", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetMediaItemIDByFieldName(string username, string contentItemID, string fieldName, int languageIndex)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(contentItemID) || string.IsNullOrEmpty(fieldName))
			{
				return empty;
			}
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								ImageField item = database.GetItem(contentItemID).Fields[fieldName];
								if (item != null && item.MediaItem != null)
								{
									empty = item.MediaID.ToString();
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("DownloadMediaItemByFieldName", exception);
			}
			return empty;
		}

		[WebMethod]
		public int GetNrOfChildren(string username, string itemID, int languageIndex, out string returnStr)
		{
			int count = -1;
			returnStr = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								count = database.GetItem(itemID).Children.Count;
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				returnStr = exception.Message;
				Logger.Error("GetNrOfChildren", exception);
			}
			return count;
		}

		[WebMethod]
		public int GetNumberOfLinksForItem(string username, string itemID, int languageIndex)
		{
			int length = 0;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								length = (int)Globals.LinkDatabase.GetReferrers(item).Length;
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetNumberOfLinksForItem", exception);
			}
			return length;
		}

		[WebMethod]
		public string GetParentID(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								empty = item.Parent.ID.ToString();
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("GetParentID", exception);
			}
			return empty;
		}

		[WebMethod]
		public string GetProjectsFolder(string username)
		{
			return WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder;
		}

		[WebMethod]
		public int GetUserDefaultLanguage(string username)
		{
			int num = -1;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							Language defaultLanguage = LanguageManager.DefaultLanguage;
							string contentLanguage = apiUser.Profile.ContentLanguage;
							if (!string.IsNullOrEmpty(contentLanguage))
							{
								defaultLanguage = LanguageManager.GetLanguage(contentLanguage, database);
							}
							int num1 = 0;
							foreach (Language language in languages)
							{
								if (!language.Equals(defaultLanguage))
								{
									num1++;
								}
								else
								{
									num = num1;
									break;
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				num = -1;
				Logger.Error("GetDefaultLanguage", exception);
			}
			return num;
		}

		[WebMethod]
		public bool HasField(string username, string itemID, string fieldName, int languageIndex)
		{
			bool item = false;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item1 = database.GetItem(itemID);
								item = item1.Fields[fieldName] != null;
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("HasField", exception);
			}
			return item;
		}

		[WebMethod]
		public bool IsItemNameValid(string username, string name)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				flag = Regex.IsMatch(name, Settings.ItemNameValidation, RegexOptions.ECMAScript);
			}
			return flag;
		}

		[WebMethod(EnableSession = true)]
		public int LogIn(string username, string password)
		{
			int num = -5;
			try
			{
				if (InDesignConnectorService.ValidateLicense(out num))
				{
					Domain domain = Factory.GetDomain(WebConfigHandler.CommonSettings.Domain);
					string fullName = domain.GetFullName(username);
					if (!Sitecore.Security.Accounts.User.Exists(fullName) || !AuthenticationManager.Login(fullName, password))
					{
						Logger.Warn(string.Concat("User does not exist: ", fullName), null);
						num = -5;
					}
					else
					{
						num = (InDesignConnectorService.NewUserAllowed() ? 0 : -6);
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("LogIn", exception);
			}
			return num;
		}

		[WebMethod]
		public void LogOut(string username)
		{
			if (this.ValidateRequest(username))
			{
				Sitecore.Context.Logout();
				HttpCookie item = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"];
				if (item != null && !string.IsNullOrEmpty(item.Value))
				{
					DomainAccessGuard.Kick(item.Value);
				}
			}
		}

		[WebMethod]
		public string LookupMasterDocument(string username, string itemID, int languageIndex)
		{
			string empty = string.Empty;
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			if (database != null)
			{
				LanguageCollection languages = LanguageManager.GetLanguages(database);
				using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
				{
					Item item = database.GetItem(itemID);
					Item item1 = null;
					if (item != null)
					{
						SitecoreHelper.LookupDocument(item, ref item1);
						if (item1 != null)
						{
							empty = item1.Fields["Reference"].Value;
							Logger.Info(string.Concat("LookupMasterDocument, masterID: ", empty), null);
						}
					}
				}
			}
			return empty;
		}

		[WebMethod]
		public string LookupProject(string username, string itemID, int languageIndex)
		{
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			return SitecoreHelper.LookupProject(itemID, languageIndex, database);
		}

		[WebMethod]
		public string MoveItem(string username, string itemID, int movetype, int languageIndex)
		{
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			return SitecoreHelper.MoveItem(username, itemID, movetype, languageIndex);
		}

		private static bool NewUserAllowed()
		{
			if (!DomainAccessGuard.HasAccess())
			{
				if (!DomainAccessGuard.IsNewUserAllowed())
				{
					return false;
				}
				DomainAccessGuard.Login(HttpContext.Current.Session.SessionID, Sitecore.Context.User.Name);
			}
			return true;
		}

		[WebMethod]
		public int ProcessCopyAndPasteCommand(string username, int languageIndex, string sourceId, string targetId, string copyPasteCommand)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return CutCopyPasteHelper.ProcessCopyAndPasteCommandLibBrowser(username, languageIndex, sourceId, targetId, copyPasteCommand);
		}

		[WebMethod]
		public int ProcessCopyAndPasteCommandContentBrowser(string username, int languageIndex, string sourceId, string targetId, string copyPasteCommand)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return CutCopyPasteHelper.ProcessCopyAndPasteCommandContentBrowser(username, languageIndex, sourceId, targetId, copyPasteCommand);
		}

		[WebMethod]
		public int ProcessCopyAndPasteCommandExtensionBrowser(string username, int languageIndex, string sourceId, string targetId, string copyPasteCommand)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return CutCopyPasteHelper.ProcessCopyAndPasteCommandExtensionBrowser(username, languageIndex, sourceId, targetId, copyPasteCommand);
		}

		[WebMethod]
		public int ProcessDragAndDropCommand(string username, int languageIndex, string sourceId, string targetId)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return DragAndDropHelper.ProcessDragAndDropCommand(username, languageIndex, sourceId, targetId);
		}

		[WebMethod]
		public string RenderItem(string username, string itemId, string renderId, string frameType, string contentItemId, string contentFieldName, int languageIndex)
		{
			string empty = string.Empty;
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			using (DatabaseSwitcher databaseSwitcher = new DatabaseSwitcher(database))
			{
				Domain domain = Factory.GetDomain(WebConfigHandler.CommonSettings.Domain);
				string fullName = domain.GetFullName(username);
				Sitecore.Security.Accounts.User user = Sitecore.Context.User;
				if (Sitecore.Security.Accounts.User.Exists(fullName))
				{
					user = Sitecore.Security.Accounts.User.FromName(fullName, true);
				}
				user = user ?? Sitecore.Context.User;
				using (UserSwitcher userSwitcher = new UserSwitcher(user))
				{
					LanguageCollection languages = LanguageManager.GetLanguages(database);
					if (languageIndex > -1 && languages[languageIndex] != null)
					{
						Language item = languages[languageIndex];
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(item))
						{
							try
							{
								Item standardValues = null;
								bool flag = false;
								if (!string.IsNullOrEmpty(itemId))
								{
									standardValues = database.GetItem(ID.Parse(itemId.Replace("[WB]", string.Empty)), item);
								}
								else if (!string.IsNullOrEmpty(frameType))
								{
									TemplateItem item1 = database.GetItem(string.Concat(WebConfigHandler.PrintStudioEngineSettings.EngineTemplates, frameType));
									standardValues = item1.StandardValues;
									flag = true;
								}
								if (standardValues != null)
								{
									SafeDictionary<string, object> strs = new SafeDictionary<string, object>();
									if (!string.IsNullOrEmpty(contentFieldName))
									{
										strs.Add("ContentFieldName", contentFieldName);
									}
									empty = RenderItem(renderId, contentItemId, contentFieldName, flag, standardValues, strs);
								}
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								Logger.Error(string.Concat("RenderText ", exception.Message), null);
							}
						}
					}
				}
			}
			return empty;
		}

		internal static string RenderItem(string rendererId, string contentItemId, string contentFieldName, bool standardValuesItem, Item renderingItem, SafeDictionary<string, object> parameters)
		{
			string empty = string.Empty;
			InDesignItemRendererBase renderer = PrintFactory.GetRenderer(renderingItem, rendererId, parameters);
			if (renderer == null)
			{
				return empty;
			}
			PrintOptions printOption = new PrintOptions()
			{
				IsClient = true,
				RootCacheFolder = WebConfigHandler.PrintStudioEngineSettings.ProjectsFolder
			};
			PrintOptions printOption1 = printOption;
			PrintContext printContext = new PrintContext(renderingItem, printOption1);
			if (!string.IsNullOrEmpty(contentItemId))
			{
				renderer.DataSource = contentItemId;
			}
			renderer.RenderDeep = true;
			XElement xElement = new XElement("indesignxml");
			renderer.Render(printContext, xElement);
			if (xElement.Elements().Count<XElement>() == 1)
			{
				xElement = xElement.Elements().ElementAt<XElement>(0);
			}
			string str = renderingItem.ID.Guid.ToString();
			if (standardValuesItem)
			{
				xElement.SetAttributeValue("SitecoreID", string.Empty);
				if (!string.IsNullOrEmpty(contentItemId))
				{
					str = contentItemId;
				}
			}
			if (!string.IsNullOrEmpty(rendererId))
			{
				xElement.SetAttributeValue("RenderingID", rendererId);
			}
			xElement.SetAttributeValue("SitecoreFieldname", contentFieldName);
			xElement.SetAttributeValue("ItemReferenceID", contentItemId);
			string str1 = Path.Combine(printOption1.CacheFolder, string.Concat(str, ".xml"));
			RenderItemHelper.OutputToFile(str1, xElement);
			string fileVal = Settings.GetSetting("PrintStudio.UseSampleFile");
            bool useFile = System.Convert.ToBoolean(fileVal);
			string filePath = (useFile) ? "sample.xml" : printOption1.FormatResourceLink(str1);
			empty = string.Concat(WebConfigHandler.CommonSettings.WebHost, "/PrintStudio/Handlers/PrintCacheHandler.ashx?fileName=", filePath);
			return empty;
		}

		[WebMethod]
		public string RenderText(string username, string itemId, string fieldName, int languageIndex)
		{
			return this.RenderItem(username, string.Empty, string.Empty, "P_TextFrame", itemId, fieldName, languageIndex);
		}

		[WebMethod]
		public string RenderTextFrame(string username, string renderId, string frameType, string textFrameContent, int languageIndex)
		{
			string empty = string.Empty;
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
			using (DatabaseSwitcher databaseSwitcher = new DatabaseSwitcher(database))
			{
				Domain domain = Factory.GetDomain(WebConfigHandler.CommonSettings.Domain);
				string fullName = domain.GetFullName(username);
				Sitecore.Security.Accounts.User user = Sitecore.Context.User;
				if (Sitecore.Security.Accounts.User.Exists(fullName))
				{
					user = Sitecore.Security.Accounts.User.FromName(fullName, true);
				}
				user = user ?? Sitecore.Context.User;
				using (UserSwitcher userSwitcher = new UserSwitcher(user))
				{
					LanguageCollection languages = LanguageManager.GetLanguages(database);
					if (languageIndex > -1 && languages[languageIndex] != null)
					{
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							try
							{
								Item standardValues = null;
								if (!string.IsNullOrEmpty(frameType))
								{
									TemplateItem item = database.GetItem(string.Concat(WebConfigHandler.PrintStudioEngineSettings.EngineTemplates, frameType));
									standardValues = item.StandardValues;
								}
								if (standardValues != null)
								{
									SafeDictionary<string, object> strs = new SafeDictionary<string, object>();
									if (!string.IsNullOrEmpty(textFrameContent))
									{
										strs.Add("InDesignContent", textFrameContent);
									}
									empty = RenderItem(renderId, string.Empty, string.Empty, true, standardValues, strs);
								}
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								Logger.Error(string.Concat("RenderText ", exception.Message), null);
							}
						}
					}
				}
			}
			return empty;
		}

		[WebMethod]
		public bool RenumberPagesInDocument(string username, int languageIndex, string itemId, string startNumber)
		{
			bool flag = false;
			if (this.ValidateRequest(username))
			{
				Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
				if (apiUser != null)
				{
					int num = System.Convert.ToInt32(startNumber);
					if (num <= 0)
					{
						num = 1;
					}
					using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						LanguageCollection languages = LanguageManager.GetLanguages(database);
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							try
							{
								foreach (Item child in database.GetItem(itemId).Children)
								{
									foreach (Item str in child.Children)
									{
										str.Editing.BeginEdit();
										str.Fields["Number"].Value = num.ToString();
										str.Editing.EndEdit();
										num++;
									}
								}
								flag = true;
							}
							catch (Exception exception)
							{
								Logger.Error("RenumberPagesInDocument", exception);
							}
						}
					}
				}
			}
			return flag;
		}

		[WebMethod]
		public bool RenumberPagesInFolder(string username, int languageIndex, string itemId, string startNumber)
		{
			if (!this.ValidateRequest(username))
			{
				return false;
			}
			return ServiceHelper.RenumberPagesInFolder(username, languageIndex, itemId, startNumber);
		}

		[WebMethod]
		public string SaveMediaDocumentByteArray(string username, byte[] file, string fileName, int languageIndex, string mediaItemId)
		{
			string empty = string.Empty;
			if (this.ValidateRequest(username) && SitecoreHelper.GetApiUser(username) != null)
			{
				try
				{
					using (SecurityDisabler securityDisabler = new SecurityDisabler())
					{
						Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
						string longID = database.GetItem(mediaItemId).Parent.Paths.LongID;
						LanguageCollection languages = LanguageManager.GetLanguages(Factory.GetDatabase(WebConfigHandler.CommonSettings.Database));
						using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
						{
							empty = ServiceHelper.CreateMediaItem(file, fileName, database, longID, username, mediaItemId);
						}
					}
				}
				catch (Exception exception)
				{
					Logger.Error("SaveMediaDocument", exception);
				}
			}
			return empty;
		}

		[WebMethod]
		public string SearchImagesIdsAndNames(string username, string itemID, string searchValue, int languageIndex)
		{
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			return ServiceHelper.SearchImagesIdsAndNames(username, itemID, searchValue, languageIndex);
		}

		[WebMethod]
		public string SetAttributesByXml(string username, string itemId, string attributeXml, int languageIndex)
		{
			if (!this.ValidateRequest(username))
			{
				return string.Empty;
			}
			return ServiceHelper.SetAttributesByXml(username, itemId, attributeXml, languageIndex);
		}

		[WebMethod]
		public int SetDataFieldsValues(string username, string itemID, string fieldnames, string values, int languageIndex, string fieldSeperator)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return ServiceHelper.SetDataFieldValue(username, itemID, fieldnames, values, languageIndex, fieldSeperator, string.Empty);
		}

		[WebMethod]
		public int SetDataFieldValue(string username, string itemID, string fieldname, string value, int languageIndex)
		{
			int num = 0;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								if (!string.IsNullOrEmpty(itemID))
								{
									Item item = database.GetItem(itemID);
									if (item != null)
									{
										item.Editing.BeginEdit();
										num = ServiceHelper.SetTheField(item, fieldname, value);
										item.Editing.EndEdit();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				num = -1;
				Logger.Error("SetDataFieldValue", exception);
			}
			return num;
		}

		[WebMethod]
		public int SetItemDisplayName(string username, string itemID, string value, int languageIndex)
		{
			int num = 0;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								using (EditContext editContext = new EditContext(item))
								{
									if (item.Fields[FieldIDs.DisplayName] != null)
									{
										item.Fields[FieldIDs.DisplayName].Value = value.RemoveSpecialCharacters();
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				num = -1;
				Logger.Error("SetItemName", exception);
			}
			return num;
		}

		[WebMethod]
		public int SetItemName(string username, string itemID, string value, int languageIndex)
		{
			int num = 0;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								Item item = database.GetItem(itemID);
								using (EditContext editContext = new EditContext(item))
								{
									if (!this.IsItemNameValid(username, value))
									{
										num = -1;
									}
									else
									{
										item.Name = value;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				num = -1;
				Logger.Error("SetItemName", exception);
			}
			return num;
		}

		[WebMethod]
		public string SetItemsByXml(string username, string itemsXml, int languageIndex)
		{
			string empty = string.Empty;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.LoadXml(itemsXml);
								XmlNode documentElement = xmlDocument.DocumentElement;
								if (documentElement != null)
								{
									XmlNodeList xmlNodeLists = documentElement.SelectNodes("// Item");
									if (xmlNodeLists != null)
									{
										foreach (XmlNode xmlNodes in xmlNodeLists)
										{
											if (xmlNodes.Attributes == null)
											{
												continue;
											}
											string value = xmlNodes.Attributes["itemId"].Value;
											if (string.IsNullOrEmpty(value))
											{
												continue;
											}
											empty = ServiceHelper.SetAttributesByXml(username, value, xmlNodes.InnerXml, languageIndex);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				empty = exception.Message;
				Logger.Error("SetItemsByXML", exception);
			}
			return empty;
		}

		[WebMethod]
		public int SetItemsContentByXml(string username, string itemsXml, int languageIndex)
		{
			int num = 0;
			try
			{
				if (this.ValidateRequest(username))
				{
					Sitecore.Security.Accounts.User apiUser = SitecoreHelper.GetApiUser(username);
					if (apiUser != null)
					{
						using (UserSwitcher userSwitcher = new UserSwitcher(apiUser))
						{
							Database database = Factory.GetDatabase(WebConfigHandler.CommonSettings.Database);
							LanguageCollection languages = LanguageManager.GetLanguages(database);
							using (LanguageSwitcher languageSwitcher = new LanguageSwitcher(languages[languageIndex]))
							{
								XmlDocument xmlDocument = new XmlDocument();
								xmlDocument.LoadXml(itemsXml);
								XmlNode documentElement = xmlDocument.DocumentElement;
								if (documentElement != null)
								{
									XmlNodeList xmlNodeLists = documentElement.SelectNodes("// Item");
									if (xmlNodeLists != null)
									{
										foreach (XmlNode xmlNodes in xmlNodeLists)
										{
											if (xmlNodes.Attributes == null)
											{
												continue;
											}
											string value = xmlNodes.Attributes["itemId"].Value;
											string innerXml = xmlNodes.InnerXml;
											if (string.IsNullOrEmpty(value))
											{
												continue;
											}
											Item item = database.GetItem(value);
											if (item.TemplateName != "P_TextFrame")
											{
												continue;
											}
											if (!string.IsNullOrEmpty(item.Fields["Item Reference"].Value) || !string.IsNullOrEmpty(item.Fields["Item Field"].Value))
											{
												if (!ServiceHelper.CheckIfReferencedItemCanBeEdited(username, value, languageIndex))
												{
													continue;
												}
												num = ServiceHelper.UpdateTextFrameContent(username, value, innerXml, languageIndex);
												if (!(num == 1 | num == 2))
												{
													continue;
												}
												num++;
											}
											else
											{
												if (item.Children.Count <= 0)
												{
													continue;
												}
												Item item1 = item.Children[0];
												if (!item.Children[0].TemplateName.Equals("P_Text"))
												{
													continue;
												}
												item1.Editing.BeginEdit();
												item1.Fields["Content"].Value = innerXml;
												item1.Editing.EndEdit();
												num++;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				Logger.Error("SetItemsContentByXML", exception);
			}
			return num;
		}

		[WebMethod]
		public int UpdateContent(string username, string frameID, string value, int languageIndex)
		{
			if (!this.ValidateRequest(username))
			{
				return 0;
			}
			return ServiceHelper.UpdateTextFrameContent(username, frameID, value, languageIndex);
		}

		private static bool ValidateLicense(out int result)
		{
			CheckResult checkResult = CheckResult.NotPresent;
			if (License.HasModule("Sitecore.AdaptivePrint"))
			{
				checkResult = License.LicenseStatus("Sitecore.AdaptivePrint");
			}
			if (License.HasModule("Sitecore.Print.Runtime"))
			{
				checkResult = License.LicenseStatus("Sitecore.Print.Runtime");
			}
			result = (int)checkResult;
			return checkResult == CheckResult.Valid;
		}

		protected bool ValidateRequest(string username)
		{
			int num;
			if (Sitecore.Context.IsLoggedIn && Sitecore.Context.User.LocalName.Equals(username, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
			if (InDesignConnectorService.ValidateLicense(out num))
			{
				return true;
			}
			Logger.Info("Invalid license", null);
			return false;
		}
	}
}
