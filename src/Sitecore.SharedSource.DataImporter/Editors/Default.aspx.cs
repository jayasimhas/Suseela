﻿using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.SharedSource.DataImporter.Providers;
using System.Text;
using System.IO;
using CsvHelper;
using Sitecore.Web;
using Sitecore.SharedSource.DataImporter.Logger;
using Sitecore.SharedSource.DataImporter.PostProcess;

namespace Sitecore.SharedSource.DataImporter.Editors
{ 
    public partial class Default : Page
    {
	    protected Database currentDB;
        protected StringBuilder log;
	    protected Item importItem; 

	    /* Querystring Params available
		    id=%7b59A62A95-9E5D-4478-BDC9-1E793823C48F%7d
		    la=en
		    language=en
		    vs=1
		    version=1
		    database=master
		    readonly=0
		    db=master
	    */

	    protected void Page_Load(object sender, EventArgs e) {

            log = new StringBuilder();

		    string dbName = WebUtil.GetQueryString("db");
		    currentDB = (!string.IsNullOrEmpty(dbName)) 
			    ? Sitecore.Configuration.Factory.GetDatabase(dbName)
			    : Sitecore.Context.ContentDatabase;

		    string idStr = WebUtil.GetQueryString("id");
		    if(Sitecore.Data.ID.IsID(idStr))
			    importItem = currentDB.GetItem(Sitecore.Data.ID.Parse(idStr));

            repJobs.DataSource = Jobs;
            repJobs.DataBind();

            if (!IsPostBack) {
			    foreach (ConnectionStringSettings c in ConfigurationManager.ConnectionStrings) {
				    ddlConnStr.Items.Add(new ListItem(c.Name, c.ConnectionString));
			    }
            } 
        }

        #region Logging

        protected void Log(string message) {
            log.Append(message).AppendLine().AppendLine();
        }

        protected void Log(string errorType, string message) {
            log.AppendFormat("{0} : {1}", errorType, message).AppendLine().AppendLine();
        }

        #endregion Loggging

        #region Import

        protected void btnImport_Click(object sender, EventArgs e) {

            //check import item
            if (importItem == null)
            {
                Log("Error", "Import item is null");
                txtMessage.Text = log.ToString();
                return;
            }

            //check handler assembly
            TextField ha = importItem.Fields["Handler Assembly"];
            if (ha == null || string.IsNullOrEmpty(ha.Value))
            {
                Log("Error", "Import handler assembly is not defined");
                txtMessage.Text = log.ToString();
                return;
            }

            //check handler class
            TextField hc = importItem.Fields["Handler Class"];
            if (hc == null || string.IsNullOrEmpty(hc.Value))
            {
                Log("Error", "Import handler class is not defined");
                txtMessage.Text = log.ToString();
                return;
            }

            //check db
            if (currentDB == null)
            {
                Log("Error", "Database is null");
                txtMessage.Text = log.ToString();
                return;
            }

            //check conn str
            if (string.IsNullOrEmpty(ddlConnStr.SelectedValue))
            {
                Log("Error", "Connection string is empty");
                txtMessage.Text = log.ToString();
                return;
            }

            //try to instantiate object
            IDataMap map = null;
            DefaultLogger l = new DefaultLogger();
            try
            {
                map = (IDataMap)Sitecore.Reflection.ReflectionUtil.CreateObject(
                    ha.Value,
                    hc.Value,
                    new object[] { currentDB, ddlConnStr.SelectedValue, importItem, l }
                );
            }
            catch (FileNotFoundException fnfe)
            {
                Log("Error", string.Format("the binary {0} could not be found", ha.Value));
                txtMessage.Text = log.ToString();
                return;
            }

            //run process
            if (map == null)
            {
                Log("Error", "the data map provided could not be instantiated");
                txtMessage.Text = log.ToString();
                return;
            }
        
            var jobOptions = new Sitecore.Jobs.JobOptions(
                                    "DataImport",
                                    importItem.DisplayName,
                                    Sitecore.Context.Site.Name,
                                    this,
                                    "HandleImport",
                                    new object[] { map, l });

            Sitecore.Jobs.JobManager.Start(jobOptions);

            repJobs.DataSource = Jobs;
            repJobs.DataBind();       
	    }
        protected void HandleImport(IDataMap map, DefaultLogger l) {

            ImportProcessor p = new ImportProcessor(map, l);
            p.Process();
            txtMessage.Text = l.GetLog();
            
            WriteLogs(l);
        }

        #endregion Import

        #region Media Import

