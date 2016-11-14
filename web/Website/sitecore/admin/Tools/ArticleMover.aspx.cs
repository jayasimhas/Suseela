using Informa.Library.Article.Search;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.sitecore.admin.Tools
{
	public partial class ArticleMover : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void bntStart_Click(object sender, EventArgs e)
		{
			try
			{
				lblError.Text = string.Empty;

				if (IsValidInput() == false)
					return;

				ArticleMovingParameters parameters = new ArticleMovingParameters();
				parameters.SourceType = SourceType.ArticleIDs;// rdMoveByArticleIDs.Checked ? SourceType.ArticleIDs : SourceType.FolderIDs;
				parameters.SourceIDs = txtIDs.Text.Trim();
				parameters.SourceIDType = rdIsItemIDs.Checked ? SourceIDType.ItemID : SourceIDType.ArticleID;

				parameters.DestinationType = rdPreserveDateFoldersHier.Checked ? DestinationType.PreserveFolderHierarchy : DestinationType.ToSpecificFolderID;
				parameters.DestinationMainArticlesFolder = rdPreserveDateFoldersHier.Checked ? txtDestinationArticlesFolder.Text.Trim() : string.Empty;
				parameters.DestinationFolderID = rdMoveToFolderID.Checked ? txtToFolderID.Text.Trim() : string.Empty;
				//parameters.MoveChildrenWithoutParentFolder = rdMoveToFolderID.Checked ? chkWithoutParentFolder.Checked : false;

				parameters.ClearTaxonomy = chkClearTaxonomy.Checked;
				parameters.NewTaxonomyIDsToAdd = chkNewTaxonomyFields.Checked ? txtNewTaxonomyFields.Text.Trim() : string.Empty;

				parameters.PublishDestinationItems = chkPublishDestination.Checked;

				string resultStatus;
				bool result = ArticleMoverUtility.Process(parameters, out resultStatus);

				if (result)
				{
					lblError.ForeColor = System.Drawing.Color.Green;
					lblError.Text = resultStatus;
				}
				else
				{
					lblError.ForeColor = System.Drawing.Color.Red;
					lblError.Text = resultStatus;
				}
			}
			catch (Exception ex)
			{
				lblError.Text = "An error has occured: " + ex.ToString();
			}
		}

		protected void btnReset_Click(object sender, EventArgs e)
		{
			try
			{
				rdIsItemIDs.Checked = true;
				txtIDs.Text = string.Empty;

				rdMoveToFolderID.Checked = false;
				rdPreserveDateFoldersHier.Checked = true;
				txtDestinationArticlesFolder.Text = string.Empty;
				txtToFolderID.Text = string.Empty;

				chkClearTaxonomy.Checked = false;
				chkNewTaxonomyFields.Checked = false;
				chkPublishDestination.Checked = false;
				txtNewTaxonomyFields.Text = string.Empty;
				lblError.Text = string.Empty;
			}
			catch (Exception ex)
			{
				lblError.Text = "An error has occured: " + ex.ToString();
			}

		}

		private bool IsValidInput()
		{
			lblError.ForeColor = System.Drawing.Color.Red;
			var masterDb = Sitecore.Data.Database.GetDatabase("master");
			if (txtIDs.Text.Trim() == string.Empty)
			{
				lblError.Text = "Please specify source ID(s)";
				txtIDs.Focus();
				return false;
			}

			if (rdIsItemIDs.Checked)
			{
				string idStatus = string.Empty;
				foreach (var itemID in txtIDs.Text.Split('|'))
				{
					Guid tempGuid;
					if (Guid.TryParse(itemID, out tempGuid) == false)
						idStatus = "Item ID " + itemID + ": is not a valid Item ID <br/>";
					else
					{
						var temp = masterDb.GetItem(new ID(new Guid(itemID)));
						if (temp == null)
							idStatus = "Item ID " + itemID + ": does not exist <br/>";
						else if (temp.TemplateID.Guid.Equals(new Guid(Constants.ArticlesTemplateID)) == false)
							idStatus = "Item ID " + itemID + ": is not an article <br/>";
					}
				}

				if (string.IsNullOrEmpty(idStatus) == false)
				{
					lblError.Text = idStatus;
					return false;
				}
			}
			else
			{
				string idStatus = string.Empty;
				List<string> articleNumbers = txtIDs.Text.Split('|').Select(s => s.Trim()).ToList();

				var articles = ArticleMoverUtility.SearchArticlesByArticleNumbers(articleNumbers);
				var missingArticleNumbers = articleNumbers.Where(w => articles.Exists(e => e.Article_Number == w) == false);

				idStatus = "Following Article ID(s) do not exist: " + string.Join(", ", missingArticleNumbers);


				//foreach (var itemID in txtIDs.Text.Split('|'))
				//{
				//using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
				//{
				//	using (new DatabaseSwitcher(masterDb))
				//	{
				//		IArticleSearch articleSearch = scope.Resolve<IArticleSearch>();
				//		IArticleSearchFilter filter = articleSearch.CreateFilter();
				//		filter.ArticleNumbers = new List<string> { itemID };
				//		var results = articleSearch.Search(filter);

				//		if (results.Articles?.Any() == false)
				//			idStatus = "Article ID " + itemID + ": does not exist";
				//	}
				//}
				//}

				if (string.IsNullOrEmpty(idStatus) == false)
				{
					lblError.Text = idStatus;
					return false;
				}
			}

			if (rdPreserveDateFoldersHier.Checked && txtDestinationArticlesFolder.Text.Trim() == string.Empty)
			{
				lblError.Text = "Please specify destination main Articles Folder ID";
				txtDestinationArticlesFolder.Focus();
				return false;
			}

			if (rdPreserveDateFoldersHier.Checked && masterDb.GetItem(txtDestinationArticlesFolder.Text) == null)
			{
				lblError.Text = "Destintation main Articles folder specified does not exist";
				txtDestinationArticlesFolder.Focus();
				return false;
			}

			if (rdPreserveDateFoldersHier.Checked && !masterDb.GetItem(txtDestinationArticlesFolder.Text).TemplateID.Guid.Equals(new Guid(Constants.MainArticlesFolderTemplateID)))
			{
				lblError.Text = "Destintation Main Articles folder specified is not a main Articles folder";
				txtDestinationArticlesFolder.Focus();
				return false;
			}

			if (rdMoveToFolderID.Checked && txtToFolderID.Text.Trim() == string.Empty)
			{
				lblError.Text = "Please specify destination Folder ID";
				txtToFolderID.Focus();
				return false;
			}

			if (rdMoveToFolderID.Checked && masterDb.GetItem(txtToFolderID.Text) == null)
			{
				lblError.Text = "Destintation folder specified does not exist";
				txtToFolderID.Focus();
				return false;
			}

			if (rdMoveToFolderID.Checked && !masterDb.GetItem(txtToFolderID.Text).TemplateID.Guid.Equals(new Guid(Constants.MainArticlesFolderTemplateID)) && !masterDb.GetItem(txtToFolderID.Text).TemplateID.Guid.Equals(new Guid(Constants.SubArticlesFolderTemplateID)))
			{
				lblError.Text = "Destintation folder specified is not a main Articles folder or a sub (by date) article folder";
				txtToFolderID.Focus();
				return false;
			}

			if (chkNewTaxonomyFields.Checked && txtNewTaxonomyFields.Text.Trim() == string.Empty)
			{
				lblError.Text = "Please specify new Taxonomy ID(s)";
				txtNewTaxonomyFields.Focus();
				return false;
			}

			if (chkNewTaxonomyFields.Checked)
			{
				foreach (var taxID in txtNewTaxonomyFields.Text.Split('|'))
				{
					if (masterDb.GetItem(taxID)?.TemplateID.Guid.Equals(new Guid(Constants.TaxonomyTemplateID)) == false)
					{
						lblError.Text = "Taxonomy ID (" + taxID + ") specified is not a Taxonomy Item";
						txtNewTaxonomyFields.Focus();
						return false;
					}
				}
			}

			return true;
		}
	}

	public static class Constants
	{
		public static string ArticlesTemplateID = Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticleConstants.TemplateIdString;
		public static string MainArticlesFolderTemplateID = "{24397E3A-81BF-4913-9B45-D2D97BDC93D6}";
		public static string SubArticlesFolderTemplateID = "{D0DFAF98-BB2A-4EB3-A931-13C335201D25}";
		public static string TaxonomyTemplateID = "{6D72D3C0-B5D7-4D08-9BDD-0D9627EF5AD1}";
	}

	public enum SourceType
	{
		ArticleIDs//,
				  //FolderIDs//Not Implemented yet
	}

	public enum DestinationType
	{
		PreserveFolderHierarchy,
		ToSpecificFolderID
	}

	public enum SourceIDType
	{
		ItemID,
		ArticleID
	}

	public struct ArticleMovingParameters
	{
		public SourceType SourceType { get; set; }
		public string SourceIDs { get; set; }
		public SourceIDType SourceIDType { get; set; }

		public DestinationType DestinationType { get; set; }
		public string DestinationMainArticlesFolder { get; set; }
		public string DestinationFolderID { get; set; }
		public bool MoveChildrenWithoutParentFolder { get; set; }

		public bool ClearTaxonomy { get; set; }
		public string NewTaxonomyIDsToAdd { get; set; }
		public bool PublishDestinationItems { get; set; }
	}

	public static class ArticleMoverUtility
	{
		static string _resultStatus;
		private static void log(string message, bool isError = false)
		{
			_resultStatus += "[" + DateTime.Now.ToString("HH:mm:ss") + "] " + (isError ? "***Error: " : string.Empty) + message + "<br />";
		}

		public static bool Process(ArticleMovingParameters parameters, out string resultStatus)
		{
			_resultStatus = string.Empty;
			try
			{
				log("Processing Articles started");
				var masterDb = Sitecore.Data.Database.GetDatabase("master");
				List<Item> items = null;

				bool result = false;
				if (parameters.SourceType == SourceType.ArticleIDs)
				{
					result = MoveByArticleIDs(parameters, out items);
					resultStatus = _resultStatus;
				}

				if (result)
				{
					if (parameters.ClearTaxonomy && items != null && items.Any())
					{
						foreach (var item in items)
						{
							try
							{
								log("Clearing taxonomy from item " + item.ID + " (" + item.Name + ")");
								item.Editing.BeginEdit();
								item.Fields["Taxonomy"].Value = string.Empty;
								item.Editing.EndEdit();
							}
							catch (Exception ex)
							{
								log("Could not clear Taxonomy from item " + item.ID + " (" + item.Name + "). " + ex.ToString(), true);
							}
						}
					}

					if (parameters.NewTaxonomyIDsToAdd != string.Empty && items != null && items.Any())
					{
						foreach (var item in items)
						{
							try
							{
								log("Adding taxonomy (" + parameters.NewTaxonomyIDsToAdd + ") to item " + item.ID + " (" + item.Name + ")");
								item.Editing.BeginEdit();
								if (string.IsNullOrEmpty(item.Fields["Taxonomy"].Value))
									item.Fields["Taxonomy"].Value = parameters.NewTaxonomyIDsToAdd;
								else
									item.Fields["Taxonomy"].Value = "|" + parameters.NewTaxonomyIDsToAdd;
								item.Editing.EndEdit();
							}
							catch (Exception ex)
							{
								log("Could not set Taxonomy to item " + item.ID + " (" + item.Name + ")" + ex.ToString(), true);
							}
						}
					}
				}

				log("Processing Complete");
				resultStatus = _resultStatus;
				return result;
			}
			catch (Exception ex)
			{
				log(ex.ToString(), true);
				resultStatus = _resultStatus;
				return false;
			}
		}

		public static bool MoveByArticleIDs(ArticleMovingParameters parameters, out List<Item> items)
		{
			log("Moving By IDs started");
			var masterDb = Sitecore.Data.Database.GetDatabase("master");

			List<Item> articlesToMove = new List<Item>();
			List<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IArticle> articlesByArticleNumber = new List<IArticle>();
			if (parameters.SourceIDType == SourceIDType.ArticleID)
			{
				articlesByArticleNumber = ArticleMoverUtility.SearchArticlesByArticleNumbers(parameters.SourceIDs.Split('|').Select(s => s.Trim()).ToList());
			}

			foreach (var id in parameters.SourceIDs.Split('|'))
			{
				try
				{
					log("Retrieving items");
					if (parameters.SourceIDType == SourceIDType.ItemID)
					{
						var item = masterDb.GetItem(new ID(new Guid(id)));

						log("Retrieved Item " + id + " (" + item.Name + ")");

						if (!item.TemplateID.Guid.Equals(new Guid(Constants.ArticlesTemplateID)))
						{
							log("**Item " + id + " is not an article");
							continue;
						}

						articlesToMove.Add(item);
					}
					else
					{

						var article = articlesByArticleNumber.FirstOrDefault(w => w.Article_Number == id);
						if (article == null)
						{
							log("**Article ID " + id + ": does not exist");
							continue;
						}

						var item = masterDb.GetItem(new ID(article._Id));

						log("Retrieved Item " + id + " (" + item.Name + ")");

						if (!item.TemplateID.Guid.Equals(new Guid(Constants.ArticlesTemplateID)))
						{
							log("**Item " + id + " is not an article");
							continue;
						}

						articlesToMove.Add(item);
					}
				}
				catch (Exception ex)
				{
					log("failed to retrieve Item " + id + ". " + ex.ToString(), true);
				}
			}

			items = articlesToMove;
			return MoveArticleToDestination(articlesToMove, parameters);
		}

		public static bool MoveArticleToDestination(List<Item> sourceItems, ArticleMovingParameters parameters)
		{
			if (parameters.DestinationType == DestinationType.ToSpecificFolderID)
				return MoveArticlesToSpecificFolderID(sourceItems, parameters);
			else if (parameters.DestinationType == DestinationType.PreserveFolderHierarchy)
				return MoveArticlesAndPreserveHierarchy(sourceItems, parameters);

			return false;
		}

		public static bool MoveArticlesAndPreserveHierarchy(List<Item> sourceItems, ArticleMovingParameters parameters)
		{
			foreach (var item in sourceItems)
			{
				Item folder = null;
				try
				{
					log("Getting destination folder item for " + item.ID + " (" + item.Name + ")");
					folder = GetDestinationFolder_WithHierarchy(item, parameters);
					if (folder == null)
						throw new Exception("folder retrieved empty");
				}
				catch (Exception ex)
				{
					log("Getting destination folder item for " + item.ID + " (" + item.Name + ") failed. " + ex.Message, true);
					continue;
				}

				try
				{
					log("Moving item " + item.ID + " (" + item.Name + ")");
					item.MoveTo(folder);
				}
				catch (Exception ex)
				{
					log("Moving item " + item.ID + " (" + item.Name + ") failed. " + ex.Message, true);
					continue;
				}

				if (parameters.PublishDestinationItems)
				{
					try
					{
						log("Publishing item " + item.ID + " (" + item.Name + ")");
						PublishItem(item, false);
					}
					catch (Exception ex)
					{
						log("Publishing item " + item.ID + " (" + item.Name + ") failed. " + ex.Message, true);
						continue;
					}
				}
			}

			return true;
		}

		public static bool MoveArticlesToSpecificFolderID(List<Item> sourceItems, ArticleMovingParameters parameters)
		{
			var masterDb = Sitecore.Data.Database.GetDatabase("master");

			var folderID = parameters.DestinationFolderID;
			var folder = masterDb.GetItem(new ID(new Guid(folderID)));

			if (folder == null)
			{
				log("Folder " + folderID + " does not exist", true);
				return false;
			}

			if (!folder.TemplateID.Guid.Equals(new Guid(Constants.MainArticlesFolderTemplateID)) && !folder.TemplateID.Guid.Equals(new Guid(Constants.SubArticlesFolderTemplateID)))
			{
				log("Folder " + folderID + " is not an articles folder", true);
				return false;
			}

			foreach (var item in sourceItems)
			{
				try
				{
					log("Moving item " + item.ID + " (" + item.Name + ")");
					item.MoveTo(folder);
				}
				catch (Exception ex)
				{
					log("Moving item " + item.ID + " (" + item.Name + ") failed. " + ex.Message, true);
				}

				if (parameters.PublishDestinationItems)
				{
					try
					{
						log("Publishing item " + item.ID + " (" + item.Name + ")");
						PublishItem(item, false);
					}
					catch (Exception ex)
					{
						log("Publishing item " + item.ID + " (" + item.Name + ") failed. " + ex.Message, true);
					}
				}
			}

			return true;
		}

		private static void PublishItem(Sitecore.Data.Items.Item item, bool deep)
		{
			// The publishOptions determine the source and target database,
			// the publish mode and language, and the publish date
			Sitecore.Publishing.PublishOptions publishOptions =
			  new Sitecore.Publishing.PublishOptions(item.Database,
													 Database.GetDatabase("web"),
													 Sitecore.Publishing.PublishMode.SingleItem,
													 item.Language,
													 System.DateTime.Now);  // Create a publisher with the publishoptions
			Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

			// Choose where to publish from
			publisher.Options.RootItem = item;

			// Publish children as well?
			publisher.Options.Deep = deep;

			// Do the publish!
			publisher.Publish();
		}

		private static Item CreateFolder(string itemName, Item parentItem)
		{
			Item newItem = null;
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				// Get the master database
				Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
				// Get the template to base the new item on
				TemplateID templateID = new TemplateID(master.GetItem(new ID(new Guid(Constants.SubArticlesFolderTemplateID))).ID);

				// Add the item to the site tree
				newItem = parentItem.Add(itemName, templateID);

				// Set the new item in editing mode
				// Fields can only be updated when in editing mode
				// (It's like the begin tarnsaction on a database)
				newItem.Editing.BeginEdit();
				try
				{
					// End editing will write the new values back to the Sitecore
					// database (It's like commit transaction of a database)
					newItem.Editing.EndEdit();
				}
				catch (System.Exception ex)
				{
					// The update failed, write a message to the log
					Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, true);

					// Cancel the edit (not really needed, as Sitecore automatically aborts
					// the transaction on exceptions, but it wont hurt your code)
					newItem.Editing.CancelEdit();
				}
			}

			return newItem;
		}

		private static Item GetDestinationFolder_WithHierarchy(Item sourceitem, ArticleMovingParameters parameters)
		{
			var masterDb = Sitecore.Data.Database.GetDatabase("master");

			Stack<string> folderHierarchy = new Stack<string>();

			var currentItem = sourceitem;
			while (currentItem.Parent != null && !currentItem.Parent.TemplateID.Guid.Equals(new Guid(Constants.MainArticlesFolderTemplateID)))
			{
				folderHierarchy.Push(currentItem.Parent.Name);
				currentItem = currentItem.Parent;
			}

			var destinationMainArticlesFolder = masterDb.GetItem(parameters.DestinationMainArticlesFolder);

			var latestFolderItemFound = destinationMainArticlesFolder;
			while (folderHierarchy.Count > 0)
			{
				string folderName = folderHierarchy.Pop();

				var tempFolder = latestFolderItemFound.Children.FirstOrDefault(w => w.Name == folderName);
				if (tempFolder == null)
				{
					latestFolderItemFound = CreateFolder(folderName, latestFolderItemFound);
				}
				else {
					latestFolderItemFound = tempFolder;
				}
			}

			return latestFolderItemFound;
		}

		public static List<IArticle> SearchArticlesByArticleNumbers(List<string> articleNumbers)
		{
			using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope())
			{
				using (new DatabaseSwitcher(Sitecore.Data.Database.GetDatabase("master")))
				{
					IArticleSearch articleSearch = scope.Resolve<IArticleSearch>();
					IArticleSearchFilter filter = articleSearch.CreateFilter();
					filter.ArticleNumbers = articleNumbers;
					var results = articleSearch.Search(filter);
					var articlesByArticleNumber = results.Articles.ToList();

					return articlesByArticleNumber;
				}
			}
		}
	}
}