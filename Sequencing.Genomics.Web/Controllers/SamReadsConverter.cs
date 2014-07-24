using System;
using System.Collections.Generic;
using System.Linq;
using Bio;
using Bio.IO.SAM;
using Google.Apis.Genomics.v1beta.Data;

namespace Sequencing.Genomics.Web.Controllers
{
    public class SamReadsConverter
    {
        public static SAMAlignedSequence Convert(Read read)
        {
            var _sq = new SAMAlignedSequence
                                      {
                                          CIGAR = read.Cigar,
                                          ISize = read.TemplateLength ?? 0,
                                          MPos = read.MatePosition ?? 0,
                                          Pos = read.Position ?? 0,
                                          MapQ = read.MappingQuality ?? 0,
                                          RName = read.ReferenceSequenceName,
                                          QName = read.Name,
                                          QuerySequence =
                                              new QualitativeSequence(SAMDnaAlphabet.Instance, FastQFormatType.Sanger,
                                              read.OriginalBases,
                                              read.BaseQuality) {ID = read.Id},
                                              MRNM = read.MateReferenceSequenceName ?? "*"
                                      };
            foreach (var _tag in read.Tags)
                _sq.OptionalFields.Add(CreateSamField(_tag));
            if (read.Flags.HasValue)
                _sq.Flag = (SAMFlags) read.Flags;
            return _sq;
        }

        private static SAMOptionalField CreateSamField(KeyValuePair<string, IList<string>> tag)
        {
            var _res = new SAMOptionalField{Tag = tag.Key};
            string _value;
            if (tag.Value.Count != 1)
                throw new Exception("Values count doesn't equal 1, unexpected:" + tag.Value.Aggregate("", (s, s1) => s + "," + s1));
            _res.Value = tag.Value[0];
            if (tagTypesLookup.TryGetValue(tag.Key, out _value))
                _res.VType = _value;
            else
                _res.VType = "Z";
            //else if (tag.Key.StartsWith("X") || tag.Key.StartsWith("Y") || tag.Key.StartsWith("Z"))
            //    _res.VType = tag.Key.Substring(1, 1);
            //else
            //    throw new Exception("Unable to locate type for tag:"+tag.Key+",value:"+tag.Value.Aggregate("", (s, s1) => s+","+s1));
            
            return _res;
        }

        private static Dictionary<string, string> tagTypesLookup = new Dictionary<string, string>();

        static SamReadsConverter()
        {
            tagTypesLookup.Add("AM", "i");
            tagTypesLookup.Add("AS", "i");
            tagTypesLookup.Add("BC", "Z");
            tagTypesLookup.Add("BQ", "Z");
            tagTypesLookup.Add("CC", "Z");
            tagTypesLookup.Add("CM", "i");
            tagTypesLookup.Add("CO", "Z");
            tagTypesLookup.Add("CP", "i");
            tagTypesLookup.Add("CQ", "Z");
            tagTypesLookup.Add("CS", "Z");
            tagTypesLookup.Add("CT", "Z");
            tagTypesLookup.Add("E2", "Z");
            tagTypesLookup.Add("FI", "i");
            tagTypesLookup.Add("FS", "Z");
            tagTypesLookup.Add("FZ", "B");
            tagTypesLookup.Add("LB", "Z");
            tagTypesLookup.Add("H0", "i");
            tagTypesLookup.Add("H1", "i");
            tagTypesLookup.Add("H2", "i");
            tagTypesLookup.Add("HI", "i");
            tagTypesLookup.Add("IH", "i");
            tagTypesLookup.Add("MC", "Z");
            tagTypesLookup.Add("MD", "Z");
            tagTypesLookup.Add("MQ", "i");
            tagTypesLookup.Add("NH", "i");
            tagTypesLookup.Add("NM", "i");
            tagTypesLookup.Add("OQ", "Z");
            tagTypesLookup.Add("OP", "i");
            tagTypesLookup.Add("OC", "Z");
            tagTypesLookup.Add("PG", "Z");
            tagTypesLookup.Add("PQ", "i");
            tagTypesLookup.Add("PT", "Z");
            tagTypesLookup.Add("PU", "Z");
            tagTypesLookup.Add("QT", "Z");
            tagTypesLookup.Add("Q2", "Z");
            tagTypesLookup.Add("R2", "Z");
            tagTypesLookup.Add("RG", "Z");
            tagTypesLookup.Add("RT", "Z");
            tagTypesLookup.Add("SA", "Z");
            tagTypesLookup.Add("SM", "i");
            tagTypesLookup.Add("TC", "i");
            tagTypesLookup.Add("U2", "Z");
            tagTypesLookup.Add("UQ", "i");
        }
    }
}