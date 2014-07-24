using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Genomics.v1beta;
using Google.Apis.Genomics.v1beta.Data;
using Google.Apis.Services;
using Newtonsoft.Json;

namespace Sequencing.Genomics.Web.Controllers
{
    public class GgApiServerWorkerImpl : IGgServerWorker
    {
        private GenomicsService CreateService()
        {
            var _certificate =
                new X509Certificate2(
                    HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["GgCertificateKeyPath"]),
                    ConfigurationManager.AppSettings["GgCertificatePassword"], X509KeyStorageFlags.Exportable);

            var _creds = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(ConfigurationManager.AppSettings["GgAccountEmail"])
                {
                    Scopes = new[] { "https://www.googleapis.com/auth/genomics" }
                }.FromCertificate(_certificate));

            var _genomicsService = new GenomicsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _creds,
                ApplicationName = "Genomic BAM Slicer",

            });
            return _genomicsService;
        }

        public List<Readset> GetReadsets(string datasetId)
        {
            var _res = new List<Readset>();
            var _genomicsService = CreateService();
            var _rq = new SearchReadsetsRequest { DatasetIds = new[] { datasetId }, MaxResults = 256 };
            do
            {
                var _rs = _genomicsService.Readsets.Search(_rq).Execute();
                _res.AddRange(_rs.Readsets);
                _rq.PageToken = _rs.NextPageToken;
                break;
            } while (_rq.PageToken != null);
            return _res;
        }

        public List<Read> SearchReads(string readsetId, string chr, ulong pos1, ulong pos2)
        {
            var _res = new List<Read>();
            var _genomicsService = CreateService();
            var _searchReadsRequest = new SearchReadsRequest
                                      {
                                          ReadsetIds = new[] { readsetId },
                                          SequenceName = chr,
                                          SequenceStart = pos1,
                                          SequenceEnd = pos2,
                                          MaxResults = 1024
                                      };
            do
            {
                var _rq = _genomicsService.Reads.Search(_searchReadsRequest);
                var _rs = _rq.Execute();
                _searchReadsRequest.PageToken = _rs.NextPageToken;
                _res.AddRange(_rs.Reads);
            } while (_searchReadsRequest.PageToken != null);

            return _res;
        }

        public void WriteReads(string readsetId, string chr, ulong pos1, ulong pos2, Action<IList<Read>> writeAction)
        {
            var _genomicsService = CreateService();
            var _searchReadsRequest = new SearchReadsRequest
            {
                ReadsetIds = new[] { readsetId },
                SequenceName = chr,
                SequenceStart = pos1,
                SequenceEnd = pos2,
                MaxResults = 1024
            };
            do
            {
                var _rq = _genomicsService.Reads.Search(_searchReadsRequest);
                var _rs = _rq.Execute();
                _searchReadsRequest.PageToken = _rs.NextPageToken;
                writeAction(_rs.Reads);
                HttpContext.Current.Response.Flush();
            } while (_searchReadsRequest.PageToken != null);
        }
    }
}