using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public partial class Deal : IDCD
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.Deals.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            context.Deals.DeleteOnSubmit(this);
        }

        public DateTime Published { get; set; }

        //public string Content { get; set; }
        //public List<string> GetCompanyRecordIDs()
        //{
        //    return DealContent.DealCompanies.Select(i => i.Company.id).ToList();
        //}
    }

    public partial class Company : IDCD
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.Companies.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            //Remove related companies first
            IQueryable<RelatedCompany> relatedRows = context.RelatedCompanies.Where(c => c.CompanyRecordId == this.RecordId);
            context.RelatedCompanies.DeleteAllOnSubmit(relatedRows);
            context.Companies.DeleteOnSubmit(this);
        }

        //public DateTime Published { get; set; }

        //public string Content { get; set; }
    }

    public partial class Drug : IDCD
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.Drugs.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            context.Drugs.DeleteOnSubmit(this);
        }

        public DateTime Published { get; set; }

        //public string Content { get; set; }
    }

    public partial class DealRecordImportLog : IDCDRecordImportLog
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.DealRecordImportLogs.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            context.DealRecordImportLogs.DeleteOnSubmit(this);
        }
    }

    public partial class CompanyRecordImportLog : IDCDRecordImportLog
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.CompanyRecordImportLogs.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            context.CompanyRecordImportLogs.DeleteOnSubmit(this);
        }
    }

    public partial class DrugRecordImportLog : IDCDRecordImportLog
    {
        public void InsertRecordOnSubmit(DCDContext context)
        {
            context.DrugRecordImportLogs.InsertOnSubmit(this);
        }

        public void DeleteRecordOnSubmit(DCDContext context)
        {
            context.DrugRecordImportLogs.DeleteOnSubmit(this);
        }
    }

    public partial class RelatedCompany //: ILink
    {
        public string SitecoreId
        {
            get { return string.Empty; }
        }

        public string LinkTitle
        {
            get
            {
                int i = RelatedCompanyPath.LastIndexOf("/");
                if (i == -1)
                {
                    return RelatedCompanyPath;
                }
                else
                {
                    return RelatedCompanyPath.Substring(i + 1);
                }
            }
        }

        public string LinkUrl
        {
            get
            {
                int asInt = Convert.ToInt32(RelatedCompanyRecordNumber);
                string recordNumber = CompanyRecordCache.Instance.Get(asInt);
                return string.Format(Sitecore.Configuration.Settings.GetSetting("DCD.OldCompaniesURL"), recordNumber);
            }
        }
    }
}
