﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Honlsoft.Pi.CameraService.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Honlsoft.Pi.CameraService
{
    /// <summary>
    /// Polls the camera, taking image captures and saving them in the image cache.
    /// </summary>
    public class CameraPollingService : IHostedService
    {
        // TODO: Add some error handling in here.  Additionally, should just continuously poll and get images from the camera on a dedicated Task/Thread rather than using a timer.

        private IOptions<CameraOptions> _options;
        private readonly CameraCapture _capture;
        private readonly CameraImageCache _cache;
        private readonly CancellationTokenSource _cancelTokenSource;
        private ILogger _logger;
        private Task _pollingTask;
        
        public CameraPollingService(IOptions<CameraOptions> options, CameraCapture capture, CameraImageCache imageCache, ILogger<CameraPollingService> logger)
        {
            _options = options;
            _capture = capture;
            _logger = logger;
            _cache = imageCache;
            _cancelTokenSource = new CancellationTokenSource();
            _options = options;
            _logger = logger;
        }

        private async Task ProcessLoopAsync()
        {
            while (!_cancelTokenSource.IsCancellationRequested)
            {
                try
                {
                    CancellationToken token = _cancelTokenSource.Token;
                    if (_options.Value.Timeout > 0)
                    {
                        CancellationTokenSource source = new CancellationTokenSource();
                        source.CancelAfter(_options.Value.Timeout);
                        token = CancellationTokenSource.CreateLinkedTokenSource(source.Token, _cancelTokenSource.Token).Token;
                    }

                    _logger.LogDebug("Taking picture.");
                    Stopwatch sw = Stopwatch.StartNew();
                    _cache.LatestImage = await _capture.GetImageAsync(token);
                    sw.Stop();
                    _logger.LogDebug("Picture taken in {time}ms.", sw.ElapsedMilliseconds);

                    if (_options.Value.Interval > 0)
                    {
                        await Task.Delay(_options.Value.Interval, _cancelTokenSource.Token);    
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError( ex, "Could not capture image from camera.");
                    await Task.Delay(_options.Value.FailureDelay);
                }
            }
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _pollingTask = ProcessLoopAsync();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancelTokenSource.Cancel();
            return _pollingTask;
        }
    }
}