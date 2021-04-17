using System;
using System.Threading;
using Unosquare.RaspberryIO.Abstractions;

namespace Honlsoft.Pi.Hardware
{

    /// <summary>
    /// Indicates when to turn a signal on and off.
    /// </summary>
    public record PwmOnOffSettings(int On, int Off);
    
    /// <summary>
    /// This class implements logic for the PCA9685 board to operate servos on it.
    /// This is not a general purpose implementation to control the board (yet).
    /// </summary>
    public class Pca9685
    {
        public const byte MODE1_RESTART = 0x80;
        public const byte MODE1_EXTCLK = 0x40;
        public const byte MODE1_AI = 0x20;
        public const byte MODE1_SLEEP = 0x10;
        public const byte MODE2_OUTDRV = 0x04;

        private const byte MODE1_ADDR = 0x00;
        private const byte MODE2_ADDR = 0x01;
        private const byte Led0OnL = 0x06;
        private const byte Led0OnH = 0x07;
        private const byte Led0OffL = 0x08;
        private const byte Led0OffH = 0x09;
        private const byte PRESCALE_ADDR = 0xFE;
        private const float CLOCK_FREQ = 25000000.0f;

        private readonly II2CDevice _i2c;

        /// <summary>
        /// Constructs an instance that will communicate via the passed in II2CDevice.
        /// </summary>
        /// <param name="i2c">The I2C device to use.</param>
        public Pca9685(II2CDevice i2c)
        {
            _i2c = i2c;
        }

        /// <summary>
        /// Initializes the board and sets the period the pulse should repeat.
        /// </summary>
        /// <param name="frequency">The frequency to use in Hz</param>
        public void Initialize(int frequency)
        {
            Reset();
            SetPeriod(frequency);
        }


        /// <summary>
        /// Resets the board.
        /// </summary>
        private void Reset()
        {
            _i2c.WriteAddressByte(MODE1_ADDR, MODE1_RESTART);
            Thread.Sleep(10);
        }

        /// <summary>
        /// Sets the period the pulse should repeat.
        /// </summary>
        /// <param name="frequency">The frequency to use in Hz.</param>
        public void SetPeriod(int frequency)
        {
            double prescale = CLOCK_FREQ;
            prescale /= 4096; 
            prescale /= frequency;
            prescale -= 0.5;
            prescale = Math.Floor(prescale);
            var oldmode = _i2c.ReadAddressByte(MODE1_ADDR);
            var newmode = (byte)((oldmode & ~MODE1_RESTART) | MODE1_SLEEP);
            _i2c.WriteAddressByte(MODE1_ADDR, newmode); // Sleep
            _i2c.WriteAddressByte(PRESCALE_ADDR, (byte)prescale); // prescale
            _i2c.WriteAddressByte(MODE1_ADDR, oldmode); // wake
            
            Thread.Sleep(5);
            _i2c.WriteAddressByte(MODE1_ADDR, (byte)(oldmode | MODE1_RESTART ));
        }


        /// <summary>
        /// Sets the pulse width by determining when the pulse should go on and off.
        /// Valid values are limited to 12 bits, so 0-4095.
        /// If a value is presented outside of the range, the value will be set to be the maximum or minimum respectively.
        /// </summary>
        /// <param name="idx">The output on the board to use.</param>
        /// <param name="onValue">The time at which the pulse should turn on.</param>
        /// <param name="offValue">The time at which the pulse should turn off.</param>
        public void ConfigurePulse(int idx, int onValue, int offValue)
        {
            onValue = Math.Max(onValue, 0);
            offValue = Math.Min(offValue, 4095);
            
            int ledIdx = idx * 4;
            
            _i2c.WriteAddressByte(Led0OnL + ledIdx, (byte)(onValue & 0xFF));
            _i2c.WriteAddressByte(Led0OnH + ledIdx, (byte)(onValue >> 8));
            _i2c.WriteAddressByte(Led0OffL + ledIdx, (byte)(offValue & 0xFF));
            _i2c.WriteAddressByte(Led0OffH + ledIdx, (byte)(offValue >> 8));
        }

        /// <summary>
        /// Turns on the pulse at 0 and turns it off at the value provided. must be a value 0-4095
        /// </summary>
        /// <param name="idx">The index of the outputs on the board.</param>
        /// <param name="value">The value of when to turn off the pulse.</param>
        public void ConfigurePulse(int idx, int value)
        {
            ConfigurePulse(idx, 0, value);
        }

        /// <summary>
        /// Gets the settings for the pulse.
        /// </summary>
        /// <param name="idx">The idx of the output on the board.</param>
        /// <returns>The settings that were set on the board.</returns>
        public PwmOnOffSettings GetPulse(int idx)
        {
            int ledIdx = idx * 4;
            
            var low = _i2c.ReadAddressByte(Led0OnL + idx);
            var hi = _i2c.ReadAddressByte(Led0OnH + idx);
            var on = (hi << 8) | low;
            low = _i2c.ReadAddressByte(Led0OffL + idx);
            hi = _i2c.ReadAddressByte(Led0OffH + idx);
            var off = (hi << 8) | low;

            return new PwmOnOffSettings(on, off);
        }

        /// <summary>
        /// The precision for configuring the pulse width.
        /// </summary>
        public int Precision => 4096;
    }
}
