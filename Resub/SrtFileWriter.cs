using System;
using System.Collections.Generic;
using System.IO;

namespace Resub
{
    public class SrtFileWriter : IDisposable
    {
        public SrtFileWriter(string path)
        {
            _internalWriter = new StreamWriter(path, false);
        }
        // for testing purposes
        internal SrtFileWriter(TextWriter internalWriter)
        {
            _internalWriter = internalWriter;
        }

        public void WriteSubtitlesToFile(IEnumerable<Subtitle> subtitles)
        {
            foreach (var subtitle in subtitles)
            {
                _internalWriter.WriteLine(subtitle.ToString());
            }
            _internalWriter.Flush();
        }

        private readonly TextWriter _internalWriter;

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
                _internalWriter.Dispose();
            }
            _disposed = true;
        }

        private bool _disposed;
        #endregion
    }
}