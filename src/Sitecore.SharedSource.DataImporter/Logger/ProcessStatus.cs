using System;
using System.IO;
using System.Text;
using System.Web.Configuration;

namespace Sitecore.SharedSource.DataImporter.Providers
{
	public enum ProcessStatus
	{
		Info,
		Error,
		Warning,
		ConnectionError,
		NewItemError,
		FieldError,
		DateParseError,
		NotFoundError,
		ImportDefinitionError,
		DuplicateError,
        ReferenceError
	}

	public class ImportRow
	{
		//Should have properties which correspond to the Column Names in the file   
		public virtual string AffectedItem { get; set; }
		public virtual string ErrorMessage { get; set; }
		public virtual string FieldName { get; set; }
		public virtual string FieldValue1 { get; set; }
		public virtual string FieldValue2 { get; set; }
	}

       
    public class XMLDataLogger

    {

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            string logFilePath = (WebConfigurationManager.AppSettings["ArticleLoggingFolder"]);
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            if(strLog == "")
            {
                log.WriteLine(Environment.NewLine);
             }
            log.WriteLine(strLog);
            log.Close();
        }

    }
}