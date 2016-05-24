using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Resub
{
    public class SrtFileReader : IDisposable
    {
        public SrtFileReader(string path, Encoding encoding, bool detectEncodingFromBom = true) : this(new StreamReader(path, encoding, detectEncodingFromBom)) 
        {
        }

        // internal constructor for testing purposes
        internal SrtFileReader(TextReader internalReader)
        {
            _internalReader = internalReader;
        }

        public List<Subtitle> ReadSubtitlesFromFile()
        {
            var subtitles = new List<Subtitle>();

            string line;
            StringBuilder subtitleText = new StringBuilder();
            while ((line = _internalReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // handle multiple empty lines gracefully
                    if (subtitleText.Length == 0) { continue; }

                    Subtitle subtitle = new Subtitle(subtitleText.ToString());
                    subtitles.Add(subtitle);
                    subtitleText.Clear();
                    continue;
                }
                subtitleText.AppendLine(line);
            }

            return subtitles;
        }

        private readonly TextReader _internalReader;

        #region Dispose pattern
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _internalReader?.Dispose();
            }
            _disposed = true;
        }

        private bool _disposed;

        #endregion
    }
}