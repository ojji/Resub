using System;
using NUnit.Framework;
// ReSharper disable ExceptionNotDocumented

namespace Resub.Test
{
    [TestFixture]
    public class TimestampTests
    {
        class ConstructionTests
        {
            [Test]
            public void Invalid_timestamp_should_throw_InvalidTimestampException()
            {
                // ReSharper disable ObjectCreationAsStatement
                Assert.Throws<InvalidTimestampException>(() => new Timestamp(""));
                Assert.Throws<InvalidTimestampException>(() => new Timestamp("1:1:2,0"));
                Assert.Throws<InvalidTimestampException>(() => new Timestamp("00:01:02"));
                Assert.Throws<InvalidTimestampException>(() => new Timestamp("00:01:02,"));
                // ReSharper restore ObjectCreationAsStatement
            }
        }

        class Equality
        {
            [Test]
            public void The_timestamp_should_be_equal_with_itself()
            {
                var sut = new Timestamp("00:00:00,001");
                Assert.That(sut.Equals(sut), "sut.Equals(sut)");
                // ReSharper disable EqualExpressionComparison
                #pragma warning disable CS1718 // Comparison made to same variable
                Assert.That(sut == sut, "sut == sut");
                #pragma warning restore CS1718 // Comparison made to same variable
                // ReSharper restore EqualExpressionComparison
            }

            [Test]
            public void Timestamp_should_not_equal_with_null()
            {
                var sut = new Timestamp("00:00:00,001");
                Assert.That(sut.Equals(null), Is.False);
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                Assert.That(sut == null, Is.False);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            }

            [Test]
            public void Two_timestamps_with_the_same_value_should_be_equal()
            {
                var first = new Timestamp("00:00:00,001");
                var second = new Timestamp("00:00:00,001");
                Assert.That(first.Equals(second), "first.Equals(second)");
                Assert.That(first == second, "first == second");
            }

            [Test]
            public void Equality_must_be_commutative()
            {
                var first = new Timestamp("00:00:00,001");
                var second = new Timestamp("00:00:00,001");
                Assert.That(first.Equals(second));
                Assert.That(second.Equals(first));

                Assert.That(first == second);
                Assert.That(second == first);
            }
        }
        

        [Test]
        public void Calling_ToString_on_a_valid_timestamp_should_return_the_same_string_value()
        {
            string validTimestamp = "00:00:00,000";
            Timestamp sut = new Timestamp(validTimestamp);
            Assert.That(sut.ToString(), Is.EqualTo(validTimestamp));
        }

        class PlusHoursTests
        {
            [Test]
            public void Adding_zero_hours_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("05:00:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusHours(0));
            }

            [Test]
            public void Adding_negative_hours_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("05:00:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusHours(-1));
            }

            [Test]
            public void Adding_too_many_hours_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.PlusHours(100));
            }

