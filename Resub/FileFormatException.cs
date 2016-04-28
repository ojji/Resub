using System;
using System.Runtime.Serialization;

namespace Resub
{
    [Serializable]
    public class FileFormatException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public FileFormatException()
        {
        }

        public FileFormatException(string message) : base(message)
        {
        }

        public FileFormatException(string message, Exception inner) : base(message, inner)
        {
        }

        protected FileFormatException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}