using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Caching;
using Google.Apis.Genomics.v1beta.Data;

namespace Sequencing.Genomics.Web.Controllers
{
    public class CachedServerWorker : IGgServerWorker
    {
        private const string DATASET = "DATASET";
        private readonly IGgServerWorker impl;
        private static object syncRoot = new object();

        public CachedServerWorker(IGgServerWorker impl)
        {
            this.impl = impl;
        }

        public List<Readset> GetReadsets(string datasetId)
        {
            lock (syncRoot)
            {
                var _key = DATASET+datasetId;
                var _o = MemoryCache.Default.Get(_key);
                if (_o != null)
                    return (List<Readset>) _o;
                var _readsets = impl.GetReadsets(datasetId);
                MemoryCache.Default.Add(_key, _readsets, DateTimeOffset.MaxValue);
                return _readsets;
            }
        }

        public void WriteReads(string readsetId, string chr, ulong pos1, ulong pos2, Action<IList<Read>>  s)
        {
            impl.WriteReads(readsetId, chr, pos1, pos2, s);
        }

        public List<Read> SearchReads(string readsetId, string chr, ulong pos1, ulong pos2)
        {
            return impl.SearchReads(readsetId, chr, pos1, pos2);
        }
    }
}