using System;
using System.Configuration;
using Microsoft.Win32;

namespace SitecoreTreeWalker.Config
{
	public static class ApplicationConfig
	{
		public static Configuration Config;

		public static string ConfigPath { get; private set; }
		public static string RegistryHive { get; private set; }

		public static string GetPropertyValue(string propName)
		{
			if (Config == null)
			{
				/*try
				{
					Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: Getting configuration object from registry...");
					Config = GetConfigFromRegistry();
					Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: Config file path = [" + Config.FilePath + "].");
					ConfigPath = Config.FilePath;
				}
				catch (ConfigurationErrorsException cex)
				{
					Globals.SitecoreAddin.LogException
						("ApplicationConfig.GetPropertyValue: Cannot load configuration from registry!", cex);
				}
				catch (NullReferenceException nex)
				{
					Globals.SitecoreAddin.LogException
						("ApplicationConfig.GetPropertyValue: Cannot load configuration from registry!", nex);
				}*/
				if (Config == null || Config.AppSettings.Settings[propName] == null)
				{
					try
					{
						Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: Getting configuration object from .NET...");
						Config = GetConfig();
						Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: Config file path = [" + Config.FilePath + "].");
						ConfigPath = Config.FilePath;
					}
					catch (ConfigurationErrorsException cex)
					{
						Globals.SitecoreAddin.LogException("ApplicationConfig.GetPropertyValue: Cannot load configuration!", cex);
						throw new ApplicationException("Cannot load configuration!", cex);
					}

					if (Config.AppSettings.Settings[propName] == null)
					{
						Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: Could not find property [" + propName + "]");
						throw new ApplicationException("Could not find property [" + propName + "]");
					}
				}
				Globals.SitecoreAddin.Log
					("ApplicationConfig.GetPropertyValue: Successfully retrieved configuration object and returning property value...");
			}
			return GetPropertyValue(propName, Config);
		}

		public static string GetPropertyValue(string propName, Configuration config)
		{
			return config.AppSettings.Settings[propName].Value;
		}

		public static int GetTitleMaxCharacters()
		{
			return Int32.Parse(GetPropertyValue(Constants.TitleMaxCharacters));
		}

		private static Configuration GetConfig()
		{
			return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoaming);
		}

		/*private static Configuration GetConfigFromRegistry()
		{
			string exePath;

			try
			{
				RegistryKey currentKey = Registry.CurrentUser.OpenSubKey("Software");
				currentKey = currentKey.OpenSubKey("Microsoft");
				currentKey = currentKey.OpenSubKey("Office");
				currentKey = currentKey.OpenSubKey("Word");
				currentKey = currentKey.OpenSubKey("AddIns");
				currentKey = currentKey.OpenSubKey("Elsevier.WordPlugin");
				exePath = (string)currentKey.GetValue("Manifest");
				exePath = exePath.Replace("file:///", "");
				exePath = exePath.Replace("/", @"\");
				exePath = exePath.Replace(".vsto|vstolocal", ".dll");
				Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: ");
				RegistryHive = "CurrentUser";
			}
			catch (NullReferenceException nex)
			{
				RegistryKey currentKey = Registry.LocalMachine.OpenSubKey("Software");
				currentKey = currentKey.OpenSubKey("Microsoft");
				currentKey = currentKey.OpenSubKey("Office");
				currentKey = currentKey.OpenSubKey("Word");
				currentKey = currentKey.OpenSubKey("AddIns");
				currentKey = currentKey.OpenSubKey("Elsevier.WordPlugin");
				exePath = (string)currentKey.GetValue("Manifest");
				exePath = exePath.Replace("file:///", "");
				exePath = exePath.Replace("/", @"\");
				exePath = exePath.Replace(".vsto|vstolocal", ".dll");
				Globals.SitecoreAddin.Log("ApplicationConfig.GetPropertyValue: ");
				RegistryHive = "LocalMachine";
			}

			return ConfigurationManager.OpenExeConfiguration(exePath);
		}*/
	}
}
