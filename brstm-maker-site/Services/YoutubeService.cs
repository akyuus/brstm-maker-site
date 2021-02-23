using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;
using brstm_maker_site.Models;
using brstm_maker_site.Interfaces;

namespace brstm_maker_site.Services
{
    public class YoutubeService : IYoutubeService
    {
        private readonly YoutubeClient youtube = new YoutubeClient();
        public YoutubeService()
        {
        }

        public async Task<YoutubeMetadata> GetMetadata(string url)
        {
            var video = await youtube.Videos.GetAsync(HttpUtility.UrlDecode(url));
            var metadata = new YoutubeMetadata
            {
                Id = video.Id.Value,
                Author = video.Author,
                Title = video.Title,
                Duration = video.Duration
            };

            return metadata;
        }
        /// <summary>
        /// Returns a wav file downloaded from the given youtube URL.
        /// </summary>
        /// <param name="url">The youtube url.</param>
        /// <returns></returns>
        public async Task<byte[]> DownloadAudio(string url)
        {
            byte[] data;
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(HttpUtility.UrlDecode(url));
            var streamInfo = streamManifest.GetAudioOnly().WithHighestBitrate();
            await youtube.Videos.DownloadAsync(new IStreamInfo[] { streamInfo }, new ConversionRequestBuilder("test.wav").Build());

            data = File.ReadAllBytes("test.wav");
            File.Delete("test.wav");
            return data;
        }
    }
}
