using System;
using System.IO;
using System.Net;
using PluginModels;
using InformaSitecoreWord.Util;

namespace InformaSitecoreWord.Sitecore
{
    public class SitecoreItemGetter
    {
        public class SitecoreMediaItem
        {
            public string Extension { get; set; }
			public string FileName { get; set; }
			public string Url { get; set; }
			public string Title { get; set; }
			public string Uploader { get; set; }
			public DateTime UploadDate { get; set; }
			public string AltText { get; set; }
		}

		protected SitecoreMediaItem GetMediaSitecoreItem(MediaItemStruct mediaItem)
		{
			if (mediaItem == null || mediaItem.Extension.IsNullOrEmpty() || mediaItem.Name.IsNullOrEmpty())
			{
				return null;
			}

			var fileName = _tempFileLocation + mediaItem.Name + "." + mediaItem.Extension;
			if (mediaItem.Data != null)
			{
				byte[] rawItem = mediaItem.Data;


				var ms = new MemoryStream(rawItem);
				var fs = new FileStream(fileName, FileMode.Create);
				ms.WriteTo(fs);

				fs.Close();
				ms.Close();


			}

			var media = new SitecoreMediaItem
			{
				Extension = mediaItem.Extension,
				FileName = fileName,
				Url = mediaItem.Url,
				Title = mediaItem.Name,
				UploadDate = mediaItem.UploadDate,
				Uploader = mediaItem.Uploader,
				AltText =  mediaItem.altText
			};

			return media;
		}

        public SitecoreMediaItem DownloadSiteCoreMediaItem(string path)
        {
			MediaItemStruct mediaItem = null;
            try
            {
                mediaItem = SitecoreClient.GetMediaLibraryItem(path);
            }
			catch(WebException)
			{
				throw;
			}
            catch (Exception e)
            {
				Globals.SitecoreAddin.LogException("Trying to Download Sitecore Media Item", e);
            }

        	return GetMediaSitecoreItem(mediaItem);
        }

		public SitecoreMediaItem GetDocumentInfo(string path)
		{
			MediaItemStruct mediaItem = null;
			try
			{
				mediaItem = SitecoreClient.GetMediaStatistics(path);
			}
			catch (WebException)
			{
				throw;
			}
			catch (Exception e)
			{
				Globals.SitecoreAddin.LogException("Trying to Download Sitecore Media Item", e);
			}

			return GetMediaSitecoreItem(mediaItem);
		}

        public SitecoreItemGetter()
        {
            _tempFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\";
        }

        protected string _tempFileLocation;
    }
}
