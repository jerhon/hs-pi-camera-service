using System;

namespace Honlsoft.Pi.Hardware
{
    /// <summary>
    /// Controls servos attached to a Pca9685 board.
    /// </summary>
    public class Servo
    {
        private Pca9685 _pca;

        private int _outputFrequency;
        
        private int _minUserRange;
        private int _maxUserRange;

        private int _minPulseWidth;
        private int _maxPulseWidth;

        public Servo(Pca9685 pca)
        {
            _pca = pca;
        }

        /// <summary>
        /// Initialize the underlying Pca9685 with parameters for the servo.
        /// </summary>
        /// <param name="frequency">The output frequency for the servos in Hz, usually 50Hz.</param>
        /// <param name="minRange">The minimum user range allowable for adjusting the servo position</param>
        /// <param name="maxRange">The maximum user range allowable for adjusting the servo position</param>
        /// <param name="minPulseWidth">The minimum pulse width allowed by the servo in Seconds, typically passed in like 0.0005</param>
        /// <param name="maxPulseWidth">The maximum pulse width allowed by the servo in Seconds, typically passed in like 0.0025</param>
        public void Initialize(int frequency, int minRange, int maxRange, double minPulseWidth, double maxPulseWidth)
        {
            _minPulseWidth = (int) (minPulseWidth * _pca.Precision * frequency);
            _maxPulseWidth = (int) (maxPulseWidth * _pca.Precision * frequency);
            _outputFrequency = frequency;
            _minUserRange = minRange;
            _maxUserRange = maxRange;
            _pca.Initialize(_outputFrequency);
        }

        /// <summary>
        /// Sets the angle of the Servo.
        /// </summary>
        /// <param name="idx">The index of the servo on the board.</param>
        /// <param name="userValue">The value to used (based on the maximum / minimum user value passed in initialization.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetAngle(int idx, int userValue)
        {
            if (userValue < _minUserRange || userValue > _maxUserRange)
            {
                throw new ArgumentOutOfRangeException(nameof(userValue), "");
            }
            
            var value = userValue * (_maxPulseWidth - _minPulseWidth) / (_maxUserRange - _minUserRange) + _minPulseWidth;
            _pca.ConfigurePulse(idx, value);
        }

        /// <summary>
        /// Gets the current configured angle from the board.
        /// </summary>
        /// <param name="idx">The index of the servo to lookup.</param>
        /// <returns>The user defined value.</returns>
        public int GetAngle(int idx)
        {
            var set = _pca.GetPulse(idx);
            return (set.Off - _minPulseWidth) * (_maxUserRange - _minUserRange) / (_maxPulseWidth - _minPulseWidth);
        }
    }
}