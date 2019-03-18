using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Models
{
    public class ChannelStatsViewModel
    {
        public BlogPost[] blogPosts { get; set; }
        public Comment[] comments { get; set; }
        public int commentsSize { get; set; }
        public int blogPostsSize { get; set; }

        public CommentStats[] indvComments { get; set; }
        public PostStats[] indvPosts { get; set; }
        public Comment[] mostRecentComment { get; set; }
    }
}
