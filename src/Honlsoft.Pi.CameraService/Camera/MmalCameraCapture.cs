using System;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;

namespace Honlsoft.Pi.CameraService.Camera
{
    /// <summary>
    /// Simple camera capture takes a snapshot on demand.
    /// </summary>
    public class MmalCameraCapture : ICameraCapture
    {
        public async Task<ImageCapture> GetImageAsync(CancellationToken cancellationToken)
        {
            using var captureHandler = new InMemoryCaptureHandler();
            using var registration = cancellationToken.Register(() => MMALCamera.Instance.ForceStop(MMALCamera.Instance.Camera.StillPort));
            await MMALCamera.Instance.TakePicture(captureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
            
            
            return new ImageCapture(captureHandler.WorkingData.ToArray(), DateTime.Now);
        }
    }
}