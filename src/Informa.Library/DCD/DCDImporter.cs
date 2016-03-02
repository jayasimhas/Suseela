using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Sitecore.Events;
using Informa.Models.DealsCompaniesDrugs;
using Informa.Library.Utilities.References;
using Informa.Library.DCD.XML;

namespace Informa.Library.DCD
{
	public class DCDImporter
	{
		//These are used to keep track of all updates and deletes, so coveo/lucene can be updated after the import
		private readonly List<IDCD> _dealUpdates;
		private readonly List<IDCD> _companyUpdates;
		private readonly List<IDCD> _drugUpdates;
		private readonly List<string> _dealDeletes;
		private readonly List<string> _companyDeletes;
		private readonly List<string> _drugDeletes;

		public enum ImportResult
		{
			InProgress,
			Success,
			PartialSuccess,
			Failure
		}

		public enum RecordImportResult
		{
			Success,
			Skipped,
			Failure
		}

		public enum RecordImportOperation
		{
			Update,
			Insert,
			Delete
		}

		private bool documentIsValid = true;
		private string documentPath;
		private string fileName;
		private DCDContext dcd;
		private DealsCompaniesDrugsConfigurationItem dcdConfig;

		//Logging information
		private ImportLog importLog;
		private StringBuilder importLogNotes;
		private bool hasSomeFailures = false;

		public DCDImporter(string documentPath, string fileName)
		{
			this.documentPath = documentPath;
			this.fileName = fileName;
			dcd = new DCDContext();
			importLogNotes = new StringBuilder();
            dcdConfig = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(ItemReferences.Instance.DCDConfigurationItem));// SitecoreDatabases.Live.GetItem(ItemReference.DealsCompaniesDrugsConfiguration.Id);
			_dealUpdates = new List<IDCD>();
			_dealDeletes = new List<string>();
			_companyUpdates = new List<IDCD>();
			_companyDeletes = new List<string>();
			_drugUpdates = new List<IDCD>();
			_drugDeletes = new List<string>();
		}

