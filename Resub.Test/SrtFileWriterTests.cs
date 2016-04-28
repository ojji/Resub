using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Resub.Test
{
    public class SrtFileWriterTests
    {
        [Test]
        public void Should_write_every_subtitle_to_the_file()
        {
            var internalWriter = new StringWriter();
            var sut = new SrtFileWriter(internalWriter);

            sut.WriteSubtitlesToFile(SampleSubtitles);

            Assert.That(internalWriter.ToString(), Is.EqualTo(CorrectSampleOutput));
        }

        private List<Subtitle> SampleSubtitles => new List<Subtitle>
        {
             new Subtitle(@"1
00:00:55,848 --> 00:00:58,348
BEMUTATJUK EGY SEGGFEJ FILMJÉT
BEMUTATJUK EGY SEGGFEJ FILMJÉT
"),
            new Subtitle(@"2
00:00:59,810 --> 00:01:02,310
A FŐSZEREPEKBEN: ISTEN BARMA
"),
            new Subtitle(@"3
00:01:02,604 --> 00:01:04,654
A LEGSZEXIBB FÉRFI A FÖLDÖN!
")
        };

        private string CorrectSampleOutput => @"1
00:00:55,848 --> 00:00:58,348
BEMUTATJUK EGY SEGGFEJ FILMJÉT
BEMUTATJUK EGY SEGGFEJ FILMJÉT

2
00:00:59,810 --> 00:01:02,310
A FŐSZEREPEKBEN: ISTEN BARMA

3
00:01:02,604 --> 00:01:04,654
A LEGSZEXIBB FÉRFI A FÖLDÖN!

";
    }
}