﻿using System;

namespace Informa.Library.User.Document
{
	public class SavedDocumentItem : ISavedDocumentItem
	{
		public string DocumentId { get; set; }
		public string Publication { get; set; }
		public string Title { get; set; }
		public DateTime PublishedOn { get; set; }
		public DateTime SavedOn { get; set; }
		public string Url { get; set; }
		public bool IsExternalUrl { get; set; }
	}
}
