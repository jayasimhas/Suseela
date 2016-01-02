using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace WordPluginCustomInstallActions
{
	/// <summary>
	/// Custom installation actions for a Word plugin installation.
	/// </summary>
	/// <remarks>Code shameless borrowed from Misha Shneerson at http://blogs.msdn.com/b/mshneer/archive/2007/09/04/deploying-your-vsto-add-in-to-all-users-part-ii.aspx</remarks>
	public class RegisterOffice2007AddIn
	{
		#region private methods

		private const string userSettingsLocation = @"Software\Microsoft\Office\12.0\User Settings";

		public void IncrementCounter(RegistryKey instructionKey)
		{
			int count = 1;
			object value = instructionKey.GetValue("Count");

			if (value != null)
			{
				if ((int)value != Int32.MaxValue)
					count = (int)value + 1;
			}

			instructionKey.SetValue("Count", count);
		}

		private string GetApplicationPath(string applicationName)
		{

			switch (applicationName.ToLower())
			{
				case "excel":
					return @"Software\Microsoft\Office\Excel\Addins\";
				case "infopath":
					return @"Software\Microsoft\Office\InfoPath\Addins\";
				case "outlook":
					return @"Software\Microsoft\Office\Outlook\Addins\";
				case "powerpoint":
					return @"Software\Microsoft\Office\PowerPoint\Addins\";
				case "word":
					return @"Software\Microsoft\Office\Word\Addins\";
				case "visio":
					return @"Software\Microsoft\Visio\Addins\";
				case "project":
					return @"Software\Microsoft\Office\MS Project\Addins\";
				default:
					throw new Exception(applicationName + " is not a supported application", null);
			}
		}

		# endregion

		public void RegisterAddIn(string addInName)
		{
			RegistryKey userSettingsKey = null;
			RegistryKey instructionKey = null;

			try
			{
				userSettingsKey = Registry.LocalMachine.OpenSubKey(userSettingsLocation, true);

				if (userSettingsKey == null)
				{
					throw new Exception("Internal error: Office User Settings key does not exist", null);
				}

				instructionKey = userSettingsKey.OpenSubKey(addInName, true);

				if (instructionKey == null)
				{
					instructionKey = userSettingsKey.CreateSubKey(addInName);
				}
				else
				{
					// Remove the Delete instruction
					try
					{
						instructionKey.DeleteSubKeyTree("DELETE");
					}
					catch (ArgumentException) { } // Delete instruction did not exist but that is ok.
				}

				IncrementCounter(instructionKey);
			}
			finally
			{
				if (instructionKey != null)
					instructionKey.Close();
				if (userSettingsKey != null)
					userSettingsKey.Close();
			}
		}

		public void UnRegisterAddIn(string applicationName, string addInName)
		{
			RegistryKey userSettingsKey = null;
			RegistryKey instructionKey = null;
			RegistryKey deleteKey = null;

			try
			{
				userSettingsKey = Registry.LocalMachine.OpenSubKey(userSettingsLocation, true);

				if (userSettingsKey == null)
				{
					throw new Exception("Internal error: Office User Settings key does not exist", null);
				}

				instructionKey = userSettingsKey.OpenSubKey(addInName, true);

				if (instructionKey == null)
				{
					instructionKey = userSettingsKey.CreateSubKey(addInName);
				}
				else
				{
					// Make sure there is no Create instruction
					try
					{
						instructionKey.DeleteSubKeyTree("CREATE");
					}
					catch (ArgumentException) { } // Create instruction did not exist but that is ok.
				}

				string instructionString =
								@"DELETE\" +
								GetApplicationPath(applicationName) +
								@"\" +
								addInName;

				deleteKey = instructionKey.CreateSubKey(instructionString);

				IncrementCounter(instructionKey);
			}
			finally
			{
				if (deleteKey != null)
					deleteKey.Close();
				if (instructionKey != null)
					instructionKey.Close();
				if (userSettingsKey != null)
					userSettingsKey.Close();
			}
		}
	}
}
