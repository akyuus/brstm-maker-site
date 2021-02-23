using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brstm_maker_site.Models;
using YoutubeExplode.Videos.Streams;

namespace brstm_maker_site.Interfaces
{
    public interface IYoutubeService
    {
        public Task<YoutubeMetadata> GetMetadata(string url);
        public Task<byte[]> DownloadAudio(string url);
    }
}
