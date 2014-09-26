using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Google.Apis.Genomics.v1beta.Data;
using Mvc.JQuery.Datatables;
using Sequencing.Genomics.Web.Models;

namespace Sequencing.Genomics.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGgServerWorker serverWorker = new CachedServerWorker(new GgApiServerWorkerImpl());

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BamHome()
        {
            return View(GetPublicDatasets());
        }

        private List<Dataset> GetPublicDatasets()
        {
            return new List<Dataset>
                   {
                       new Dataset {Id = "10473108253681171589", Name = "1000 Genomes"},
                       new Dataset {Id = "383928317087", Name = "PGP"},
                       new Dataset {Id = "461916304629", Name = "Simons Foundation"},
                       new Dataset {Id = "337315832689", Name = "DREAM SMC Challenge"},
                   };
        }

        public DataTablesResult<ReadsetInfo> GetReadsets(DataTablesParamRs dataTableParam)
        {
            var _readsets = serverWorker.GetReadsets(dataTableParam.DatasetId);
            return DataTablesResult.Create(
                _readsets.Select(readset => new ReadsetInfo {Id = readset.Id, Name = readset.Name}).AsQueryable(), dataTableParam);
        }

        public class DataTablesParamRs : DataTablesParam
        {
            public string DatasetId { get; set; }
        }

        public ActionResult CreateBamFile(BamSlicingParams pars)
        {
            Response.BufferOutput = false;
            Response.ContentType = "application/octet-stream";
            var _extension = pars.SamOutput ? "sam" : "bam";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + pars.ReadsetName + "_CHR" 
                + pars.ChrNum+"_"+pars.ChrStartPos+"_"+pars.ChrEndPos+"."+_extension);
            serverWorker.WriteReads(pars.ReadsetId, pars.ChrNum, pars.ChrStartPos, pars.ChrEndPos,
                delegate(IList<Read> list)
                {
                    new SamBamWriter(pars.SamOutput).Write(list, Response.OutputStream);
                    Response.OutputStream.Flush();
                } );
            return null;
        }
    }
}