            [Test]
            public void Adding_hours_should_return_a_valid_timestamp_with_hours_added()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.That(sut.PlusHours(1), Is.EqualTo(new Timestamp("01:00:00,000")));
                Assert.That(sut.PlusHours(10), Is.EqualTo(new Timestamp("10:00:00,000")));
            }
        }

        class MinusHoursTests
        {
            [Test]
            public void Subtracting_zero_hours_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("05:00:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusHours(0));
            }

            [Test]
            public void Subtracting_negative_hours_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("05:00:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusHours(-1));
            }

            [Test]
            public void Subtracting_too_many_hours_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.MinusHours(1));
            }

            [Test]
            public void Subtracting_valid_hours_should_return_new_valid_timestamp()
            {
                Timestamp sut = new Timestamp("01:00:00,000");
                Assert.That(sut.MinusHours(1), Is.EqualTo(new Timestamp("00:00:00,000")));
            }
        }

        class PlusMinutesTest
        {
            [Test]
            public void Adding_zero_minutes_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusMinutes(0));
            }

            [Test]
            public void Adding_negative_minutes_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusMinutes(-1));
            }

            [Test]
            public void Adding_too_many_minutes_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("99:59:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.PlusMinutes(1));
            }

            [Test]
            public void Adding_minutes_should_return_a_valid_timestamp_with_minutes_added()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.That(sut.PlusMinutes(1), Is.EqualTo(new Timestamp("00:01:00,000")));
                Assert.That(sut.PlusMinutes(60), Is.EqualTo(new Timestamp("01:00:00,000")));
            }

            [Test]
            public void Adding_minutes_should_handle_minute_overflow()
            {
                Timestamp sut = new Timestamp("00:50:00,000");
                Assert.That(sut.PlusMinutes(15), Is.EqualTo(new Timestamp("01:05:00,000")));
            }
        }

        class MinusMinutesTests
        {
            [Test]
            public void Subtracting_zero_minutes_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusMinutes(0));
            }

            [Test]
            public void Subtracting_negative_minutes_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusMinutes(-1));
            }

            [Test]
            public void Subtracting_too_many_minutes_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.MinusMinutes(1));
            }

            [Test]
            public void Subtracting_valid_minutes_should_return_new_valid_timestamp()
            {
                Timestamp sut = new Timestamp("01:01:00,000");
                Assert.That(sut.MinusMinutes(1), Is.EqualTo(new Timestamp("01:00:00,000")));
                Assert.That(sut.MinusMinutes(60), Is.EqualTo(new Timestamp("00:01:00,000")));
            }

            [Test]
            public void Subtracting_minutes_should_handle_minute_underflow()
            {
                Timestamp sut = new Timestamp("01:00:00,000");
                Assert.That(sut.MinusMinutes(15), Is.EqualTo(new Timestamp("00:45:00,000")));
            }
        }

        class PlusSecondsTests
        {
            [Test]
            public void Adding_zero_seconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusSeconds(0));
            }

            [Test]
            public void Adding_negative_seconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusSeconds(-1));
            }

            [Test]
            public void Adding_too_many_seconds_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("99:59:59,000");
                Assert.Throws<InvalidTimestampException>(() => sut.PlusSeconds(1));
            }

            [Test]
            public void Adding_seconds_should_return_a_valid_timestamp_with_seconds_added()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.That(sut.PlusSeconds(1), Is.EqualTo(new Timestamp("00:00:01,000")));
                Assert.That(sut.PlusSeconds(60), Is.EqualTo(new Timestamp("00:01:00,000")));
            }

            [Test]
            public void Adding_seconds_should_handle_overflow()
            {
                Timestamp sut = new Timestamp("00:00:59,000");
                Assert.That(sut.PlusSeconds(1), Is.EqualTo(new Timestamp("00:01:00,000")));

                Timestamp sut2 = new Timestamp("00:59:59,000");
                Assert.That(sut2.PlusSeconds(1), Is.EqualTo(new Timestamp("01:00:00,000")));
            }
        }

        class MinusSecondsTests
        {
            [Test]
            public void Subtracting_zero_seconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusSeconds(0));
            }

            [Test]
            public void Subtracting_negative_seconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusSeconds(-1));
            }

            [Test]
            public void Subtracting_too_many_seconds_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.MinusSeconds(1));
            }

            [Test]
            public void Subtracting_seconds_should_return_a_valid_timestamp_with_seconds_subtracted()
            {
                Timestamp sut = new Timestamp("00:05:01,000");
                Assert.That(sut.MinusSeconds(1), Is.EqualTo(new Timestamp("00:05:00,000")));
                Assert.That(sut.MinusSeconds(60), Is.EqualTo(new Timestamp("00:04:01,000")));
            }

            [Test]
            public void Subtracting_seconds_should_handle_underflow()
            {
                Timestamp sut = new Timestamp("00:01:00,000");
                Assert.That(sut.MinusSeconds(1), Is.EqualTo(new Timestamp("00:00:59,000")));

                Timestamp sut2 = new Timestamp("01:00:00,000");
                Assert.That(sut2.MinusSeconds(1), Is.EqualTo(new Timestamp("00:59:59,000")));
            }
        }

        class PlusMillisecondsTests
        {
            [Test]
            public void Adding_zero_milliseconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusMilliseconds(0));
            }

            [Test]
            public void Adding_negative_milliseconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.PlusMilliseconds(-1));
            }

            [Test]
            public void Adding_too_many_seconds_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("99:59:59,999");
                Assert.Throws<InvalidTimestampException>(() => sut.PlusMilliseconds(1));
                Assert.Throws<InvalidTimestampException>(() => sut.PlusMilliseconds(Int32.MaxValue));
            }

            [Test]
            public void Adding_milliseconds_should_return_a_valid_timestamp_with_milliseconds_added()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.That(sut.PlusMilliseconds(1), Is.EqualTo(new Timestamp("00:00:00,001")));
                Assert.That(sut.PlusMilliseconds(1000), Is.EqualTo(new Timestamp("00:00:01,000")));
            }

            [Test]
            public void Adding_milliseconds_should_handle_overflow()
            {
                Timestamp sut = new Timestamp("00:00:00,999");
                Assert.That(sut.PlusMilliseconds(1), Is.EqualTo(new Timestamp("00:00:01,000")));

                Timestamp sut2 = new Timestamp("00:59:59,999");
                Assert.That(sut2.PlusMilliseconds(1), Is.EqualTo(new Timestamp("01:00:00,000")));
            }
        }

        class MinusMillisecondsTests
        {
            [Test]
            public void Subtracted_zero_milliseconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusMilliseconds(0));
            }

            [Test]
            public void Subtracted_negative_milliseconds_should_throw_ArgumentOutOfRangeException()
            {
                Timestamp sut = new Timestamp("00:05:00,000");
                Assert.Throws<ArgumentOutOfRangeException>(() => sut.MinusMilliseconds(-1));
            }

            [Test]
            public void Subtracted_too_many_seconds_should_throw_InvalidTimestampException()
            {
                Timestamp sut = new Timestamp("00:00:00,000");
                Assert.Throws<InvalidTimestampException>(() => sut.MinusMilliseconds(1));
            }

            [Test]
            public void Subtracted_milliseconds_should_return_a_valid_timestamp_with_milliseconds_subtracted()
            {
                Timestamp sut = new Timestamp("00:00:01,000");
                Assert.That(sut.MinusMilliseconds(1), Is.EqualTo(new Timestamp("00:00:00,999")));
                Assert.That(sut.MinusMilliseconds(1000), Is.EqualTo(new Timestamp("00:00:00,000")));
            }

            [Test]
            public void Subtracted_milliseconds_should_handle_underflow()
            {
                Timestamp sut = new Timestamp("00:00:01,000");
                Assert.That(sut.MinusMilliseconds(1), Is.EqualTo(new Timestamp("00:00:00,999")));

                Timestamp sut2 = new Timestamp("99:00:00,000");
                Assert.That(sut2.MinusMilliseconds(1), Is.EqualTo(new Timestamp("98:59:59,999")));
            }
        }
    }
}