        protected void btnMediaImport_Click(object sender, EventArgs e)
		{

			//check import item
			if (importItem == null)
			{
				Log("Error", "Import item is null");
				txtMessage.Text = log.ToString();
				return;
			}

			//check handler assembly
			TextField ha = importItem.Fields["Handler Assembly"];
			if (ha == null || string.IsNullOrEmpty(ha.Value))
			{
				Log("Error", "Import handler assembly is not defined");
				txtMessage.Text = log.ToString();
				return;
			}

			//check handler class
			TextField hc = importItem.Fields["Handler Class"];
			if (hc == null || string.IsNullOrEmpty(hc.Value))
			{
				Log("Error", "Import handler class is not defined");
				txtMessage.Text = log.ToString();
				return;
			}

			//check db
			if (currentDB == null)
			{
				Log("Error", "Database is null");
				txtMessage.Text = log.ToString();
				return;
			}

			//check conn str
			if (string.IsNullOrEmpty(ddlConnStr.SelectedValue))
			{
				Log("Error", "Connection string is empty");
				txtMessage.Text = log.ToString();
				return;
			}

			//try to instantiate object
			IDataMap map = null;
			DefaultLogger l = new DefaultLogger();
			try
			{
				map = (IDataMap)Sitecore.Reflection.ReflectionUtil.CreateObject(
					ha.Value,
					hc.Value,
					new object[] { currentDB, ddlConnStr.SelectedValue, importItem, l }
				);
			}
			catch (FileNotFoundException fnfe)
			{
				Log("Error", string.Format("the binary {0} could not be found", ha.Value));
				txtMessage.Text = log.ToString();
				return;
			}

			//run process
			if (map == null)
			{
				Log("Error", "the data map provided could not be instantiated");
				txtMessage.Text = log.ToString();
				return;
			}

			var jobOptions = new Sitecore.Jobs.JobOptions(
									"MediaLibraryImport",
									"Media",
									Sitecore.Context.Site.Name,
									this,
									"HandleMediaImport",
									new object[] { map, l });

			Sitecore.Jobs.JobManager.Start(jobOptions);

			repJobs.DataSource = Jobs;
			repJobs.DataBind();
		}

	    protected void HandleMediaImport(IDataMap map, DefaultLogger l)
	    {
		    var handler = map as PmbiDataMap;
			handler?.TransferMediaLibrary();
	    }

        #endregion Media Import

        #region Post Import

        protected void btnPostImport_Click(object sender, EventArgs e)
        {
            var jobOptions = new Sitecore.Jobs.JobOptions(
                                    "PostImport",
                                    "Post Import",
                                    Sitecore.Context.Site.Name,
                                    this,
                                    "HandlePostImport",
                                    new object[] { });

            Sitecore.Jobs.JobManager.Start(jobOptions);

            repJobs.DataSource = Jobs;
            repJobs.DataBind();
        }

        protected void HandlePostImport()
        {
            DefaultLogger l = new DefaultLogger();
            InformaPostProcess p = new InformaPostProcess(l);
            p.UpdateArticleReferences();

            WriteLogs(l);
        }

		#endregion Post Import

		#region Publish Items
		protected void btnPublishImported_Click(object sender, EventArgs e)
		{
			try
			{
				//check import item
				if (importItem == null)
				{
					Log("Error", "Import item is null");
					txtMessage.Text = log.ToString();
					return;
				}

				//check handler assembly
				TextField wh = importItem.Fields["Import To Where"];
				if (wh == null || string.IsNullOrEmpty(wh.Value))
				{
					Log("Error", "Import To Where is not defined");
					txtMessage.Text = log.ToString();
					return;
				}

				var jobOptions = new Sitecore.Jobs.JobOptions(
									"PublishImportedItems",
									"Publish Imported Items",
									Sitecore.Context.Site.Name,
									this,
									"HandlePublishItems",
									new object[] { new ID(new Guid(wh.Value)) });

				Sitecore.Jobs.JobManager.Start(jobOptions);

				repJobs.DataSource = Jobs;
				repJobs.DataBind();
			}
			catch (Exception ex)
			{ }
		}

		protected void HandlePublishItems(ID importToWhereID)
		{
			var item = Sitecore.Context.ContentDatabase.GetItem(importToWhereID);
			if (item == null)
			{
				Log("Error", "Import To Where is not valid");
				txtMessage.Text = log.ToString();
				return;
			}

			DefaultLogger l = new DefaultLogger();
			PublishImportedItems p = new PublishImportedItems(l, item);
			p.Publish();

			WriteLogs(l);
		}

		#endregion

		public IEnumerable<Sitecore.Jobs.Job> Jobs
        {
            get
            {
                return Sitecore.Jobs.JobManager.GetJobs().OrderBy(job => job.QueueTime);
            }
        }

        private void WriteLogs(DefaultLogger l)
        {
            foreach (KeyValuePair<string, List<ImportRow>> kvp in l.GetLogRecords()) {
                string logPath = string.Format(@"{0}sitecore modules\Shell\Data Import\logs\{1}.{2}.{3}.csv",
                                    HttpRuntime.AppDomainAppPath, importItem.DisplayName.Replace(" ", "-"),
                                    DateTime.Now.ToString("yyyy.MM.dd.H.mm.ss"),
                                    kvp.Key);
                var file = File.CreateText(logPath);
                var csvFile = new CsvWriter(file);
                csvFile.WriteHeader<ImportRow>();
                foreach (ImportRow ir in kvp.Value)
                    csvFile.WriteRecord(ir);
                file.Close();
            }
        }
    }
}