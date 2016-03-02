using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Schema;

namespace Informa.Library.DCD
{
    public class DCDImportWatcher
    {
        private bool _isValidFile = false;
        private readonly string _destPath = importDirectory + @"\archive\";
        private readonly string _processingPath = importDirectory + @"\processing\";

        private delegate void ProcessFileDelegate(string path);

        private static string importDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["DCD.ImportFolder"];
            }
        }
        /// <summary>
        /// Subscribe to the FileSystemEventHandler and RenamedEventHander
        /// This subscription is executed in the Global.asax file.
        /// </summary>
        public void CreateWatcher()
        {

            DCDLogger.Debug("Attempting to start FileWatcher.");
            //Create a new FileSystemWatcher. 
            FileSystemWatcher watcher = new FileSystemWatcher();

            //Set the filter to only catch xml files.
            watcher.Filter = "*.xml";
            watcher.Changed += new FileSystemEventHandler(Watcher_FileChanged);
            watcher.Path = importDirectory;

            //Enable the FileSystemWatcher events. 
            watcher.EnableRaisingEvents = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            DCDLogger.Info("FileWatcher for DCD Import Started.");			

            //Process any files that are in the "processing" folder
            //This is to re-process anything that was aborted during an app pool recycle

            DCDLogger.Debug("Checking path [" + _processingPath + "]");
            if (Directory.Exists(_processingPath))
            {
                string[] files = Directory.GetFiles(_processingPath, "*.xml");
                DCDLogger.Debug("Found [" + files.Length + "] files with filter *.xml");

                //process each file
                foreach (string file in files)
                {

                    ProcessFileDelegate dg = new ProcessFileDelegate(processFile);
                    DCDLogger.Debug("Processing existing file [" + file +"]");
                    dg.BeginInvoke(file, null, null);
                }
            }

        }

        /// <summary>
        /// Triggered when a file is created.
        /// a) loads and validates the xml
        /// b) copies the file to the archived folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Watcher_FileChanged(object sender, FileSystemEventArgs e)
        {
            string fullPath = e.FullPath;
            string name = e.Name;
            DCDLogger.Debug("FileChanged event raised for [" + name + "] found at [" + fullPath + "]");

            if (!Directory.Exists(_destPath))
            {
                Directory.CreateDirectory(_destPath);
            }

            if (!Directory.Exists(_processingPath))
            {
                Directory.CreateDirectory(_processingPath);
            }

            //If the file does not exist, exit out
            //This event can be fired when a file is deleted, so we need this check
            if (!File.Exists(fullPath))
            {
                return;
            }
            DCDLogger.Info(string.Format("Processing File: " + name));

            // check number one to make sure file is done uploading
            if (Velir.Utilities.FileUtil.IsOpen(fullPath))
            {
                bool bFileOpen = true;
                while (bFileOpen)
                {
                    if (Velir.Utilities.FileUtil.IsOpen(fullPath))
                    {
                        DCDLogger.Info(string.Format("File is open, Waiting 2 seconds and looping..."));
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        bFileOpen = false;
                        DCDLogger.Info(string.Format("File is available... continue processing."));
                    }
                }
            }

            //clean bad characters from the file
            string processingName = "Processing_" + name.Replace(".xml", "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
            string processingFullPath = _processingPath + processingName;
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
                    writer.WriteLine(sanitizeXmlString(line));
                }
            }
            reader.Close();
            writer.Close();

            //Copy the original file to the archive folder
            string processingDestPath = _destPath + processingName;
            File.Copy(fullPath, processingDestPath);
            //Delete the original file
            File.Delete(fullPath);

            processFile(processingFullPath);
        }

        //Process file that is in the processing folder
        private void processFile(string path)
        {
            DCDLogger.Debug("Inner file processing has started for [" + path + "]");
            string name = path.Substring(path.LastIndexOf("\\") + 1);
            DCDImporter importer = new DCDImporter(path, name);
            bool success = importer.ProcessFile();

            string oldArchivePath = _destPath + name;
            string newArchiveName = name.Replace("Processing_", "");
            newArchiveName = (success) ? "Valid_" + newArchiveName : "Invalid_" + newArchiveName;
            string newArchivePath = _destPath + newArchiveName;

            //Delete the processed file file
            File.Delete(path);
            //Rename the old archive file
            File.Move(oldArchivePath, newArchivePath);
        }

        /// <summary>
        /// Remove illegal XML characters from a string.
        /// </summary>
        private string sanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            //convert the encoding from windows-1252 to utf8
            Encoding windows = Encoding.GetEncoding("Windows-1252");
            Encoding utf8 = Encoding.UTF8;

            byte[] isoBytes = windows.GetBytes(xml);
            byte[] utfBytes = Encoding.Convert(windows, utf8, isoBytes);
            string line = utf8.GetString(utfBytes);

            StringBuilder buffer = new StringBuilder(line.Length);

            foreach (char c in line)
            {
                if (isLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private bool isLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }

    }
}
