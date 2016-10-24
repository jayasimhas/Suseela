This project C#6 features and configured to use Node Tools for Visual Studio, it's recommended for back-end developers to use Visual Studio 2015 Update 1 or greater due to the improvements in that release.  However... the project also references the Roslyn compiler so you should be able to build the environment without these.

The instructions for your local setup should be:

1.	Clone/Extract the project
2.	Extract the Sitecore 81 rev 151003 Website folder into the web/Website folder.  (Skip items that already exist)
    (This is located on the G drive: G:\Software\Sitecore\Installers\8.1\Sitecore 81 rev 151003.zip)
3.	Copy the local.properties example located at /config/env and change the Data-directory, hostname, and connection string items.
    3a.  FRONTEND: Copy the values from the dev environment's connection strings.
    3b.  BACKEND:  Restore the dev databases from \\VWSQL2012\Backups and set your local conneection strings.  (informa_web, informa_master, etc...)
4.	Get the license xml from \\vwfs01\Shares\Software\Sitecore\Developer License\ Put it in the web/Data folder, and re-name it to license.xml.  
5.	Run 'nant.bat init' from the root folder.


Test