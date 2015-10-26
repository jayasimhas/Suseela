using System.Linq;
using Sitecore.Buckets.Rules.Bucketing;
using Sitecore.Buckets.Util;
using Sitecore.Diagnostics;
using Sitecore.Rules.Actions;

namespace Informa.Library.CustomSitecore.Rules
{
	/// <remarks>
	/// See: http://www.brimit.com/blog/custom-bucket-structure
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class ResolvePathBasedOnCleanName<T> : RuleAction<T> where T : BucketingRuleContext
	{
		public string Levels { get; set; }

		public override void Apply(T ruleContext)
		{
			Assert.ArgumentNotNull(ruleContext, "ruleContext");

			int nestedFoldersCount;
			if (int.TryParse(Levels, out nestedFoldersCount))
			{
				var foldersNames = ruleContext.NewItemName.ToCharArray().Where(char.IsLetterOrDigit).Take(nestedFoldersCount).ToList();

				if (foldersNames.Any())
				{
					ruleContext.ResolvedPath = string.Join(Constants.ContentPathSeperator, foldersNames);
				}
			}
			else
			{
				Log.Warn("CreateItemNameBasedPath: Cannot resolve item path by this rule", this);
			}
		}
	}
}
