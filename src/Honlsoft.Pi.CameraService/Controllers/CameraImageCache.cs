namespace Honlsoft.Pi.CameraService.Controllers
{
    /// <summary>
    /// Caches the latest Image Capture from the camera.
    /// </summary>
    public class CameraImageCache
    {
        public ImageCapture LatestImage { get; set; }
    }
}