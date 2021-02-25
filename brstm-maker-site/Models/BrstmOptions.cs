using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brstm_maker_site.Models
{
    public class BrstmOptions
    {
        public BrstmOptions()
        {
        }
        public CourseInfo courseInfo { get; set; }
        public bool finalLap { get; set; }
        //these are all in seconds
        public int start { get; set; }
        public int end { get; set; }
        public int loopPoint { get; set; }
        public double speedFactor { get; set; }
        public int decibelIncrease { get; set; }
    }
}
