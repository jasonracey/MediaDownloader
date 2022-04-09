using System;
using System.Runtime.Serialization;

namespace MediaDownloaderLib
{
    [Serializable]
    public class DownloaderException : Exception
    {
        public DownloaderException()
        {
        }

        public DownloaderException(string message) : base(message)
        {
        }

        public DownloaderException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DownloaderException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}