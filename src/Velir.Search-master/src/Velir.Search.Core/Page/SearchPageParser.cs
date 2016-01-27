using System;
using System.Collections.Generic;
using Glass.Mapper.Sc;
using Jabberwocky.Glass.Models;
using Sitecore.Mvc.Presentation;
using Velir.Search.Core.Rules.Parser;
using Velir.Search.Models;

namespace Velir.Search.Core.Page
{
	public class SearchPageParser : ISearchPageParser
	{
		public SearchPageParser(ISitecoreContext context, ISearchRuleParser ruleParser)
		{
			Initialize(context.GetCurrentItem<IGlassBase>(inferType:true), ruleParser);
		}

		public SearchPageParser(IGlassBase page, ISearchRuleParser ruleParser)
		{
			Initialize(page, ruleParser);
		}

		public SearchPageParser(string pageId, ISitecoreContext context, ISearchRuleParser ruleParser)
		{
			IGlassBase page = null;

			if (!string.IsNullOrEmpty(pageId))
			{
				page = context.GetItem<IGlassBase>(pageId, inferType: true);	
			}
			
			if (page == null)
			{
				page = context.GetCurrentItem<IGlassBase>(inferType: true);
			}

			if (!(page is I_Listing_Configuration) && RenderingContext.CurrentOrNull != null)
			{
				page = context.GetItem<IGlassBase>(RenderingContext.Current.Rendering.DataSource, inferType: true);
			}

			Initialize(page, ruleParser);
		}

		private void Initialize(IGlassBase page, ISearchRuleParser ruleParser)
		{
			RuleParser = ruleParser;

			_listConfig = new Lazy<I_Listing_Configuration>(() => page as I_Listing_Configuration);
			_sortOptions = new Lazy<IEnumerable<ISort>>(() => ListingConfiguration.Available_Sort_Options);

			var refineConfig = page as ISearch_Refinements;
			_refinementOptions = new Lazy<IEnumerable<I_Refinement>>(() =>
			{
				if (refineConfig != null)
				{
					return refineConfig.Refinements;
				}

				return new I_Refinement[0];
			});
		}

		public ISearchRuleParser RuleParser { get; set; }

		private Lazy<I_Listing_Configuration> _listConfig; 
		public I_Listing_Configuration ListingConfiguration
		{
			get { return _listConfig.Value; }
		}

		private Lazy<IEnumerable<ISort>> _sortOptions; 
		public IEnumerable<ISort> SortOptions
		{
			get { return _sortOptions.Value; }
		}

		private Lazy<IEnumerable<I_Refinement>> _refinementOptions; 
		public IEnumerable<I_Refinement> RefinementOptions
		{
			get { return _refinementOptions.Value; }
		}
	}
}
