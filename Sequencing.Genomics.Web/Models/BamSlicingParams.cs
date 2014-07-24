namespace Sequencing.Genomics.Web.Models
{
    public class BamSlicingParams
    {
        public string ReadsetName { get; set; }
        public string ReadsetId { get; set; }
        public string ChrNum { get; set; }
        public ulong ChrStartPos { get; set; }
        public ulong ChrEndPos { get; set; }
        public bool SamOutput { get; set; }
    }
}