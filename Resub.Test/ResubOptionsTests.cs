using System.Text;
using NUnit.Framework;

namespace Resub.Test
{
    public class ResubOptionsTests
    {
        [Test]
        public void GetHelp_should_return_usage_string()
        {
            var sut = new ResubOptions(new string[]{});
            Assert.That(ResubOptions.GetHelp(), Is.Not.Empty);
        }

        [Test]
        public void No_input_encoding_set_should_fall_back_to_default_encoding()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.InputEncoding, Is.EqualTo(ResubOptions.DefaultInputEncoding));
        }

        [Test]
        public void Invalid_encoding_string_should_result_invalid_options()
        {
            var sut = new ResubOptions(new[] {
                "-ienc=invalidstring",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void Valid_encoding_string_should_set_the_input_encoding()
        {
            var sut = new ResubOptions(new[] {
                "-ienc=utf-8",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.InputEncoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void No_i_switch_should_result_invalid_options()
        {
            var sut = new ResubOptions(new[]
            {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-offset", "+1h1m5s200ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void No_inputfile_after_the_i_switch_should_result_invalid_options()
        {
            var sut = new ResubOptions(new[]
            {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void Inputpath_should_be_the_string_following_the_i_switch()
        {
            var sut = new ResubOptions(new[]
            {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m5s200ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.InputPath, Is.EqualTo("inputfilepath.srt"));
        }

        [Test]
        public void No_offset_switch_should_be_invalid()
        {
            var sut = new ResubOptions(new[]
            {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void No_offset_value_should_be_invalid()
        {
            var sut = new ResubOptions(new[]
            {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void Offset_should_follow_input_file()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-offset", "+1h1m5s200ms",
                "-i", "inputfilepath.srt",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.False);
        }
        
        [Test]
        [TestCase("+")]
        [TestCase("+1")]
        [TestCase("1")]
        [TestCase("-")]
        [TestCase("-1")]
        [TestCase("1hsomethingtotallywrong")]
        public void Offset_with_bad_value_should_be_invalid(string offsetString)
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", offsetString,
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        [TestCase("+0ms", 0)]
        [TestCase("1h", 1 * 60 * 60 * 1000)]
        [TestCase("+1h", 1 * 60 * 60 * 1000)]
        [TestCase("-1h", -1 * 60 * 60 * 1000)]
        [TestCase("+1m", 1 * 60 * 1000)]
        [TestCase("+1s", 1 * 1000)]
        [TestCase("+1h1m1s1ms", 1 * 60 * 60 * 1000 + 1 * 60 * 1000 + 1 * 1000 + 1)]
        [TestCase("+100ms", 100)]
        public void Offset_should_set_proper_value(string offsetString, int offsetValue)
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", offsetString,
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });

            Assert.That(sut.IsValid, Is.True);
            Assert.That(sut.Offset, Is.EqualTo(offsetValue));
        }

        [Test]
        public void No_output_switch_should_be_invalid()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void No_output_file_after_the_switch_should_be_invalid()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void Output_switch_should_follow_the_offset()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-o", "outputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value"
            });
            Assert.That(sut.IsValid, Is.False);
        }

        [Test]
        public void Outputpath_should_be_the_value_following_the_o_switch()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.OutputPath, Is.EqualTo("outputfilepath.srt"));
        }

        [Test]
        public void IsValid_should_be_true_if_every_parameter_is_supplied_in_order_and_valid()
        {
            var sut = new ResubOptions(new[] {
                "-iopt1", "iopt1value",
                "-iopt2", "iopt2value",
                "-i", "inputfilepath.srt",
                "-offset", "+1h1m1s1ms",
                "-oopt1", "oopt1value",
                "-oopt2", "oopt2value",
                "-o", "outputfilepath.srt"
            });
            Assert.That(sut.IsValid, Is.True);
            Assert.That(sut.InputPath, Is.EqualTo("inputfilepath.srt"), "Input path");
            Assert.That(sut.Offset, Is.EqualTo(1+1*1000+1*60*1000+1*60*60*1000), "Offset");
            Assert.That(sut.OutputPath, Is.EqualTo("outputfilepath.srt"), "Output path");
        }
    }
}