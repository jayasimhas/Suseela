The instructions for your local setup should be:

1.	Clone/Extract the project
2.	Extract the Sitecore 81 rev 151003 Website folder into the web/Website folder.  (Skip items that already exist)
    (This is located on the G drive: G:\Software\Sitecore\Installers\8.1\Sitecore 81 rev 151003.zip)
3.	Copy the local.properties example and change the Data-directory, hostname, and connection string items.
4.	Add the license.xml to the data directory of your project.
5.	Nant/Build and you should be up and running.  (using the databases from the sitecore installer)
