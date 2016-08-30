using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Jabberwocky.Glass.Autofac.Util;
using Sitecore.Data.Items;
using System;
using Autofac;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Services.Global;
using System.Web.Mvc;

namespace Informa.Library.Publication
{
	[AutowireService(LifetimeScope.Default)]
	public class SitePublicationWorkflow : ISitePublicationWorkflow
	{
		private readonly ISitecoreService _service;
		private readonly ICacheProvider _cacheProvider;
		private readonly IGlobalSitecoreService _globalSitecoreService;

		public SitePublicationWorkflow(Func<string, ISitecoreService> serviceFactory, ICacheProvider cacheProvider, IGlobalSitecoreService globalSitecoreService)
		{
			_service = serviceFactory("master");
			_cacheProvider = cacheProvider;
			_globalSitecoreService = globalSitecoreService;
		}

		public IWorkflow GetPublicationWorkflow(Item item)
		{
			string cacheKey = $"{getSiteRoot(item).Publication_Code}-PublicationWorkflow";

			IWorkflow workflowFromCache = _cacheProvider.GetFromCache<IWorkflow>(cacheKey);
			if (workflowFromCache == null)
			{
				var workflow = _service.GetItem<IWorkflow>(getSiteRoot(item).Workflow);
				_cacheProvider.AddToCache(cacheKey, workflow);
				return workflow;
			}

			return workflowFromCache;
		}

		public IState GetEditAfterPublishState(Item item)
		{
			string cacheKey = $"{getSiteRoot(item).Publication_Code}-EditAfterPublishWorkflowState";

			IState stateFromCache = _cacheProvider.GetFromCache<IState>(cacheKey);
			if (stateFromCache == null)
			{
				var state = getEditAfterPublishState(item);
				_cacheProvider.AddToCache(cacheKey, state);
				return state;
			}

			return stateFromCache;
		}

		public IState GetFinalState(Item item)
		{
			string cacheKey = $"{getSiteRoot(item).Publication_Code}-FinalWorkflowState";

			IState stateFromCache = _cacheProvider.GetFromCache<IState>(cacheKey);
			if (stateFromCache == null)
			{
				var state = getFinalState(item);
				_cacheProvider.AddToCache(cacheKey, state);
				return state;
			}

			return stateFromCache;
		}

		public IState GetInitialState(Item item)
		{
			string cacheKey = $"{getSiteRoot(item).Publication_Code}-InitialWorkflowState";

			IState stateFromCache = _cacheProvider.GetFromCache<IState>(cacheKey);
			if (stateFromCache == null)
			{
				var state = getInitialState(item);
				_cacheProvider.AddToCache(cacheKey, state);
				return state;
			}

			return stateFromCache;
		}

		private ISite_Root getSiteRoot(Item item)
		{
			return _globalSitecoreService.GetSiteRootAncestor(item.ID.ToGuid());
		}

		private IState getEditAfterPublishState(Item item)
		{
			var workflowItem = _service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.IWorkflow>(getSiteRoot(item).Workflow);

			var states = _service.Database.WorkflowProvider.GetWorkflow(getSiteRoot(item).Workflow.ToString()).GetStates();

			IState editAfterPublish = null;
			IState finalState = null;
			foreach (var state in states)
			{
				var stateID = state.StateID;

				var istate = _service.GetItem<IState>(new Guid(stateID));
				if (istate.Is_Edit_After_Publish && editAfterPublish == null)
				{
					editAfterPublish = istate;
				}
				else if (istate.Final && finalState == null)
				{
					finalState = istate;
				}
			}

			return editAfterPublish == null || editAfterPublish == null ? finalState : editAfterPublish;
		}

		private IState getFinalState(Item item)
		{
			var workflowItem = _service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.IWorkflow>(getSiteRoot(item).Workflow);

			var states = _service.Database.WorkflowProvider.GetWorkflow(getSiteRoot(item).Workflow.ToString()).GetStates();

			IState finalState = null;
			foreach (var state in states)
			{
				var stateID = state.StateID;

				var istate = _service.GetItem<IState>(new Guid(stateID));
				if (istate.Final)
				{
					return istate;
				}
			}

			return finalState;
		}

		private IState getInitialState(Item item)
		{
			var workflowItem = _service.GetItem<Informa.Models.Informa.Models.sitecore.templates.System.Workflow.IWorkflow>(getSiteRoot(item).Workflow);

			return _service.GetItem<IState>(workflowItem.Initial_State);
		}
	}
}
