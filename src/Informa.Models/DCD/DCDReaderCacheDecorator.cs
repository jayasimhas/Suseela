using System;
using System.Collections.Generic;
using Informa.Models.DCD;
using Jabberwocky.Core.Caching;

namespace Informa.Model.DCD
{
	public class DCDReaderCacheDecorator : IDCDReader
	{
		private readonly IDCDReader _reader;
		private readonly ICacheProvider _cache;
		private readonly TimeSpan _timeSpan = new TimeSpan(0, 5, 0);
		public DCDReaderCacheDecorator(IDCDReader reader, ICacheProvider cacheProvider)
		{
			_reader = reader;
			_cache = cacheProvider;
		}

		public Drug GetDrugByRecordNumber(string recordNumber)
		{
			return _cache.GetFromCache($"DCDManager:GetDrugsByRecordNumber:{recordNumber}", _timeSpan, () => _reader.GetDrugByRecordNumber(recordNumber));
		}

		public Drug GetDrugByRecordId(int recordId)
		{
			return _cache.GetFromCache($"DCDManager:GetDrugByRecordId:{recordId}", _timeSpan, () => _reader.GetDrugByRecordId(recordId));
		}

		public Deal GetDealByRecordNumber(string recordNumber)
		{
			return _cache.GetFromCache($"DCDManager:GetDealByRecordNumber:{recordNumber}", _timeSpan, () => _reader.GetDealByRecordNumber(recordNumber));
		}

		public Deal GetDealByRecordId(int recordId)
		{
			return _cache.GetFromCache($"DCDManager:GetDealByRecordId:{recordId}", _timeSpan, () => _reader.GetDealByRecordId(recordId));
		}

		public Company GetCompanyByRecordNumber(string recordNumber)
		{
			return _cache.GetFromCache($"DCDManager:GetCompanyByRecordNumber:{recordNumber}", _timeSpan, () => _reader.GetCompanyByRecordNumber(recordNumber));
		}

		public Company GetCompanyByRecordId(int recordId)
		{
			return _cache.GetFromCache($"DCDManager:GetCompanyByRecordId:{recordId}", _timeSpan, () => _reader.GetCompanyByRecordId(recordId));
		}

		public List<Company> GetAllCompanies()
		{
			return _cache.GetFromCache("DCDManager:GetAllCompanies", _timeSpan, () => _reader.GetAllCompanies());
		}

		public List<RelatedCompany> GetAllRelatedCompanies()
		{
			return _cache.GetFromCache("DCDManager:GetAllRelatedCompanies", _timeSpan, () => _reader.GetAllRelatedCompanies());
		}
	}
}