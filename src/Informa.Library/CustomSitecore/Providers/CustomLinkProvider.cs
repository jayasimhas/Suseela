using System;
using Sitecore;
using Sitecore.Buckets.Extensions;
using Sitecore.Buckets.Managers;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Links;

namespace Informa.Library.CustomSitecore.Providers
{
	/// <remarks>
	/// See: https://adeneys.wordpress.com/2013/07/19/item-buckets-and-urls/
	/// </remarks>
	public class CustomLinkProvider : LinkProvider
	{
		private const string AspxExtension = ".aspx";

		public override string GetItemUrl(Item item, UrlOptions options)
		{
			if (BucketManager.IsItemContainedWithinBucket(item))
			{
				var bucketItem = item.GetParentBucketItemOrParent();
				var itemName = item.Name;
				if (bucketItem != null && bucketItem.IsABucket())
				{
					var bucketUrl = base.GetItemUrl(bucketItem, options);
					if (options.AddAspxExtension && bucketUrl.EndsWith(AspxExtension, StringComparison.InvariantCultureIgnoreCase))
					{
						var index = bucketUrl.LastIndexOf(AspxExtension, StringComparison.InvariantCultureIgnoreCase);
						bucketUrl = bucketUrl.Substring(0, index);
					}
					if (options.UseDisplayName)
					{
						itemName = item.DisplayName;
					}
					if (options.EncodeNames)
					{
						itemName = MainUtil.EncodePath(itemName, '/');
					}
					if (options.LowercaseUrls)
					{
						itemName = itemName.ToLowerInvariant();
					}

					return FileUtil.MakePath(bucketUrl, itemName) + (options.AddAspxExtension ? AspxExtension : string.Empty);
				}
			}

			return base.GetItemUrl(item, options);
		}
	}
}