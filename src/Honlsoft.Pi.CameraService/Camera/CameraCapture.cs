using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Unosquare.RaspberryIO.Camera;

namespace Honlsoft.Pi.CameraService.Camera
{
    /// <summary>
    /// Captures images from a Raspberry Pi.
    /// </summary>
    public class CameraCapture
    {
        private readonly IOptions<CameraOptions> _options;

        public CameraCapture(IOptions<CameraOptions> options)
        {
            _options = options;
        }
        
        /// <summary>
        /// Takes a single image capture.
        /// </summary>
        /// <param name="cancellationToken">Passed to the image capture to cancel the camera from taking the photo.</param>
        /// <returns>An image capture which contains the JPG, and date the image was taken.</returns>
        public async Task<ImageCapture> GetImageAsync(CancellationToken cancellationToken)
        {
            var image = await Unosquare.RaspberryIO.Pi.Camera.CaptureImageAsync(new CameraStillSettings(){ CaptureWidth  = _options.Value.Width, CaptureHeight = _options.Value.Height, }, cancellationToken);
            return new ImageCapture(image, DateTime.Now);
        }
    }
}