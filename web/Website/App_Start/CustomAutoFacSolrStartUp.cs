using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.CommonServiceLocator;
using AutofacContrib.SolrNet;
using AutofacContrib.SolrNet.Config;
using Microsoft.Practices.ServiceLocation;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SolrProvider;
using Sitecore.ContentSearch.SolrProvider.DocumentSerializers;
using SolrNet;
using SolrNet.Impl;
using RegistrationExtensions = Autofac.RegistrationExtensions;

namespace Informa.Web.App_Start
{
	public class CustomAutoFacSolrStartUp : ISolrStartUp, IProviderStartUp
	{
		private readonly ContainerBuilder builder;
		private readonly SolrServers Cores;
		private IContainer container;

		public CustomAutoFacSolrStartUp(ContainerBuilder builder)
		{
			if (!SolrContentSearchManager.IsEnabled)
				return;
			this.builder = builder;
			this.Cores = new SolrServers();
		}

		private ISolrCoreAdmin BuildCoreAdmin()
		{
			SolrConnection solrConnection = new SolrConnection(SolrContentSearchManager.ServiceAddress);
			int timeout;
			int.TryParse(Settings.GetSetting("SolrConnectionTimeout", "200000"), out timeout);
			solrConnection.Timeout = timeout;
			if (SolrContentSearchManager.EnableHttpCache)
				solrConnection.Cache = ResolutionExtensions.Resolve<ISolrCache>((IComponentContext)this.container) ?? (ISolrCache)new NullCache();
			return (ISolrCoreAdmin)new SolrCoreAdmin((ISolrConnection)solrConnection, ResolutionExtensions.Resolve<ISolrHeaderResponseParser>((IComponentContext)this.container), ResolutionExtensions.Resolve<ISolrStatusResponseParser>((IComponentContext)this.container));
		}

		public void Initialize()
		{
			if (!SolrContentSearchManager.IsEnabled)
				throw new InvalidOperationException("Solr configuration is not enabled. Please check your settings and include files.");
			foreach (string coreId in SolrContentSearchManager.Cores)
				this.AddCore(coreId, typeof(Dictionary<string, object>), string.Format("{0}/{1}", (object)SolrContentSearchManager.ServiceAddress, (object)coreId));

			builder.RegisterModule((IModule)new SolrNetModule(this.Cores));
			RegistrationExtensions.RegisterType<SolrFieldBoostingDictionarySerializer>(this.builder).As<ISolrDocumentSerializer<Dictionary<string, object>>>();
			if (SolrContentSearchManager.EnableHttpCache)
			{
				RegistrationExtensions.RegisterType<HttpRuntimeCache>(this.builder).As<ISolrCache>();
				foreach (SolrServerElement solrServerElement in (ConfigurationElementCollection)this.Cores)
					((IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>)RegistrationExtensions.WithParameters<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(RegistrationExtensions.RegisterType(this.builder, typeof(SolrConnection)).Named(solrServerElement.Id + (object)typeof(SolrConnection), typeof(ISolrConnection)), (IEnumerable<Parameter>)new NamedParameter[1]
		  {
			new NamedParameter("serverURL", (object) solrServerElement.Url)
		  })).OnActivated((Action<IActivatedEventArgs<object>>)(args => ((SolrConnection)args.Instance).Cache = ResolutionExtensions.Resolve<ISolrCache>(args.Context)));
			}
			this.container = this.builder.Build(ContainerBuildOptions.None);
			ServiceLocator.SetLocatorProvider((ServiceLocatorProvider)(() => (IServiceLocator)new AutofacServiceLocator((IComponentContext)this.container)));
			SolrContentSearchManager.SolrAdmin = this.BuildCoreAdmin();
			SolrContentSearchManager.Initialize();
		}

		public void AddCore(string coreId, Type documentType, string coreUrl)
		{
			this.Cores.Add(new SolrServerElement()
			{
				Id = coreId,
				DocumentType = documentType.AssemblyQualifiedName,
				Url = coreUrl
			});
		}

		public bool IsSetupValid()
		{
			if (!SolrContentSearchManager.IsEnabled)
				return false;
			ISolrCoreAdmin admin = this.BuildCoreAdmin();
			return Enumerable.All<CoreResult>(Enumerable.Select<string, CoreResult>(SolrContentSearchManager.Cores, (Func<string, CoreResult>)(defaultIndex => Enumerable.First<CoreResult>((IEnumerable<CoreResult>)admin.Status(defaultIndex)))), (Func<CoreResult, bool>)(status => status.Name != null));
		}
	}
}