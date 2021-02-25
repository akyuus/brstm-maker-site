using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brstm_maker_site.Interfaces;
using static brstm_maker_site.Utilities.WavFileUtils;
using brstm_maker_site.Models;
using NAudio;
using NAudio.Wave;
using SoundTouch;
using SoundTouch.Net.NAudioSupport;
using VGAudio.Formats;
using VGAudio.Containers.Wave;
using VGAudio.Containers.NintendoWare;

namespace brstm_maker_site.Services
{
    public class BrstmService : IBrstmService
    {
        public BrstmService()
        {
        }

        public async Task<byte[]> makeBrstm(BrstmOptions options, string url)
        {
            byte[] data;
            YoutubeService yt = new YoutubeService();
            YoutubeMetadata ytmd = await yt.GetMetadata(url);
            MemoryStream ms = new MemoryStream(await yt.DownloadAudio(HttpUtility.UrlDecode(url)));
            data = TrimWavFile(ms, TimeSpan.FromSeconds(options.start), TimeSpan.FromSeconds(options.end));
            data = raiseVolume(data, options.decibelIncrease);
            if(options.finalLap)
            {
                data = adjustSpeed(data, options.speedFactor);
            }
            data = adjustChannels(data, options.courseInfo.channelCount);
            return convertToBrstm(data);
        }
        public byte[] raiseVolume(byte[] data, int decibels)
        {
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
            double percentage = Math.Pow(10, ((double)decibels) / 10);
            
            using(var reader = new WaveFileReader(msInput))
            {
                var volumeProvider = new VolumeWaveProvider16(reader);
                volumeProvider.Volume = (float)percentage;
                WaveFileWriter.WriteWavFileToStream(msOutput, volumeProvider);
                return msOutput.ToArray();
            }
        }
        public byte[] adjustChannels(byte[] data, int channelCount)
        {
            if (channelCount == 2) return data;

            MemoryStream ms = new MemoryStream();
            using(var reader = new WaveFileReader(new MemoryStream(data)))
            {
                var multiplexer = new MultiplexingWaveProvider(new IWaveProvider[] { reader }, channelCount);
                WaveFileWriter.WriteWavFileToStream(ms, multiplexer);
            }

            return ms.ToArray();
        }
        public byte[] adjustSpeed(byte[] data, double speedFactor)
        {
            MemoryStream msInput = new MemoryStream(data);
            MemoryStream msOutput = new MemoryStream();
            string tempfilename = $"{DateTime.Now.ToFileTimeUtc()}.wav";
            using (var reader = new WaveFileReader(msInput))
            {
                var stupidIeeeNecessity = WaveFormat.CreateIeeeFloatWaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.Channels);
                using(var resampler = new MediaFoundationResampler(reader, stupidIeeeNecessity))
                {
                    WaveFileWriter.CreateWaveFile(tempfilename, resampler);
                }
            }

            using(var ieeeReader = new WaveFileReader(tempfilename))
            {
                var stwp = new SoundTouchWaveProvider(ieeeReader);
                stwp.Tempo = speedFactor;
                WaveFileWriter.WriteWavFileToStream(msOutput, stwp);
            }

            File.Delete(tempfilename);
            return msOutput.ToArray();
        }
        public byte[] convertToBrstm(byte[] data)
        {
            string temppath = $"{DateTime.Now.ToFileTimeUtc()}.wav";
            using (var input = new WaveFileReader(new MemoryStream(data)))
            {
                if (input.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                {
                    var outFormat = new WaveFormat(input.WaveFormat.SampleRate, input.WaveFormat.Channels);
                    using (var resampler = new MediaFoundationResampler(input, outFormat))
                    {
                        // resampler.ResamplerQuality = 48;
                        WaveFileWriter.CreateWaveFile(temppath, resampler);
                    }

                    data = File.ReadAllBytes(temppath);
                }
            }

            WaveReader reader = new WaveReader();
            WaveStructure structure = reader.ReadMetadata(new MemoryStream(data));
            AudioData audio = reader.Read(data);
            audio.SetLoop(true, 0, structure.SampleCount);
            byte[] brstmFile = new BrstmWriter().GetFile(audio);
            File.Delete(temppath);
            return brstmFile;
        }
    }
}
