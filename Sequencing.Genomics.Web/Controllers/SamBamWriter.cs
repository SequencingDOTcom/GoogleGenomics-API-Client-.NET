using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.Extensions;
using Bio.IO;
using Bio.IO.BAM;
using Bio.IO.SAM;
using Google.Apis.Genomics.v1beta.Data;

namespace Sequencing.Genomics.Web.Controllers
{
    public class SamBamWriter
    {
        private readonly bool samOutput;

        public SamBamWriter(bool samOutput)
        {
            this.samOutput = samOutput;
        }

        public void Write(IEnumerable<Read> reads, Stream targetStream)
        {
            if (reads == null)
                reads = new Read[] {};
            var _samFormatter = samOutput ? (IFormatter<ISequenceAlignment>)new SAMFormatter() : new BAMFormatter();
            var _samAlignedSequences = reads.Select(SamReadsConverter.Convert).ToList();
            var _sa = new SequenceAlignment();
            foreach (var _samAlignedSequence in _samAlignedSequences)
                _sa.AlignedSequences.Add(_samAlignedSequence);
            var _refSequenceName = "1";
            if (_samAlignedSequences.Count != 0)
                _refSequenceName = _samAlignedSequences[0].RName;
            _sa.Metadata.Add("SAMAlignmentHeader",
                new SAMAlignmentHeader
                {
                    ReferenceSequences =
                    {
                        new ReferenceSequenceInfo
                        {
                            Name = _refSequenceName
                        }
                    }
                });
            _samFormatter.Format(targetStream, _sa);
        }
    }
}