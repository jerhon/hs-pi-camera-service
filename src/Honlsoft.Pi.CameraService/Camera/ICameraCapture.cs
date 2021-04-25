using System.Threading;
using System.Threading.Tasks;

namespace Honlsoft.Pi.CameraService.Camera
{
    public interface ICameraCapture
    {
        Task<ImageCapture> GetImageAsync(CancellationToken cancellationToken);
    }
}