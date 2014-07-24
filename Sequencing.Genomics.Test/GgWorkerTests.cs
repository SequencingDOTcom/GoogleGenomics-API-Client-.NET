using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Genomics.v1beta.Data;
using MbUnit.Framework;
using Sequencing.Genomics.Web.Controllers;

namespace Sequencing.Genomics.Test
{
    [TestFixture]
    public class GgWorkerTests
    {
        [Test]
        public void TestCachedWorker()
        {
            var _sampleWorker = new SampleWorker();
            var _cachedServerWorker = new CachedServerWorker(_sampleWorker);
            Assert.AreEqual(0, _sampleWorker.GetReadsetsCallCounter);
            _cachedServerWorker.GetReadsets("a");
            Assert.AreEqual(1, _sampleWorker.GetReadsetsCallCounter);
            _cachedServerWorker.GetReadsets("a");
            Assert.AreEqual(1, _sampleWorker.GetReadsetsCallCounter);
            _cachedServerWorker.GetReadsets("b");
            Assert.AreEqual(2, _sampleWorker.GetReadsetsCallCounter);
            _cachedServerWorker.GetReadsets("b");
            Assert.AreEqual(2, _sampleWorker.GetReadsetsCallCounter);
        }

        private class SampleWorker : IGgServerWorker
        {
            public int GetReadsetsCallCounter = 0;
            
            public List<Readset> GetReadsets(string datasetId)
            {
                GetReadsetsCallCounter++;
                return new List<Readset>();
            }

            public List<Read> SearchReads(string readsetId, string chr, ulong pos1, ulong pos2)
            {
                throw new NotImplementedException();
            }

            public void WriteReads(string readsetId, string chr, ulong pos1, ulong pos2, Action<IList<Read>> writeAction)
            {
                throw new NotImplementedException();
            }
        }
    }
}
