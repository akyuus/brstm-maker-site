using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using brstm_maker_site.Models;

namespace brstm_maker_site.Interfaces
{
    public interface IBrstmService
    {
        Task<byte[]> makeBrstm(BrstmOptions options, string url);
        byte[] raiseVolume(byte[] data, int decibels);
        byte[] adjustChannels(byte[] data, int channelCount);
        byte[] adjustSpeed(byte[] data, double speedFactor);
        byte[] convertToBrstm(byte[] data);
    }
}
