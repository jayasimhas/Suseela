using System;
using System.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.IO;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.System.Media;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Models;
using Sitecore.Resources.Media;

namespace Informa.Web.Controllers
{
	public class WordDocToMediaLibrary
	{
		public const string MasterDb = "master";
		protected static string MediaLibraryRoot;
		protected static string MediaLibraryPath;
		private static ISitecoreService SitecoreMasterService;

		public WordDocToMediaLibrary(Func<string, ISitecoreService> sitecoreFactory)
		{
			SitecoreMasterService = sitecoreFactory(MasterDb);
			MediaLibraryRoot = "/sitecore/media library/";
			MediaLibraryPath = "Documents/";
		}

		public static MediaItem SaveWordDocIntoMediaLibrary(IArticle article, string fileName, string docName, string extension)
		{
			Guid publicationGuid = article.Publication;
			var articleDate = article.Planned_Publish_Date > DateTime.MinValue ? article.Planned_Publish_Date : article.Created_Date;
			var itemFolder = GetMediaFolder(publicationGuid, articleDate);
			var path = itemFolder._Path;
			return CreateMediaLibraryItemFromFile(fileName, docName, extension,path);
		}

		public static Item GetMSWordDocumentRootNode()
		{
			IItem_Pointer_Config pointer = SitecoreMasterService.GetItem<IItem_Pointer_Config>("{FDBFCAC8-03CA-4B0B-BEFE-2171050E19C6}");
			if (pointer == null) return null;
			return SitecoreMasterService.GetItem<Item>(pointer.Item_Pointer);
		}
		public static string GetMSWordDocumentRootNodePath()
		{
			var item = GetMSWordDocumentRootNode();
			return item?.Paths.FullPath;
		}

		public static IMedia_Folder GetMediaFolder(Guid publicationGuid, DateTime date)
		{
			var mediaFolder = SitecoreMasterService.GetItem<IMedia_Folder>(MediaLibraryRoot + MediaLibraryPath);
			var publication = SitecoreMasterService.GetItem<IGlassBase>(publicationGuid);
			string year = date.Year.ToString();
			string month = date.Month.ToString();
			string day = date.Day.ToString();
			IMedia_Folder mediaPublicationFolder;
			IMedia_Folder yearFolder;
			IMedia_Folder monthFolder;
			IMedia_Folder dayFolder;
			if (mediaFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == publication._Name))
			{
				mediaPublicationFolder = mediaFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == publication._Name);
			}
			else
			{
				var newMediaFolder = SitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(mediaFolder, publication._Name);
				SitecoreMasterService.Save(newMediaFolder);
				mediaPublicationFolder = newMediaFolder;
			}

			// Year
			if (mediaPublicationFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == year))
			{
				yearFolder = mediaPublicationFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == year);
			}
			else
			{
				var yearItem = SitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(mediaPublicationFolder, year);
				SitecoreMasterService.Save(yearItem);
				yearFolder = yearItem;
			}

			// Month
			if (yearFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == month))
			{
				monthFolder = yearFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == month);
			}
			else
			{
				var monthItem = SitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(yearFolder, month);
				SitecoreMasterService.Save(monthItem);
				monthFolder = monthItem;
			}

			// Day
			if (monthFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == day))
			{
				dayFolder = monthFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == day);
			}
			else
			{
				var dayItem = SitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(monthFolder, day);
				SitecoreMasterService.Save(dayItem);
				dayFolder = dayItem;
			}

			return dayFolder;
		}


		/// <summary>
		/// Creates an item in the media library from a file. Saves it to the specified path in the media library
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="itemName"></param>
		/// <param name="fileExtension"></param>		
		/// <param name="mediaLibraryPath">Path in sitecore to save the file</param>
		/// <returns></returns>
		public static MediaItem CreateMediaLibraryItemFromFile(string filePath, string itemName, string fileExtension, string mediaLibraryPath)
		{
			Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.Current;
			MediaCreatorOptions mediaCreatorOptions = GetDefaultMediaCreatorOptions(MediaLibraryRoot + mediaLibraryPath, itemName);
			using (new Sitecore.Security.Accounts.UserSwitcher(user))
			{
				try
				{
					//first, find the item, if it exists already
					string itempath = mediaCreatorOptions.Destination;
					Item mediaItem = SitecoreMasterService.GetItem<Item>(itempath);
					if (mediaItem == null)
					{
						//new media item
						MediaItem media = MediaManager.Creator.CreateFromFile(filePath, mediaCreatorOptions);
						return media;
					}
					else
					{
						//existing media item, update and increment version.
						MediaItem media = mediaItem;
						MediaItem newMedia;

						Stream stream = new FileStream(filePath, FileMode.Open);
						using (new EditContext(media))
						{
							newMedia = media.InnerItem.Versions.AddVersion();

							Media data = MediaManager.GetMedia(newMedia);
							var mediaStream = new MediaStream(stream, fileExtension, mediaItem);
							data.SetStream(mediaStream);
						}

						return newMedia;

					}
				}
				catch (Exception e)
				{
					throw new Exception("CreateMediaLibraryItemFromFile: Cannot create Media Option from file = " + filePath, e);
				}
			}
		}

		protected static MediaCreatorOptions GetDefaultMediaCreatorOptions(string mediaLibraryPath, string itemName)
		{
			//Set up the options for creating the new media library options
			MediaCreatorOptions mediaCreatorOptions;
			Database masterDb = SitecoreMasterService.Database;
			// SitecoreDatabases.AuthoringDatabase;
			try
			{
				//Create the full media library item path including the path and the media item name
				//TODO: Validate media item name
				//itemName = ItemNameUtil.GetValidItemName(itemName);
				itemName = Regex.Replace(itemName, @"<(.|\n)*?>", string.Empty).Trim();
				string fullMediaPath = mediaLibraryPath + @"/" + itemName;
				mediaCreatorOptions = new MediaCreatorOptions
				{
					Destination = fullMediaPath,
					Database = masterDb,
					Versioned = true
				};
			}
			catch (Exception exc)
			{
				throw new Exception($"Error creating media creator options when trying to create publication media library item: {exc.Message}");
			}

			return mediaCreatorOptions;
		}


	}
}
