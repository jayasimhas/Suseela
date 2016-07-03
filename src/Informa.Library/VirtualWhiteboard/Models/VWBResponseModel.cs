using System;

namespace Informa.Library.VirtualWhiteboard.Models
{
    public class VwbResponseModel
    {
        public bool IsSuccess { get; set; }
        public string FriendlyErrorMessage { get; set; }
        public string DebugErrorMessage { get; set; }
		public Guid IssueId { get; set; }
    }
}