﻿using Informa.Model.DCD;

namespace Informa.Library.Services.NlmExport.Parser.Legacy.Link
{
    public class CompanyLink : AssetLinkBase
    {
        public override string GetLink(string linkId)
        {
            var finder = new DCDManager();
            var company = finder.GetCompanyByRecordNumber(linkId);

            if (company == null)
            {
                return string.Empty;
            }

            return company.RecordNumber;
        }

        public override string GetLinkText(string linkId)
        {
            var finder = new DCDManager();
            var company = finder.GetCompanyByRecordNumber(linkId);

            if (company == null)
            {
                return string.Empty;
            }

            return company.Title;
        }

        public override string LinkType
        {
            get { return "company"; }
        }
    }
}
