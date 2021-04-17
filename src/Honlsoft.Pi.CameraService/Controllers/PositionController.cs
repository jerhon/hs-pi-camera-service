using System;
using System.Threading;
using Honlsoft.Pi.CameraService.Camera;
using Honlsoft.Pi.Hardware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Honlsoft.Pi.CameraService.Controllers
{
    public record PositionDto(int PanAngle, int TiltAngle);
    
    [ApiController]
    [Route("position")]
    public class PositionController : ControllerBase
    {
        private Servo _servo;
        private IOptions<ServoOptions> _options;
        
        public PositionController(Servo servo, IOptions<ServoOptions> options)
        {
            _servo = servo;
            _options = options;
        }

        private int GetValidAngle(int userProvided, int min, int max)
        {
            return Math.Min(Math.Max(userProvided, min), max);
        }
        
        [HttpPost]
        public void Position([FromBody] PositionDto position)
        {
            _servo.Initialize(_options.Value.Frequency,_options.Value.MinUserRange, _options.Value.MaxUserRange, _options.Value.MinPulseWidth, _options.Value.MaxPulseWidth);

            var panAngle = GetValidAngle(position.PanAngle, _options.Value.PanMin, _options.Value.PanMax);
            var tiltAngle = GetValidAngle(position.TiltAngle, _options.Value.TiltMin, _options.Value.TiltMax);
            
            _servo.SetAngle(0, tiltAngle);
            _servo.SetAngle(1, panAngle);
        }

        [HttpGet]
        public PositionDto PositionDto()
        {
            var pan = _servo.GetAngle(0);
            var tilt = _servo.GetAngle(1);
            return new PositionDto(pan, tilt);
        }
    }
}