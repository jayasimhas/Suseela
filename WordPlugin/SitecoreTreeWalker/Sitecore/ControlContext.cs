using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace SitecoreTreeWalker.Sitecore
{
	public class ControlContext
	{
		private ControlContext() { }

		public Word.Application Application { get; set; }
		public static ControlContext Current { get; private set; }

		public static void Initalize(Word.Application application)
		{
			var context = new ControlContext();
			context.Application = application;
			Current = context;
		}
	}
}
