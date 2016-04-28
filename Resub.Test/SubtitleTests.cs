using NUnit.Framework;
// ReSharper disable ExceptionNotDocumented

namespace Resub.Test
{
    public class SubtitleTests
    {
        const string Valid_Subtitle = @"1
00:01:07,693 --> 00:01:10,113
EGY DÖGÖS CSAJSZI
EGY DÖGÖS CSAJSZI2
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

        [Test]
        public void Valid_subtitle_should_set_the_properties()
        {
            var sut = new Subtitle(Valid_Subtitle);
            Assert.That(sut.Index, Is.EqualTo(1));
            Assert.That(sut.StartTime, Is.EqualTo(new Timestamp("00:01:07,693")));
            Assert.That(sut.EndTime, Is.EqualTo(new Timestamp("00:01:10,113")));
            Assert.That(sut.SubtitleLines.Count, Is.EqualTo(2));
        }

        [Test]
        public void Invalid_index_should_throw_FileFormatException()
        {
            Assert.Throws<FileFormatException>(() => new Subtitle(Malformed_NoIndex_SrtSample));
        }

        [Test]
        public void Invalid_timestamps_should_throw_FileFormatException()
        {
            Assert.Throws<FileFormatException>(() => new Subtitle(Malformed_NoIndex_SrtSample));
        }

        [Test]
        public void Empty_subtitle_text_should_throw_FileFormatException()
        {
            Assert.Throws<FileFormatException>(() => new Subtitle(Malformed_NoIndex_SrtSample));
        }

        [Test]
        public void ToString_should_return_a_well_formed_subtitle_string()
        {
            var sut = new Subtitle(Valid_Subtitle);
            Assert.That(sut.ToString(), Is.EqualTo(Valid_Subtitle));
        }
    }
}