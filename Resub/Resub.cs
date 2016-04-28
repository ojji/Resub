using System;
using System.Collections.Generic;

namespace Resub
{
    public class Resub
    {
        private readonly ResubOptions _options;
        private List<Subtitle> _subtitles;

        /// <exception cref="ArgumentException">Invalid Resub options.</exception>
        public Resub(ResubOptions options)
        {
            _options = options;
            if (options == null || !options.IsValid)
            {
                throw new ArgumentException("Resub options must be valid.");
            }
        }

        public void ReadjustSubtitles()
        {
            using (var reader = new SrtFileReader(_options.InputPath))
            {
                _subtitles = reader.ReadSubtitlesFromFile();
            }

            foreach (var subtitle in _subtitles)
            {
                Timestamp newStart, newEnd;

                if (_options.Offset > 0)
                {
                    newStart = subtitle.StartTime.PlusMilliseconds(_options.Offset);
                    newEnd = subtitle.EndTime.PlusMilliseconds(_options.Offset);
                }
                else
                {
                    newStart = subtitle.StartTime.MinusMilliseconds(_options.Offset);
                    newEnd = subtitle.EndTime.MinusMilliseconds(_options.Offset);
                }

                subtitle.StartTime = newStart;
                subtitle.EndTime = newEnd;
            }

            using (var writer = new SrtFileWriter(_options.OutputPath))
            {
                writer.WriteSubtitlesToFile(_subtitles);
            }
        }
    }
}