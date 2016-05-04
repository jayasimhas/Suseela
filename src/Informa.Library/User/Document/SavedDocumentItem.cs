using System;

namespace Informa.Library.User.Document
{
	public class SavedDocumentItem : ISavedDocumentItem
	{
		public string DocumentId { get; set; }
		public string Publication { get; set; }
		public string Title { get; set; }
		public DateTime Published { get; set; }
		public string Url { get; set; }
	}
}
