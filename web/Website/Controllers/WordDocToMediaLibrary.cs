using System;
using System.Linq;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data;
using Sitecore.Data.Items;
using System.IO;
using System.Text.RegularExpressions;
using Glass.Mapper.Sc;
using Informa.Library.Utilities;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.System.Media;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Models;
using Sitecore.Resources.Media;

namespace Informa.Web.Controllers
{
	public class WordDocToMediaLibrary
	{
		private static ISitecoreService _sitecoreMasterService;

		public WordDocToMediaLibrary(ISitecoreService sitecoreService)
		{
			_sitecoreMasterService = sitecoreService;
		}

		public MediaItem SaveWordDocIntoMediaLibrary(ArticleItem article, string fileName, string docName, string extension)
		{
			var item = _sitecoreMasterService.GetItem<Item>(article._Id);
			var publicationItem = ArticleExtension.GetAncestorItemBasedOnTemplateID(item);
			Guid publicationGuid = publicationItem.ID.Guid;
			var articleDate = article.Planned_Publish_Date > DateTime.MinValue ? article.Planned_Publish_Date : article.Created_Date;
			var itemFolder = GetMediaFolder(publicationGuid, articleDate);
			var path = itemFolder._Path;
			return CreateMediaLibraryItemFromFile(fileName, docName, extension,path);
		}

		public Item GetMSWordDocumentRootNode()
		{
			IItem_Pointer_Config pointer = _sitecoreMasterService.GetItem<IItem_Pointer_Config>(Constants.MSWordDocumentRootNode);
			if (pointer == null) return null;
			return _sitecoreMasterService.GetItem<Item>(pointer.Item_Pointer);
		}
		public string GetMSWordDocumentRootNodePath()
		{
			var item = GetMSWordDocumentRootNode();
			return item?.Paths.FullPath;
		}

		public IMedia_Folder GetMediaFolder(Guid publicationGuid, DateTime date)
		{
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
                var publication = _sitecoreMasterService.GetItem<IGlassBase>(publicationGuid);
                var vertical = _sitecoreMasterService.GetItem<IGlassBase>((Guid)publication?._Parent._Id);//Vertical Folder//added,21Sep16
                var mediaFolder = _sitecoreMasterService.GetItem<IMedia_Folder>(Constants.MediaLibraryRoot + Constants.MediaLibraryPath);

                string year = date.Year.ToString();
				string month = date.Month.ToString();
				string day = date.Day.ToString();
				IMedia_Folder mediaPublicationFolder;
                IMedia_Folder mediaVerticalFolder;//Vertical Folder//added,21Sep16
                IMedia_Folder yearFolder;
				IMedia_Folder monthFolder;
				IMedia_Folder dayFolder;

                //Vertical Folder (added,21Sep16)
                if (mediaFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == vertical._Name))
                {
                    mediaVerticalFolder =
                        mediaFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == vertical._Name);
                }
                else
                {
                    var newMediaFolder = _sitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(mediaFolder, vertical._Name);
                    _sitecoreMasterService.Save(newMediaFolder);
                    mediaVerticalFolder = newMediaFolder;
                }

                //Publication (modified,21Sep16)
                if (mediaFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == publication._Name))
				{
					mediaPublicationFolder =
                        mediaVerticalFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == publication._Name);
				}
				else
				{
					var newMediaFolder = _sitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(mediaVerticalFolder, publication._Name);
					_sitecoreMasterService.Save(newMediaFolder);
					mediaPublicationFolder = newMediaFolder;
				}

				// Year
				if (mediaPublicationFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == year))
				{
					yearFolder = mediaPublicationFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == year);
				}
				else
				{
					var yearItem = _sitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(mediaPublicationFolder, year);
					_sitecoreMasterService.Save(yearItem);
					yearFolder = yearItem;
				}

				// Month
				if (yearFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == month))
				{
					monthFolder = yearFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == month);
				}
				else
				{
					var monthItem = _sitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(yearFolder, month);
					_sitecoreMasterService.Save(monthItem);
					monthFolder = monthItem;
				}

				// Day
				if (monthFolder._ChildrenWithInferType.OfType<IMedia_Folder>().Any(x => x._Name == day))
				{
					dayFolder = monthFolder._ChildrenWithInferType.OfType<IMedia_Folder>().First(x => x._Name == day);
				}
				else
				{
					var dayItem = _sitecoreMasterService.Create<IMedia_Folder, IMedia_Folder>(monthFolder, day);
					_sitecoreMasterService.Save(dayItem);
					dayFolder = dayItem;
				}
				return dayFolder;
			}
		}

		/// <summary>
		/// Creates an item in the media library from a file. Saves it to the specified path in the media library
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="itemName"></param>
		/// <param name="fileExtension"></param>		
		/// <param name="mediaLibraryPath">Path in sitecore to save the file</param>
		/// <returns></returns>
		public MediaItem CreateMediaLibraryItemFromFile(string filePath, string itemName, string fileExtension, string mediaLibraryPath)
		{
			Sitecore.Security.Accounts.User user = Sitecore.Security.Accounts.User.Current;
			MediaCreatorOptions mediaCreatorOptions = GetDefaultMediaCreatorOptions(Constants.MediaLibraryRoot + mediaLibraryPath, itemName.Replace("-",""));
			//using (new Sitecore.Security.Accounts.UserSwitcher(user))
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				try
				{
					//first, find the item, if it exists already
					string itempath = mediaCreatorOptions.Destination;
					Item mediaItem = _sitecoreMasterService.GetItem<Item>(itempath);
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

		protected MediaCreatorOptions GetDefaultMediaCreatorOptions(string mediaLibraryPath, string itemName)
		{
			//Set up the options for creating the new media library options
			MediaCreatorOptions mediaCreatorOptions;
			Database masterDb = _sitecoreMasterService.Database;
			try
			{
				//Create the full media library item path including the path and the media item name
				//itemName = Sitecore.Data.Items.ItemUtil.ProposeValidItemName(itemName);
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
