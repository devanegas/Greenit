using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Models
{
    public class StatsViewModel
    {
        public Channel[] channels { get; set;}
        public BlogPost[] blogPosts { get; set; }
        public Comment[] comments { get; set; }
        public int channelsSize { get; set; }
        public int blogPostslSize { get; set; }
        public int commentsSize { get; set; }
    }
}
