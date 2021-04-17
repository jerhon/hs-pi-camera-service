namespace Honlsoft.Pi.CameraService.Camera
{
    public class ServoOptions
    {
        
        /// <summary>
        /// The minimum pulse width in seconds.  The specifications of the servo should mention this.
        /// </summary>
        public double MinPulseWidth { get; set; }
        /// <summary>
        /// The maximum pulse width in seconds.  The specifications of the servo should mention this.
        /// </summary>
        public double MaxPulseWidth { get; set; }
        /// <summary>
        /// The minimum range a user can set.
        /// </summary>
        public int MinUserRange { get; set; }
        /// <summary>
        /// The maximum range a user can set.
        /// </summary>
        public int MaxUserRange { get; set; }
        public int Frequency { get; set; }
        
        
        /// <summary>
        /// Minimum value allowed to pan.
        /// </summary>
        public int PanMin { get; set; }
        
        /// <summary>
        /// Maximum value allowed to pan.
        /// </summary>
        public int PanMax { get; set; }
        
        /// <summary>
        /// Minimum value allowed to tilt.
        /// </summary>
        public int TiltMin { get; set; }
        
        /// <summary>
        /// Maximum value allowed to tilt.
        /// </summary>
        public int TiltMax { get; set; }
    }
}