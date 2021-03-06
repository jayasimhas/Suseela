﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.SharedSource.DataImporter.Providers;

namespace Sitecore.SharedSource.DataImporter.Logger {
    public class DefaultLogger : ILogger {

        public bool LoggedError { get; set; }

        /// <summary>
        /// the log is returned with any messages indicating the status of the import
        /// </summary>
        private StringBuilder log;

        private Dictionary<string, List<ImportRow>> LogRecords = new Dictionary<string, List<ImportRow>>();

        public DefaultLogger(){
            LoggedError = false;
            log = new StringBuilder();
        }
        
        public void Log(string affectedItem, string message, ProcessStatus pResult = ProcessStatus.Info, string fieldName = "", string fieldValue1 = "", string fieldValue2 = "")
        {
            if (pResult.ToString().ToLower().Contains("error"))
                LoggedError = true;

            //log for ui messaging
            log.AppendFormat("{0} : {1}", pResult, message).AppendLine();
            
            //records are for csv file logging
            string fileName = pResult.ToString();
            if (!LogRecords.ContainsKey(fileName))
                LogRecords.Add(fileName, new List<ImportRow>());

            LogRecords[fileName].Add(new ImportRow { AffectedItem = affectedItem, ErrorMessage = message, FieldName = fieldName, FieldValue1 = fieldValue1, FieldValue2 = fieldValue2 });
        }

        public string GetLog(){
            return log.ToString();
        }

        public Dictionary<string, List<ImportRow>> GetLogRecords()
        {
            return LogRecords;
        }

        public void Clear() {
            log.Clear();
        }
    }
}
