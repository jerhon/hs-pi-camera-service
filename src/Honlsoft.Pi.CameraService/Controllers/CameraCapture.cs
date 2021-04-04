using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Unosquare.RaspberryIO.Camera;

namespace Honlsoft.Pi.CameraService.Controllers
{
    /// <summary>
    /// Captures images from the raspberry Pi.
    /// </summary>
    public class CameraCapture
    {
        private readonly IOptions<CameraOptions> _options;

        public CameraCapture(IOptions<CameraOptions> options)
        {
            _options = options;
        }
        
        public async Task<ImageCapture> GetImageAsync()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(_options.Value.Timeout);
            var image = await Unosquare.RaspberryIO.Pi.Camera.CaptureImageAsync(new CameraStillSettings(){ CaptureWidth  = _options.Value.Width, CaptureHeight = _options.Value.Height }, tokenSource.Token);
            return new ImageCapture(image, DateTime.Now);
        }
    }
}