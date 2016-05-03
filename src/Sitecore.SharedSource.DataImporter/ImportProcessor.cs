﻿using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.SharedSource.DataImporter.Extensions;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.Mappings.Fields;
using Sitecore.SharedSource.DataImporter.Providers;
using Sitecore.SharedSource.DataImporter.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Jobs;

namespace Sitecore.SharedSource.DataImporter {
    public class ImportProcessor {

        protected ILogger Logger { get; set; }

        protected IDataMap DataMap { get; set; }

        public ImportProcessor(IDataMap dm, ILogger l) {
            if (dm == null)
                throw new ArgumentNullException("The provided Data Map was null");
            if(l == null)
                throw new ArgumentNullException("The provided Logger was null");

            DataMap = dm;
            Logger = l;
        }

        /// <summary>
        /// processes each field against the data provided by subclasses
        /// </summary>
        public void Process() {

            Logger.Log("N/A", string.Format("Import Started at: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            if (Sitecore.Context.Job != null)
                Sitecore.Context.Job.Options.Priority = ThreadPriority.Highest;

            IEnumerable<object> importItems;
            try {
                importItems = DataMap.GetImportData();
            } catch (Exception ex) {
                Logger.Log("N/A", string.Format("GetImportData Failed: {0}", ex.Message), ProcessStatus.Error);
                Logger.Log("N/A", string.Format("Import Finished at: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

                if (Sitecore.Context.Job != null)
                    Sitecore.Context.Job.Status.State = JobState.Finished;

                return;
            }

            int totalLines = importItems.Count();
            if (Sitecore.Context.Job != null)
                Sitecore.Context.Job.Status.Total = totalLines;

            long line = 0;

            using (new BulkUpdateContext()) { // try to eliminate some of the extra pipeline work
                foreach (object importRow in importItems) {
                    //import each row of data
                    line++;
                    try {
                        string newItemName = DataMap.BuildNewItemName(importRow);
                        if (string.IsNullOrEmpty(newItemName)) {
                            Logger.Log("N/A", string.Format("BuildNewItemName failed on import row {0} because the new item name was empty", line), ProcessStatus.NewItemError);
                            continue;
                        }

                        Item thisParent = DataMap.GetParentNode(importRow, newItemName);
                        if (thisParent.IsNull())
                        {
                            Logger.Log("N/A", string.Format("Get parent failed on import row {0} because the new item's parent is null", line), ProcessStatus.NewItemError);
                            continue;
                        }

                        DataMap.CreateNewItem(thisParent, importRow, newItemName);
                    } catch (Exception ex) {
                        Logger.Log("N/A", string.Format("Exception thrown on import row {0} : {1}", line, ex.Message), ProcessStatus.NewItemError, "All Import Values", string.Join("||", ((Dictionary<string,string>)importRow).Select(a => $"{a.Key}-{a.Value}")));
                    }

                    if (Sitecore.Context.Job != null)
                    {
                        Sitecore.Context.Job.Status.Processed = line;
                        Sitecore.Context.Job.Status.Messages.Add(string.Format("Processed item {0} of {1}", line, totalLines));
                    }
                }
            }

	        (DataMap as PmbiDataMap)?.SetArticleNumber();

	        //if no messages then you're good
            if (!Logger.LoggedError)
                Logger.Log("Success", "the import completed successfully");

            Logger.Log("N/A", string.Format("Import Finished at: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            if (Sitecore.Context.Job != null)
                Sitecore.Context.Job.Status.State = JobState.Finished;
        }
    }
}
