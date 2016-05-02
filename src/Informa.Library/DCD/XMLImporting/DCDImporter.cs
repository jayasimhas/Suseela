using Informa.Models.DCD;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Informa.Library.DCD.XMLImporting
{
    public class DCDImporter
    {
        private string _documentPath { get; set; }
        private string _fileName { get; set; }
        private ImportLog _importingLogRecord;
        private StringBuilder _importLogNotes;
        private bool _hasSomeFailures;

        private readonly List<IDCD> _dealUpdates;
        private readonly List<IDCD> _companyUpdates;
        private readonly List<IDCD> _drugUpdates;
        private readonly List<string> _dealDeletes;
        private readonly List<string> _companyDeletes;
        private readonly List<string> _drugDeletes;

        //private DCDContext dcContext
        //{
        //    get { return new DCDContext(); }
        //}

        public DCDImporter(string path, string name)
        {
            _documentPath = path;
            _fileName = name;
            _importLogNotes = new StringBuilder();

            _dealUpdates = new List<IDCD>();
            _dealDeletes = new List<string>();
            _companyUpdates = new List<IDCD>();
            _companyDeletes = new List<string>();
            _drugUpdates = new List<IDCD>();
            _drugDeletes = new List<string>();
        }

        public bool ProcessFile()
        {
            using (DCDContext dcCntxt = new DCDContext())
            {
                //Initialize the database log entry for this import
                dcCntxt.CreateImportLogEntry(DateTime.Now, _fileName, out _importingLogRecord);
            }

            if (_importingLogRecord == null)
            {
                return false;
            }

            try
            {
                //Set the DTD validation options
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ProhibitDtd = false;
                settings.ValidationType = ValidationType.DTD;
                settings.ValidationEventHandler += new ValidationEventHandler(XMLValidationHandler);
                settings.CheckCharacters = false;

                XmlReader reader = XmlReader.Create(_documentPath, settings);
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
                    DCDImportLogger.ThrowCriticalImportError("DCD XML doc de-serialization failed! ", e, _fileName, _importingLogRecord, _importLogNotes);
                    return false;
                }
                reader.Close();

                //If there was an error validating the XML file with the appropriate DTD file
                if (!documentIsValid)
                {
                    DCDImportLogger.ThrowCriticalImportError("DCD XML doc validation failed! " + _importLogNotes, _fileName, _importingLogRecord, _importLogNotes);
                    return false;
                }

                //Our XML file is good. Quickly validate that the number of records is correct.
                int numRecords;
                if (!int.TryParse(content.Feed.NumberOfRecords, out numRecords))
                {
                    DCDImportLogger.ThrowCriticalImportError("Error processing DCD XML doc. Number of records value is not a valid integer.", _fileName, _importingLogRecord, _importLogNotes);
                    return false;
                }
                if (numRecords != content.RecordSet.Count)
                {
                    DCDImportLogger.ThrowCriticalImportError("Error processing DCD XML doc. Number of records value does not equal the record set size.", _fileName, _importingLogRecord, _importLogNotes);
                    return false;
                }

                //We're past the critical error point. Time to process each record
                foreach (Record record in content.RecordSet)
                {
                    using (DCDContext dc = new DCDContext())
                    {
                        bool success = false;
                        if (content.type == IBIContentType.deal)
                        {
                            //_importingLogRecord = new Model.DCD.DCDManager().GetImportLogById(_importingLogRecord.Id);
                            success = dc.ProcessRecord<Deal, DealRecordImportLog>(record, _importingLogRecord);
                            if (success)
                            {
                                if (record.Command == CommandType.Delete)
                                {
                                    _dealDeletes.Add(record.Identification.RecordNumber);
                                }
                                else
                                {
                                    _dealUpdates.Add(dc.GetRecord<Deal>(record.Identification.RecordId));
                                }
                            }
                        }
                        else if (content.type == IBIContentType.company)
                        {
                            success = dc.ProcessRecord<Informa.Models.DCD.Company, CompanyRecordImportLog>(record, _importingLogRecord);
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
                                    success = dc.AddRelatedCompanies(record.Identification.RecordId, paths);
                                }

                                if (record.Command == CommandType.Delete)
                                {
                                    _companyDeletes.Add(record.Identification.RecordNumber);
                                }
                                else
                                {
                                    _companyUpdates.Add(dc.GetRecord<Informa.Models.DCD.Company>(record.Identification.RecordId));
                                }
                            }
                        }
                        else if (content.type == IBIContentType.drug)
                        {
                            success = dc.ProcessRecord<Drug, DrugRecordImportLog>(record, _importingLogRecord);
                            if (success)
                            {
                                if (record.Command == CommandType.Delete)
                                {
                                    _drugDeletes.Add(record.Identification.RecordNumber);
                                }
                                else
                                {
                                    _drugUpdates.Add(dc.GetRecord<Drug>(record.Identification.RecordId));
                                }
                            }
                        }

                        if (!success)
                        {
                            _hasSomeFailures = true;
                        }
                    }
                }

                //COVEO INDEX REBUILDING REMOVED

                using (DCDContext dc = new DCDContext())
                {
                    //If it is an "all update" note any discrepancies and send them to the distro
                    if (content.Feed.type == FeedType.ALLRECORDS)
                    {
                        IEnumerable<IDCD> discrepancies = null;
                        IEnumerable<IDCD> olderItems = null;
                        IEnumerable<IDCD> noContentItems = null;
                        string type = string.Empty;
                        if (content.type == IBIContentType.deal)
                        {
                            var imported = dc.DealRecordImportLogs.Where(d => d.ImportLog == _importingLogRecord);
                            discrepancies = dc.Deals.Where(d => !imported.Any(dl => dl.RecordId == d.RecordId)).Select(i => (IDCD)i);
                            olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dc.Deals.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dc.Deals.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            type = "deals";
                        }
                        else if (content.type == IBIContentType.company)
                        {
                            var imported = dc.CompanyRecordImportLogs.Where(c => c.ImportLog == _importingLogRecord);
                            discrepancies = dc.Companies.Where(c => !imported.Any(co => co.RecordId == c.RecordId)).Select(i => (IDCD)i);
                            olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dc.Companies.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dc.Companies.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            type = "companies";
                        }
                        else if (content.type == IBIContentType.drug)
                        {
                            var imported = dc.DrugRecordImportLogs.Where(d => d.ImportLog == _importingLogRecord);
                            discrepancies = dc.Drugs.Where(d => !imported.Any(dl => dl.RecordId == d.RecordId)).Select(i => (IDCD)i);
                            olderItems = imported.Where(r => r.Notes == "Existing record has a newer Last Modified Date.").Select(r => (IDCD)dc.Drugs.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            noContentItems = imported.Where(r => r.Notes == "Record does not have any content.").Select(r => (IDCD)dc.Drugs.FirstOrDefault(c => c.RecordId == r.RecordId.Value));
                            type = "drugs";
                        }

                        //Send email if there are any discrepancies
                        bool hasDiscrepancies = (discrepancies != null && discrepancies.Count() > 0);
                        bool hasOlderItems = (olderItems != null && olderItems.Count() > 0);
                        bool hasNoContentItems = noContentItems != null && noContentItems.Count() > 0;
                        if (hasDiscrepancies || hasOlderItems || hasNoContentItems)
                        {
                            string subject = DCDConstants.BusinessAcronym + " discrepancy report for " + type + " import: " + _fileName;
                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("There were discrepancies when processing the full import file: " + _fileName);
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

                            DCDImportLogger.SendNotification(subject, sb.ToString());
                        }
                    }
                }

                Event.RaiseEvent("dcdimport:end", content.type);
                Sitecore.Eventing.EventManager.QueueEvent<DCDImportEndRemoteEvent>(new DCDImportEndRemoteEvent(content.type));
            }
            catch (Exception e)
            {
                DCDImportLogger.ThrowCriticalImportError("Error processing DCD XML doc.", e, _fileName, _importingLogRecord, _importLogNotes);
                return false;
            }

            //If we get this far, the import was a success
            //Log import completion
            _importingLogRecord.ImportEnd = DateTime.Now;
            _importingLogRecord.Result = (_hasSomeFailures) ? ImportResult.PartialSuccess.ToString() : ImportResult.Success.ToString();
            _importingLogRecord.Notes = _importLogNotes.ToString();

            using (DCDContext dcdContext = new DCDContext())
                dcdContext.UpdateImportLogEntry(_importingLogRecord);

            return true;
        }

        private bool documentIsValid = true;
        private void XMLValidationHandler(object sender, ValidationEventArgs e)
        {
            documentIsValid = false;
            _importLogNotes.AppendLine(e.Message);
        }
    }
}

