using System.Net;
using System.Net.Mime;
using Honlsoft.Pi.CameraService.Camera;
using Microsoft.AspNetCore.Mvc;

namespace Honlsoft.Pi.CameraService.Controllers
{
    /// <summary>
    /// Returns the latest picture in the CameraImageCache
    /// </summary>
    [ApiController]
    [Route("camera")]
    public class CameraController : ControllerBase
    {
        private readonly CameraImageCache _imageCache;
        
        public CameraController(CameraImageCache imageCache)
        {
            _imageCache = imageCache;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            var lastImage = _imageCache.LatestImage;
            if (lastImage == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
            }

            return new FileContentResult(lastImage.data, MediaTypeNames.Image.Jpeg);
        }
    }
}
