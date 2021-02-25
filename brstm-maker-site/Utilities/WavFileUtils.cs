using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAudio.Wave;

namespace brstm_maker_site.Utilities
{
    public static class WavFileUtils
    {
        public static byte[] TrimWavFile(Stream input, TimeSpan cutFromStart, TimeSpan cutFromEnd)
        {
            byte[] trimmedData;
            MemoryStream ms = new MemoryStream();
            using(WaveFileReader reader = new WaveFileReader(input))
            {
                using(WaveFileWriter writer = new WaveFileWriter(ms, reader.WaveFormat))
                {
                    int bytesPerMillisecond = reader.WaveFormat.AverageBytesPerSecond / 1000;
                    int startPos = (int)cutFromStart.TotalMilliseconds * bytesPerMillisecond;
                    startPos = startPos - startPos % reader.WaveFormat.BlockAlign;

                    int endBytes = (int)cutFromEnd.TotalMilliseconds * bytesPerMillisecond;
                    endBytes = endBytes - endBytes % reader.WaveFormat.BlockAlign;
                    int endPos = endBytes;

                    TrimWavFile(reader, writer, startPos, endPos);
                    writer.Dispose();
                    trimmedData = ms.ToArray();
                    return trimmedData;
                }
            }
        }

        public static void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPos, int endPos)
        {
            reader.Position = startPos;
            byte[] buffer = new byte[1024];
            while (reader.Position < endPos)
            {
                int bytesRequired = (int)(endPos - reader.Position);
                if (bytesRequired > 0)
                {
                    int bytesToRead = Math.Min(bytesRequired, buffer.Length);
                    int bytesRead = reader.Read(buffer, 0, bytesToRead);
                    if (bytesRead > 0)
                    {
                        writer.Write(buffer, 0, bytesRead);
                    }
                }
            }
        }
    }
}
