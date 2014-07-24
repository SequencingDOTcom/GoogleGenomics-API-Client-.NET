using System;
using System.Collections.Generic;
using System.IO;
using Google.Apis.Genomics.v1beta.Data;

namespace Sequencing.Genomics.Web.Controllers
{
    public interface IGgServerWorker
    {
        List<Readset> GetReadsets(string datasetId);
        List<Read> SearchReads(string readsetId, string chr, ulong pos1, ulong pos2);
        void WriteReads(string readsetId, string chr, ulong pos1, ulong pos2, Action<IList<Read>> writeAction);
    }
}