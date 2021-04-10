using System;

namespace Honlsoft.Pi.CameraService.Camera
{
      
    public record ImageCapture(byte[] data, DateTime time);
    
    /// <summary>
    /// Options for the 
    /// </summary>
    public class CameraOptions
    {
        
        public int Width { get; set; }
        public int Height { get; set; }
        public int Interval { get; set; }
        
        public int Timeout { get; set; }

        public int FailureDelay { get; set; } = 5000; // It the image fails to save, wait 5 seconds by default and try again.
    }
}