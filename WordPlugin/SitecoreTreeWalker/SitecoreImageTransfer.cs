using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SitecoreTreeWalker.Sitecore;
using System.IO;

namespace SitecoreTreeWalker
{
	public class SitecoreImageTransfer
	{
		public SitecoreImageTransfer(string path)
		{
			_path = path;
		}

		private string _path { get; set; }
		public string FileName { get; set; }

		public delegate void TransferCompleteDel();
		public event TransferCompleteDel TransferComplete;

		public void ProcessTransfer()
		{
			try
			{
				var data = GetData();
				FileName = SaveFile(data);
				TransferComplete();
			}
			catch(WebException e)
			{
				throw e;
			}
		}

		private byte[] GetData()
		{
			try
			{
				return SitecoreClient.GetMediaLibraryItemData(_path);
			}
			catch(WebException e)
			{
				throw e;
			}
		}

		private string SaveFile(byte[] data)
		{
			string fileName = string.Format(
				@"C:\temp\{0}.{1}",Guid.NewGuid().ToString(),"jpg");

			var stream = new FileStream(fileName,
				FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
			stream.Write(data, 0, data.Length);
			stream.Close();

			return fileName;
		}


	}
}
