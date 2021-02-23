using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using brstm_maker_site.Services;
using brstm_maker_site.Interfaces;
using YoutubeExplode.Videos.Streams;

namespace brstm_maker_site.Controllers
{
    [Route("api/yt")]
    [ApiController]
    public class YoutubeController : ControllerBase
    {
        private readonly IYoutubeService _youtubeService;

        public YoutubeController(IYoutubeService youtubeService)
        {
            _youtubeService = youtubeService;
        }

        [HttpGet("metadata/{url}")]
        public async Task<IActionResult> GetMetadataAsync(string url)
        {
            var metadata = await _youtubeService.GetMetadata(url);
            return Ok(metadata);
        }

        [HttpGet("{url}")]
        public async Task<ActionResult> DownloadFile(string url)
        {
            byte[] data = await _youtubeService.DownloadAudio(url);
            return File(data, "application/octet-stream", "test.wav");
        }
    }
}
