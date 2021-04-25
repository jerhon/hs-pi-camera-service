using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MMALSharp;
using MMALSharp.Common.Utility;

namespace Honlsoft.Pi.CameraService.Camera
{
    public class MmalInitialization: IHostedService
    {
        private IOptions<CameraOptions> _options;
        
        public MmalInitialization(IOptions<CameraOptions> options)
        {
            _options = options;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_options.Value.Width != 0 && _options.Value.Height != 0)
            {
                MMALCameraConfig.StillResolution = new Resolution(_options.Value.Width, _options.Value.Height);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Have the application cleanup the camera.
            MMALCamera.Instance.Cleanup();

            return Task.CompletedTask;
        }
    }
}