using System;
using System.Collections.Generic;

namespace MediaDownloaderLib
{
    public interface IProcessingStatusReporter
    {
        ProcessingStatus? ProcessingStatus { get; }
    }
    
    public class ProcessingStatus
    {
        public ProcessingStatus(
            int countCompleted,
            int countTotal,
            IEnumerable<Track> tracks,
            string? message = null)
        {
            if (countCompleted < 0)
                throw new ArgumentException("Must be >= 0");
            if (countTotal < 0)
                throw new ArgumentException("Must be >= 0");
            if (tracks == null)
                throw new ArgumentNullException(nameof(tracks));
			
            CountCompleted = countCompleted;
            CountTotal = countTotal;
            Tracks = tracks;
            Message = message ?? string.Empty;
        }
		
        public int CountCompleted { get; }
        
        public int CountTotal { get; }
        
        public IEnumerable<Track> Tracks { get; }
        
        public string Message { get; }
        
        public double PercentCompleted => CountTotal == 0.0D
            ? 0.0D
            : 100.0D * CountCompleted / CountTotal;
    }
}