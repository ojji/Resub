using System.IO;
using System.Text;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace Resub.Test
{
    public class SrtFileReaderTests
    {
        const string SrtSample = @"1
00:00:55,848 --> 00:00:58,348
BEMUTATJUK EGY SEGGFEJ FILMJÉT

2
00:00:59,810 --> 00:01:02,310
A FŐSZEREPEKBEN: ISTEN BARMA



3
00:01:02,604 --> 00:01:04,654
A LEGSZEXIBB FÉRFI A FÖLDÖN!

4
00:01:07,693 --> 00:01:10,113
EGY DÖGÖS CSAJSZI

";

        const string Malformed_NoIndex_SrtSample = @"no index
00:00:55,848 --> 00:00:58,348
BEMUTATJUK EGY SEGGFEJ FILMJÉT

";
        const string Malformed_NoTimestamp_SrtSample = @"1
NO TIMESTAMP --> INVALID
BEMUTATJUK EGY SEGGFEJ FILMJÉT

";
        const string Malformed_NoSubText_SrtSample = @"1
00:00:55,848 --> 00:00:58,348

";

        class ReadSubtitlesFromFileTests
        {
            [Test]
            public void Invalid_path_should_throw_IOException()
            {
                Assert.Throws(Is.InstanceOf<IOException>(), () => new SrtFileReader("totallyinvalidpath.srt", Encoding.UTF8));
            }

            [Test]
            public void Should_read_every_subtitle_from_the_file()
            {
                var sut = new SrtFileReader(new StringReader(SrtSample));
                var subtitles = sut.ReadSubtitlesFromFile();
                Assert.That(subtitles.Count, Is.EqualTo(4));
            }

            [Test]
            public void Malformed_subtitle_files_should_throw_FileFormatException()
            {
                var sut_noIndex = new SrtFileReader(new StringReader(Malformed_NoIndex_SrtSample));
                Assert.Throws<FileFormatException>(() => sut_noIndex.ReadSubtitlesFromFile());

                var sut_noTimestamp = new SrtFileReader(new StringReader(Malformed_NoIndex_SrtSample));
                Assert.Throws<FileFormatException>(() => sut_noTimestamp.ReadSubtitlesFromFile());

                var sut_noSubtext = new SrtFileReader(new StringReader(Malformed_NoIndex_SrtSample));
                Assert.Throws<FileFormatException>(() => sut_noSubtext.ReadSubtitlesFromFile());
            }
        }
    }
}