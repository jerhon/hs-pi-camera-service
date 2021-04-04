using System.Net;
using System.Net.Mime;
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
            // TODO: eventually have a background service that takes a picture every X seconds, then have this just return the latest.
            var lastImage = _imageCache.LatestImage;
            if (lastImage == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
            }

            return new FileContentResult(lastImage.data, MediaTypeNames.Image.Jpeg);
        }
    }
}
