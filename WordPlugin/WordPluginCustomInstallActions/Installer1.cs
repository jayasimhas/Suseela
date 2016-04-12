using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace WordPluginCustomInstallActions
{
	[RunInstaller(true)]
	public partial class Installer1 : System.Configuration.Install.Installer
	{
		public Installer1()
		{
			InitializeComponent();

			_registerer = new RegisterOffice2007AddIn();
		}

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
		public override void Install(IDictionary stateSaver)
		{
			_registerer.RegisterAddIn(_addinName);
			base.Install(stateSaver);
		}

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
		public override void Commit(IDictionary savedState)
		{
			base.Commit(savedState);
		}

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
		}

		[System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
		public override void Uninstall(IDictionary savedState)
		{
			_registerer.UnRegisterAddIn("word", _addinName);
			base.Uninstall(savedState);
		}

		protected RegisterOffice2007AddIn _registerer;
		protected readonly string _addinName = "InformaSitecoreWord.WordPlugin";
	}
}
