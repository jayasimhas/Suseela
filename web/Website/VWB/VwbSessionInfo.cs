using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elsevier.Web.VWB
{
	public class VwbSessionInfo
	{
		public const string IsEntitledIndex = "IsEntitledToVwb";

		public static bool IsEntitled(System.Web.SessionState.HttpSessionState session)
		{
			if (session[IsEntitledIndex] == null) return false;
			return (bool)session[IsEntitledIndex];
		}

		public static void Entitle(System.Web.SessionState.HttpSessionState session, bool entitlement)
		{
			session[IsEntitledIndex] = entitlement;
		}
	}
}