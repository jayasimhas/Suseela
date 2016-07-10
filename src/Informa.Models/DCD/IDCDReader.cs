using System.Collections.Generic;
using Informa.Models.DCD;

namespace Informa.Model.DCD
{
	public interface IDCDReader
	{
		Drug GetDrugByRecordNumber(string recordNumber);
		Drug GetDrugByRecordId(int recordId);
		Deal GetDealByRecordNumber(string recordNumber);
		Deal GetDealByRecordId(int recordId);
		Company GetCompanyByRecordNumber(string recordNumber);
		Company GetCompanyByRecordId(int recordId);
		List<Company> GetAllCompanies();
		List<RelatedCompany> GetAllRelatedCompanies();
	}
}