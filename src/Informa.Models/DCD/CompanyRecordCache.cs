using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public class CompanyRecordCache
    {
        protected static volatile CompanyRecordCache _instance = null;
        private static readonly object padlock = new object();

        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim(); // normally would dispose, but singleton instance lives for entire appdomain lifetime
        private readonly Dictionary<int, string> _recordIdToNumberCache;

        /// <summary>
        /// Private constructor that initializes a new instance of the <see cref="CompanyRecordCache"/> class.
        /// </summary>
        protected CompanyRecordCache()
        {
            _recordIdToNumberCache = new Dictionary<int, string>();
        }

        public void Set(int recordId, string recordNumber)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                if (_recordIdToNumberCache.ContainsKey(recordId))
                {
                    _recordIdToNumberCache.Remove(recordId);
                }
                _recordIdToNumberCache.Add(recordId, recordNumber);
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns the associated record number to the record id
        /// </summary>
        /// <param name="recordId">The id whose associated record number is being searched for.</param>
        /// <returns>The corresponding record number to the input record id. If the record id is not found, null is returned.</returns>
        public string Get(int recordId)
        {
            _readWriteLock.EnterUpgradeableReadLock();
            try
            {
                string recordNumber;
                // If the recordNumber is not found successfully, set it.
                if (!_recordIdToNumberCache.TryGetValue(recordId, out recordNumber))
                {
                    _readWriteLock.EnterWriteLock();
                    try
                    {
                        // Second check is optimistic
                        if (_recordIdToNumberCache.TryGetValue(recordId, out recordNumber))
                        {
                            return recordNumber;
                        }
                        // Otherwise, continue to grab from the database
                        using (var dcd = new DCDContext())
                        {
                            // Get the recordnumber
                            Company company = dcd.Companies.FirstOrDefault(f => f.RecordId == recordId);
                            // Return null if the company record isn't found.
                            if (company == null)
                            {
                                return null;
                            }
                            recordNumber = company.RecordNumber;
                            // Set it in the cache so we don't have to hit the database next time.
                            _recordIdToNumberCache[recordId] = recordNumber;
                        }
                    }
                    finally
                    {
                        _readWriteLock.ExitWriteLock();
                    }
                }
                return recordNumber;
            }
            finally
            {
                _readWriteLock.ExitUpgradeableReadLock();
            }
        }

        public static CompanyRecordCache Instance
        {
            get
            {
                //Check to see if we need to initialize
                if (_instance == null)
                {
                    //Lock to keep more than one thread from initializing at once
                    lock (padlock)
                    {
                        //We need to recheck, because threads that were waiting at the lock
                        //won't want to reinitialize after the first thread went thru and initialized
                        if (_instance == null)
                        {
                            _instance = new CompanyRecordCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public void ClearIndex()
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                _recordIdToNumberCache.Clear();
                GC.Collect(); // Is this really necessary? 9/10 times this is a bad idea...
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
