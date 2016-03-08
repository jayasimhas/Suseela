using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Elsevier.Library.Reference;

namespace Elsevier.Web.VWB.Util
{

	public class VWBUtil
	{
		public static string GetFormattedDateTime(DateTime dateTime)
		{
			return dateTime.ToString(Constants.VwbDateTimeFormatWithDashes);
		}

	}

}