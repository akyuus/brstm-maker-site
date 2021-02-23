using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brstm_maker_site.Models
{
    public class YoutubeMetadata
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
