using System.Collections.Generic;
using Sitecore.Diagnostics;

namespace Velir.Search.Core.Rules
{
	public class ErrorReport
	{
		public List<ErrorReport.Message> Errors { get; private set; }

		public List<ErrorReport.Message> Warnings { get; private set; }

		public bool HasErrors
		{
			get { return this.Errors.Count > 0; }
		}

		public bool HasWarnings
		{
			get { return this.Warnings.Count > 0; }
		}

		public ErrorReport()
		{
			this.Errors = new List<ErrorReport.Message>();
			this.Warnings = new List<ErrorReport.Message>();
		}

		public void AddError(string p_Message)
		{
			this.Errors.Add(new ErrorReport.Message(p_Message));
		}

		public void AddWarning(string p_Message)
		{
			this.Warnings.Add(new ErrorReport.Message(p_Message));
		}

		public void AppendAsGroup(string p_GroupTitle, ErrorReport p_SubReport)
		{
			Assert.ArgumentNotNullOrEmpty(p_GroupTitle, "p_GroupTitle");
			Assert.ArgumentNotNull((object) p_SubReport, "p_SubReport");
			if (p_SubReport.HasErrors)
				this.Errors.Add(
					(ErrorReport.Message)
						new ErrorReport.MessageGroup(p_GroupTitle, (IEnumerable<ErrorReport.Message>) p_SubReport.Errors));
			if (!p_SubReport.HasWarnings)
				return;
			this.Warnings.Add(
				(ErrorReport.Message)
					new ErrorReport.MessageGroup(p_GroupTitle, (IEnumerable<ErrorReport.Message>) p_SubReport.Warnings));
		}

		public class Message
		{
			public string Text { get; private set; }

			public Message(string p_Text)
			{
				Assert.ArgumentNotNullOrEmpty(p_Text, "p_Text");
				this.Text = p_Text;
			}
		}

		public class MessageGroup : ErrorReport.Message
		{
			public List<ErrorReport.Message> GroupMessages { get; private set; }

			public MessageGroup(string p_GroupTitle, IEnumerable<ErrorReport.Message> p_GroupMessages)
				: base(p_GroupTitle)
			{
				this.GroupMessages = new List<ErrorReport.Message>(p_GroupMessages);
			}
		}
	}
}
