using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resub
{
    public class Subtitle
    {
        public Timestamp StartTime { get; set; }
        public Timestamp EndTime { get; set; }
        public int Index { get; private set; }
        public List<string> SubtitleLines { get; }

        public Subtitle(string content)
        {
            SubtitleLines = new List<string>();
            string line;
            using (var sr = new StringReader(content))
            {
                ReadIndex(sr.ReadLine());
                ReadTimestamps(sr.ReadLine());
                while ((line = sr.ReadLine()) != null)
                {
                    SubtitleLines.Add(line);
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\r\n", Index);
            sb.AppendFormat("{0} --> {1}\r\n", StartTime, EndTime);
            foreach (var subtitleLine in SubtitleLines)
            {
                sb.AppendLine(subtitleLine);
            }

            return sb.ToString();
        }

        private void ReadTimestamps(string timestampsLines)
        {
            string[] segments = timestampsLines.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length != 3) { throw new FileFormatException("Invalid subtitle format."); }
            try
            {
                StartTime = new Timestamp(segments[0]);
                EndTime = new Timestamp(segments[2]);
            }
            catch (InvalidTimestampException ex)
            {
                throw new FileFormatException("Invalid subtitle format.", ex);
            }
        }

        private void ReadIndex(string indexLine)
        {
            int index;
            if (!Int32.TryParse(indexLine, out index))
            {
                throw new FileFormatException("Invalid subtitle format.");
            }

            Index = index;
        }
    }
}