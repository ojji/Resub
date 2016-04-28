using System;
using System.Text.RegularExpressions;

namespace Resub
{
    public class Timestamp : IEquatable<Timestamp>
    {
        private const int MinTimestampValue = 0;
        /// Max valid value:
        /// 99:59:59,999 = 999 + 59*1000 + 59*60*1000 + 99*60*60*1000 = 359'999'999 ms
        private const int MaxTimestampValue = 359999999;
        public static Regex TimeStampRegex = new Regex(@"^(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2}),(?<millisecond>\d{3})$", RegexOptions.Compiled);

        private readonly int _timestamp;

        /// <exception cref="InvalidTimestampException">Timestamp format is invalid.</exception>
        public Timestamp(string timeStampString)
        {
            var match = TimeStampRegex.Match(timeStampString);
            if (!match.Success)
            {
                throw new InvalidTimestampException($"Timestamp is invalid: '{timeStampString}'");
            }

            int hours = Int32.Parse(match.Groups["hour"].Value);
            int minutes = Int32.Parse(match.Groups["minute"].Value);
            int seconds = Int32.Parse(match.Groups["second"].Value);
            int milliSeconds = Int32.Parse(match.Groups["millisecond"].Value);

            _timestamp = milliSeconds + 1000*seconds + 1000*60*minutes + 1000*60*60*hours;

            if (!IsValidTime())
            {
                throw new InvalidTimestampException($"Timestamp is invalid: '{timeStampString}'");
            }
        }

        /// <exception cref="InvalidTimestampException">This is not a valid timestamp.</exception>
        public Timestamp(int hours, int minutes, int seconds, int milliSeconds)
        {
            try
            {
                _timestamp = checked(milliSeconds + 1000 * seconds + 1000 * 60 * minutes + 1000 * 60 * 60 * hours);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("This is not a valid timestamp.");
            }
            if (!IsValidTime())
            {
                throw new InvalidTimestampException("This is not a valid timestamp.");
            }
        }

        private Timestamp(int timestamp)
        {
            _timestamp = timestamp;
            if (!IsValidTime())
            {
                throw new InvalidTimestampException("This is not a valid timestamp.");
            }
        }

        public override string ToString()
        {
            int milliSeconds = _timestamp%1000;
            int seconds = (_timestamp/1000)%60;
            int minutes = (_timestamp/1000/60)%60;
            int hours = _timestamp/1000/60/60;
            return $"{hours:D2}:{minutes:D2}:{seconds:D2},{milliSeconds:D3}";
        }

        /// <exception cref="ArgumentOutOfRangeException">Add parameter is either negative or zero</exception>
        /// <exception cref="InvalidTimestampException">Add results an invalid timestamp.</exception>
        public Timestamp PlusHours(int hoursAdded)
        {
            if (hoursAdded < 1) { throw new ArgumentOutOfRangeException(nameof(hoursAdded), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp + hoursAdded * 60 * 60 * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }

            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Subtraction parameter is either negative or zero.</exception>
        /// <exception cref="InvalidTimestampException">Subtracting results an invalid timestamp.</exception>
        public Timestamp MinusHours(int hoursSubtracted)
        {
            if (hoursSubtracted < 1) { throw new ArgumentOutOfRangeException(nameof(hoursSubtracted), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp - hoursSubtracted * 60 * 60 * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Add parameter is either negative or zero.</exception>
        /// <exception cref="InvalidTimestampException">Add results an invalid timestamp.</exception>
        public Timestamp PlusMinutes(int minutesAdded)
        {
            if (minutesAdded < 1) { throw new ArgumentOutOfRangeException(nameof(minutesAdded), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp + minutesAdded * 60 * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Subtraction parameter is either negative or zero.</exception>
        /// <exception cref="InvalidTimestampException">Subtracting results an invalid timestamp.</exception>
        public Timestamp MinusMinutes(int minutesSubtracted)
        {
            if (minutesSubtracted < 1) { throw new ArgumentOutOfRangeException(nameof(minutesSubtracted), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp - minutesSubtracted * 60 * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Add parameter is either negative or zero.</exception>
        /// <exception cref="InvalidTimestampException">Add results an invalid timestamp.</exception>
        public Timestamp PlusSeconds(int secondsAdded)
        {
            if (secondsAdded < 1) { throw new ArgumentOutOfRangeException(nameof(secondsAdded), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp + secondsAdded * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Subtract parameter is either negative or zero.</exception>
        public Timestamp MinusSeconds(int secondsSubtracted)
        {
            if (secondsSubtracted < 1) { throw new ArgumentOutOfRangeException(nameof(secondsSubtracted), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp - secondsSubtracted * 1000);
            }
            catch (OverflowException)
            {
                throw new InvalidTimestampException("Timestamp is invalid.");
            }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Add parameter is either zero or negative.</exception>
        /// <exception cref="InvalidTimestampException">Add results an invalid timestamp.</exception>
        public Timestamp PlusMilliseconds(int millisecondsAdded)
        {
            if (millisecondsAdded < 1) { throw new ArgumentOutOfRangeException(nameof(millisecondsAdded), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp + millisecondsAdded);
            }
            catch (OverflowException) { throw new InvalidTimestampException("Timestamp is invalid."); }
            return new Timestamp(newTimestamp);
        }

        /// <exception cref="ArgumentOutOfRangeException">Subtraction parameter is either zero or negative.</exception>
        /// <exception cref="InvalidTimestampException">Subtraction results an invalid timestamp.</exception>
        public Timestamp MinusMilliseconds(int millisecondsSubtracted)
        {
            if (millisecondsSubtracted < 1) { throw new ArgumentOutOfRangeException(nameof(millisecondsSubtracted), "Parameter value must be positive."); }
            int newTimestamp;
            try
            {
                newTimestamp = checked(_timestamp - millisecondsSubtracted);
            }
            catch (OverflowException) { throw new InvalidTimestampException("Timestamp is invalid."); }
            return new Timestamp(newTimestamp);
        }

        #region Equality

        public bool Equals(Timestamp other)
        {
            if ((object)other == null)
            {
                return false;
            }
            
            return _timestamp == other._timestamp;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Timestamp);
        }

        public override int GetHashCode()
        {
            return _timestamp;
        }

        public static bool operator ==(Timestamp lhs, Timestamp rhs)
        {
            if (ReferenceEquals(lhs, rhs))
            {
                return true;
            }

            if ((object)lhs == null || (object)rhs == null)
            {
                return false;
            }

            return lhs._timestamp == rhs._timestamp;
        }

        public static bool operator !=(Timestamp lhs, Timestamp rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        private bool IsValidTime()
        {
            return (_timestamp >= MinTimestampValue && _timestamp <= MaxTimestampValue);
        }
    }
}