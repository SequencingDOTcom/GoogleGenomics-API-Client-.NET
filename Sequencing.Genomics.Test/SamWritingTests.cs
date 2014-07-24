using System.Collections.Generic;
using System.IO;
using Google.Apis.Genomics.v1beta.Data;
using MbUnit.Framework;
using Newtonsoft.Json;
using Sequencing.Genomics.Web.Controllers;

namespace Sequencing.Genomics.Test
{
    [TestFixture]
    public class SamWritingTests
    {
        [Test]
        public void TestReadsConversion()
        {
            var _list = JsonConvert.DeserializeObject<List<Read>>(File.ReadAllText("SampleRead.json"));
            var _list2 = _list.ConvertAll(SamReadsConverter.Convert);
            Assert.AreEqual(_list[0].Tags.Count, _list2[0].OptionalFields.Count);
            Assert.Count(_list.Count, _list2);
        }

        [Test]
        public void TestSamGeneration()
        {
            var _list = JsonConvert.DeserializeObject<List<Read>>(File.ReadAllText("SampleRead.json"));
            var _targetStream = new MemoryStream();
            new SamBamWriter(true).Write(_list, _targetStream);
            Assert.AreNotEqual(0, _targetStream.Length);
        }

        [Test]
        public void TestSamGenerationEmpty()
        {
            var _targetStream = new MemoryStream();
            new SamBamWriter(true).Write(null, _targetStream);
       }

        [Test]
        public void TestBamGeneration()
        {
            var _list = JsonConvert.DeserializeObject<List<Read>>(File.ReadAllText("SampleRead.json"));
            var _targetStream = new MemoryStream();
            new SamBamWriter(false).Write(_list, _targetStream);
            Assert.AreNotEqual(0, _targetStream.Length);
        }
    }
}