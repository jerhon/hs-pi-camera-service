using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Honlsoft.Pi.CameraService.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Timer = System.Timers.Timer;

namespace Honlsoft.Pi.CameraService
{

    public record ImageCapture(byte[] data, DateTime time);

    /// <summary>
    /// Polls the camera, taking image captures and saving them in the image cache.
    /// </summary>
    public class CameraPollingService : IHostedService
    {
        // TODO: Add some error handling in here.  Additionally, should just continuously poll and get images from the camera on a dedicated Task/Thread rather than using a timer.
        
        private readonly CameraCapture _capture;
        private readonly IOptions<CameraOptions> _options;
        private readonly Timer _timer;
        private readonly CameraImageCache _cache;
        private ILogger _logger;
        
        public CameraPollingService(IOptions<CameraOptions> options, CameraCapture capture, CameraImageCache imageCache, ILogger<CameraPollingService> logger)
        {
            _options = options;
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromMilliseconds(_options.Value.Interval).TotalMilliseconds;
            _timer.AutoReset = true;
            _timer.Elapsed += TimerOnElapsed;
            _capture = capture;
            _logger = logger;
            _cache = imageCache;
        }

        private async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _logger.LogDebug("Timer elapsed.");
            _cache.LatestImage = await _capture.GetImageAsync();
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cache.LatestImage = await _capture.GetImageAsync();
            _timer.Enabled = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Enabled = false;
            return Task.CompletedTask;
        }
    }
}