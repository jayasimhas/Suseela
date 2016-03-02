using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformaSitecoreWord.Custom_Exceptions
{
	public class InsecureIFrameException : Exception
	{
		public InsecureIFrameException(List<string> insecureIframes)
		{
			InsecureIframes = insecureIframes;
		}

		public List<string> InsecureIframes { get; set; }
	}
}
