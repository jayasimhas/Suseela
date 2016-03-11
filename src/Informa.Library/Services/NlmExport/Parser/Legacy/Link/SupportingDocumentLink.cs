using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
	public class SupportingDocumentLink : AssetLinkBase
	{
		private static readonly Dictionary<string, string> MimeTypes =
			new Dictionary<string, string>()
				{
					{".pdf", "application/pdf"},
					{".xls", "application/vnd.ms-excel"},
					{".doc", "application/msword"},
					{".ppt", "application/vnd.ms-powerpoint"},
					{".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
					{".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
					{".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"}
				};

		public static bool IsSupportedExtension(string link)
		{
			try
			{
				var ext = Path.GetExtension(link);

				if (string.IsNullOrEmpty(ext))
				{
					return false;
				}

				return MimeTypes.ContainsKey(ext.ToLower());
			}
			catch (Exception)
			{
				return false;
			}
		}

		private string getMimeType(string link)
		{
			var ext = Path.GetExtension(link);

			if (!string.IsNullOrEmpty(ext))
			{
				string mimeType;
				if (MimeTypes.TryGetValue(ext.ToLower(), out mimeType))
				{
					return mimeType;
				}
			}

			return string.Empty;
		}

		public override string GetLink(string linkId)
		{
			return linkId;
		}

		public override string GetLinkText(string linkId)
		{
			return linkId;
		}

		public override string LinkType
		{
			get { return "supplementary-material"; }
		}

		public override void Write(StreamWriter writer, string link, string text)
		{
			var nodeLink = GetLink(link);

			// FB#34075: Supplemental fields should have a full path. 
			if (!nodeLink.StartsWith("http://") || !nodeLink.StartsWith("https://"))
			{
				string domain = ConfigurationManager.AppSettings["Redirect.FrontEndHostName"];
				if (!string.IsNullOrEmpty(domain))
				{
					var baseUri = new UriBuilder(domain).Uri;
					var myUri = new Uri(baseUri, nodeLink);
					nodeLink = myUri.AbsoluteUri;
				}
			}

			var startTag = LinkNode.LessThanTemporaryIdentifier + "supplementary-material";
			startTag += " xmlns:xlink=\"http://www.w3.org/1999/xlink\" ";
			startTag += string.Format("xlink:title=\"{0}\" ", text);
			startTag += string.Format("xlink:href=\"{0}\" ", nodeLink);
			startTag += string.Format("mimetype=\"{0}\"{1}", getMimeType(link), LinkNode.GreaterThanTemporaryIdentifier);
			writer.Write(startTag);

			string linkText = string.Empty;

			linkText += LinkNode.LessThanTemporaryIdentifier + "caption" + LinkNode.GreaterThanTemporaryIdentifier;
			linkText += LinkNode.LessThanTemporaryIdentifier + "title" + LinkNode.GreaterThanTemporaryIdentifier;
			linkText += text;
			linkText += LinkNode.LessThanTemporaryIdentifier + "/title" + LinkNode.GreaterThanTemporaryIdentifier;
			linkText += LinkNode.LessThanTemporaryIdentifier + "/caption" + LinkNode.GreaterThanTemporaryIdentifier;

			writer.Write(linkText);

			var endTag = string.Format("{1}/{0}{2}", LinkType, LinkNode.LessThanTemporaryIdentifier,
									   LinkNode.GreaterThanTemporaryIdentifier);
			writer.Write(endTag);
		}

		public override bool UseItalics
		{
			get
			{
				return false;
			}
		}
	}
}