		public bool ProcessFile()
		{
			try
			{
				//Initialize the database log entry for this import
				//importLog = dcd.CreateImportLogEntry(DateTime.Now, fileName);

				//if (importLog == null)
				//{
					//return false;
				//}
				//Set the DTD validation options
				XmlReaderSettings settings = new XmlReaderSettings();
				settings.ProhibitDtd = false;
				settings.ValidationType = ValidationType.DTD;
				settings.ValidationEventHandler += new ValidationEventHandler(XMLValidationHandler);
				settings.CheckCharacters = false;
				XmlReader reader = XmlReader.Create(documentPath, settings);
				IBIContent content;
				try
				{
					//Deserialize the xml file
					XmlSerializer serializer = new XmlSerializer(typeof(IBIContent));
					content = (IBIContent)serializer.Deserialize(reader);
				}
				catch (Exception e)
				{
					reader.Close();
					throwCriticalImportError("DCD XML doc de-serialization failed! ", e);
					return false;
				}
				reader.Close();

				//If there was an error validating the XML file with the appropriate DTD file
				if (!documentIsValid)
				{
					throwCriticalImportError("DCD XML doc validation failed! " + importLogNotes);
					return false;
				}

				//Our XML file is good. Quickly validate that the number of records is correct.
				int numRecords;
				if (!int.TryParse(content.Feed.NumberOfRecords, out numRecords))
				{
					throwCriticalImportError("Error processing DCD XML doc. Number of records value is not a valid integer.");
					return false;
				}
				if (numRecords != content.RecordSet.Count)
				{
					throwCriticalImportError("Error processing DCD XML doc. Number of records value does not equal the record set size.");
					return false;
				}
				//We're past the critical error point. Time to process each record
				foreach (Record record in content.RecordSet)
				{
					bool success = false;
					if (content.type == IBIContentType.deal)
					{
						success = dcd.ProcessRecord<Deal, DealRecordImportLog>(record, importLog);
						if (success)
						{
							if (record.Command == CommandType.Delete)
							{
								_dealDeletes.Add(record.Identification.RecordNumber);
							}
							else
							{
								_dealUpdates.Add(dcd.GetRecord<Deal>(record.Identification.RecordId));
							}
						}
					}
					else if (content.type == IBIContentType.company)
					{
						success = dcd.ProcessRecord<Company, CompanyRecordImportLog>(record, importLog);
						if (success)
						{
							//TODO: This is a hack for the thin records. Replace this when full records are imported
							if (!string.IsNullOrEmpty(record.Content.InnerXml))
							{
								XDocument contentDoc = XDocument.Parse(record.Content.InnerXml);
								List<CompanyPath> paths = new List<CompanyPath>();
								foreach (XElement node in contentDoc.Descendants("CompanyPath"))
								{
									CompanyPath path = new CompanyPath();
									XAttribute id = node.Attribute("id");
									if (id != null)
									{
										path.id = id.Value;
									}
									path.Value = node.Value;
									paths.Add(path);
								}
								success = dcd.AddRelatedCompanies(record.Identification.RecordId, paths);
							}

							if (record.Command == CommandType.Delete)
							{
								_companyDeletes.Add(record.Identification.RecordNumber);
							}
							else
							{
								_companyUpdates.Add(dcd.GetRecord<Company>(record.Identification.RecordId));
							}
						}
					}
					else if (content.type == IBIContentType.drug)
					{
						success = dcd.ProcessRecord<Drug, DrugRecordImportLog>(record, importLog);
						if (success)
						{
							if (record.Command == CommandType.Delete)
							{
								_drugDeletes.Add(record.Identification.RecordNumber);
							}
							else
							{
								_drugUpdates.Add(dcd.GetRecord<Drug>(record.Identification.RecordId));
							}
						}
					}

					if (!success)
					{
						hasSomeFailures = true;
					}
				}

				//Update the indexes
				ApplicationConfig config = ApplicationConfig.Get();
				if (config.DCDUpdateIndexesOnImport)
				{
					//Only update coveo on an "Update" type feed. A full update will take to long and the weekly coveo recrawl will 
					//be scheduled to take place on the same day.
					if (content.Feed.type == FeedType.UPDATEDRECORDS)
					{
						List<string> coveoServerIps = config.CoveoServerIP.Split('|').ToList();
						string dealCollectionName = config.CoveoCollectionDeals;
						string dealSourceName = config.CoveoSourceDeals;
						string companyCollectionName = config.CoveoCollectionCompanies;
						string companySourceName = config.CoveoSourceCompanies;
						string drugCollectionName = config.CoveoCollectionDrugs;
						string drugSourceName = config.CoveoSourceDrugs;

						if (coveoServerIps.Count > 0)
						{
							PushUtil.PushUpdatesAsync(_dealUpdates, dealSourceName, dealCollectionName, coveoServerIps.ToArray());
							PushUtil.PushDeletesAsync(_dealDeletes, typeof(Deal), dealSourceName, dealCollectionName, coveoServerIps.ToArray());
							PushUtil.PushUpdatesAsync(_companyUpdates, companySourceName, companyCollectionName, coveoServerIps.ToArray());
							PushUtil.PushDeletesAsync(_companyDeletes, typeof(Company), companySourceName, companyCollectionName, coveoServerIps.ToArray());
							PushUtil.PushUpdatesAsync(_drugUpdates, drugSourceName, drugCollectionName, coveoServerIps.ToArray());
							PushUtil.PushDeletesAsync(_drugDeletes, typeof(Drug), drugSourceName, drugCollectionName, coveoServerIps.ToArray());
						}
					}
				}

				//If it is an "all update" note any discrepancies and send them to the distro
				if (content.Feed.type == FeedType.ALLRECORDS)
				{
					IEnumerable<IDCD> discrepancies = null;
					IEnumerable<IDCD> olderItems = null;
				    IEnumerable<IDCD> noContentItems = null;
					string type = string.Empty;
					if (content.type == IBIContentType.deal)
					{
						var imported = dcd.DealRecordImportLogs.Where(d => d.ImportLog == importLog);
						discrepancies = dcd.Deals.Where(d => !imported.Any(dl => dl.RecordId == d.RecordId)).Select(i => (IDCD)i);
						olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dcd.GetDeal(r.RecordId.Value));
                        noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dcd.GetDeal(r.RecordId.Value));
						type = "deals";
					}
					else if (content.type == IBIContentType.company)
					{
						var imported = dcd.CompanyRecordImportLogs.Where(c => c.ImportLog == importLog);
						discrepancies = dcd.Companies.Where(c => !imported.Any(co => co.RecordId == c.RecordId)).Select(i => (IDCD)i);
						olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dcd.GetCompany(r.RecordId.Value));
                        noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dcd.GetCompany(r.RecordId.Value));
						type = "companies";
					}
					else if (content.type == IBIContentType.drug)
					{
						var imported = dcd.DrugRecordImportLogs.Where(d => d.ImportLog == importLog);
						discrepancies = dcd.Drugs.Where(d => !imported.Any(dl => dl.RecordId == d.RecordId)).Select(i => (IDCD)i);
						olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dcd.GetDrug(r.RecordId.Value));
                        noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dcd.GetDrug(r.RecordId.Value));
						type = "drugs";
					}

					//Send email if there are any discrepancies
					bool hasDiscrepancies = (discrepancies != null && discrepancies.Count() > 0);
					bool hasOlderItems = (olderItems != null && olderItems.Count() > 0);
				    bool hasNoContentItems = noContentItems != null && noContentItems.Count() > 0;
                    if (hasDiscrepancies || hasOlderItems || hasNoContentItems)
					{
						string subject = Constants.BusinessAcronym + " discrepancy report for " + type + " import: " + fileName;
						StringBuilder sb = new StringBuilder();
						sb.AppendLine("There were discrepancies when processing the full import file: " + fileName);
						sb.AppendLine();
						if (hasDiscrepancies)
						{
							sb.AppendLine("These existing " + type + " were not altered by the import.");
							sb.AppendLine();
							foreach (var discrepancy in discrepancies)
							{
								sb.AppendLine(discrepancy.RecordNumber + " - " + discrepancy.Title);
							}
						}

						if (hasOlderItems)
						{
							sb.AppendLine("These existing " + type + " have a newer modified date in the database.");
							sb.AppendLine();
							foreach (var olderItem in olderItems)
							{
								sb.AppendLine(olderItem.RecordNumber + " - " + olderItem.Title);
							}
						}

                        if (hasNoContentItems)
                        {
                            sb.AppendLine("These existing " + type + " do not have a <Content> node in their record.");
                            sb.AppendLine();
                            foreach (var noContentItem in noContentItems)
                            {
                                sb.AppendLine(noContentItem.RecordNumber + " - " + noContentItem.Title);
                            }
                        }

						sendNotification(subject, sb.ToString());
					}
				}

				Event.RaiseEvent("dcdimport:end", content.type);
				Sitecore.Eventing.EventManager.QueueEvent<DCDImportEndRemoteEvent>(new DCDImportEndRemoteEvent(content.type));
			}
			catch (Exception e)
			{
				throwCriticalImportError("Error processing DCD XML doc.", e);
				return false;
			}

			//If we get this far, the import was a success
			//Log import completion
			importLog.ImportEnd = DateTime.Now;
			importLog.Result = (hasSomeFailures) ? ImportResult.PartialSuccess.ToString() : ImportResult.Success.ToString();
			importLog.Notes = importLogNotes.ToString();
			dcd.UpdateImportLogEntry(importLog);


			return true;
		}

		public void XMLValidationHandler(object sender, ValidationEventArgs args)
		{
			documentIsValid = false;
			importLogNotes.AppendLine(args.Message);
		}

		private void throwCriticalImportError(string error)
		{
			throwCriticalImportError(error, null);
		}

		private void throwCriticalImportError(string error, Exception e)
		{
			try
			{
				//Log the error in the database and the log
				importLogNotes.AppendLine(error);
				importLog.ImportEnd = DateTime.Now;
				importLog.Result = ImportResult.Failure.ToString();
				importLog.Notes = importLogNotes.ToString();
				dcd.UpdateImportLogEntry(importLog);
			}
			catch (Exception ex)
			{
				DCDImportLogger.Log.Error("Error writing log message to database", ex);
			}

			if (e != null)
			{
				DCDImportLogger.Log.Error(error, e);
			}
			else
			{
				DCDImportLogger.Log.Error(error);
			}

			string subject = "Error processing Deals, Companies, Drugs import file. ";
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("There was an error processing the import file: " + fileName);
			sb.AppendLine();
			sb.AppendLine("ERROR:");
			sb.AppendLine();
			sb.AppendLine(error);
			if (e != null)
			{
				sb.AppendLine(e.Message);
				if (e.InnerException != null)
				{
					sb.AppendLine(e.InnerException.Message);
				}
			}

			if (!string.IsNullOrEmpty(importLogNotes.ToString()))
			{
				sb.AppendLine(importLogNotes.ToString());
			}

			sendNotification(subject, sb.ToString());
		}

		private bool sendNotification(string subject, string body)
		{
			var mailer = new EmailSender();
			var message = new MailMessage();
			var from = new MailAddress(Constants.EmailNoReplySenderAddress);

			message.IsBodyHtml = false;

			foreach (var email in dcdConfig.GetEmailDistributionList())
			{
				if (string.IsNullOrEmpty(email)) continue;
				message.To.Add(new MailAddress(email));
			}

			message.Body = body;
			message.Subject = subject;
			message.From = from;
			return mailer.SendEmail(message);
		}
	}
}
