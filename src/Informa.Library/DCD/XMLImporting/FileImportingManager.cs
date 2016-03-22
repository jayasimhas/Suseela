using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Informa.Library.DCD.XMLImporting
{
    public class FileImportingManager
    {
        public bool EnableWatcher
        {
            get
            {
                if (string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("DCD.EnableWatcher")))
                    return false;

                bool enabled = false;
                bool.TryParse(Sitecore.Configuration.Settings.GetSetting("DCD.EnableWatcher"), out enabled);
                return enabled;
            }
        }

        public string ImportDirectory
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("DCD.ImportFolder") ?? string.Empty;
            }
        }

        public string ProcessingDirectory
        {
            get
            {
                return ImportDirectory + @"\processing\";
            }
        }

        public string DestinationDirectory
        {
            get
            {
                return ImportDirectory + @"\archive\";
            }
        }

        public FileImportingManager()
        { }

        public void StartIfStartable()
        {
            try
            {
                if (EnableWatcher)
                {
                    //This is to make sure, the DCD DB has started and the sitecore context is ready.
                    Thread t = new Thread(() => { Thread.Sleep(30000); CreateWatcher(); });
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                DCDImportLogger.Error("Error Starting Watcher", ex);
            }
        }

        private void CreateWatcher()
        {
            DCDImportLogger.Debug("Attempting to start FileWatcher.");
            //Create a new FileSystemWatcher. 
            FileSystemWatcher watcher = new FileSystemWatcher();

            //If import directory is invalid do not continue
            if (Directory.Exists(ImportDirectory) == false)
                return;

            //Set the filter to only catch xml files.
            watcher.Filter = "*.xml";
            watcher.Changed += Watcher_FileChanged;
            watcher.Path = ImportDirectory;

            //Enable the FileSystemWatcher events. 
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            DCDImportLogger.Info("FileWatcher for DCD Import Started.");

            //Process any files that are in the "processing" folder
            //This is to re-process anything that was aborted during an app pool recycle
            DCDImportLogger.Debug("Checking path [" + ProcessingDirectory + "]");
            if (Directory.Exists(ProcessingDirectory))
            {
                string[] files = Directory.GetFiles(ProcessingDirectory, "*.xml");
                DCDImportLogger.Debug("Found [" + files.Length + "] files with filter *.xml");

                //process each file
                foreach (string file in files)
                {
                    Action<string> processAction = path => processFile(path);

                    DCDImportLogger.Debug("Processing existing file [" + file + "]");
                    processAction.BeginInvoke(file, null, null);
                }
            }
        }

        private void Watcher_FileChanged(object sender, FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            string name = e.Name;
            DCDImportLogger.Debug("FileChanged event raised for [" + name + "] found at [" + fullPath + "]");

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            if (!Directory.Exists(ProcessingDirectory))
            {
                Directory.CreateDirectory(ProcessingDirectory);
            }

            //If the file does not exist, exit out
            //This event can be fired when a file is deleted, so we need this check
            if (!File.Exists(fullPath))
            {
                return;
            }
            DCDImportLogger.Info(string.Format("Processing File: " + name));

            // check number one to make sure file is done uploading
            if (XMLFileUtilities.IsOpen(fullPath))
            {
                bool bFileOpen = true;
                while (bFileOpen)
                {
                    if (XMLFileUtilities.IsOpen(fullPath))
                    {
                        DCDImportLogger.Info(string.Format("File is open, Waiting 2 seconds and looping..."));
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        bFileOpen = false;
                        DCDImportLogger.Info(string.Format("File is available... continue processing."));
                    }
                }
            }

            //clean bad characters from the file
            string processingName = "Processing_" + name.Replace(".xml", "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
            string processingFullPath = ProcessingDirectory + processingName;
            StreamReader reader = new StreamReader(fullPath, Encoding.GetEncoding("Windows-1252"));
            StreamWriter writer = new StreamWriter(processingFullPath, false, Encoding.UTF8);
            bool first = true;
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                if (first && line.Contains("<?xml version"))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    first = false;
                }
                else
                {
                    writer.WriteLine(XMLFileUtilities.SanitizeXmlString(line));
                }
            }
            reader.Close();
            writer.Close();

            //Copy the original file to the archive folder
            string processingDestPath = DestinationDirectory + processingName;
            File.Copy(fullPath, processingDestPath);
            //Delete the original file
            File.Delete(fullPath);

            processFile(processingFullPath);
        }

        private void processFile(string path)
        {
            DCDImportLogger.Debug("Inner file processing has started for [" + path + "]");
            string name = path.Substring(path.LastIndexOf("\\") + 1);
            DCDImporter importer = new DCDImporter(path, name);
            bool success = false;
            success = importer.ProcessFile();

            string oldArchivePath = DestinationDirectory + name;
            string newArchiveName = name.Replace("Processing_", "");
            newArchiveName = (success) ? "Valid_" + newArchiveName : "Invalid_" + newArchiveName;
            string newArchivePath = DestinationDirectory + newArchiveName;

            //Delete the processed file file
            File.Delete(path);
            //Rename the old archive file
            File.Move(oldArchivePath, newArchivePath);
        }
    }
}
