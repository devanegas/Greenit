using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenit.Data;
using Greenit.Models;
using Microsoft.AspNetCore.Identity;


namespace Greenit.Services
{
    public class ChannelStatsService
    {
        private readonly ApplicationDbContext _context;

        public ChannelStatsService(ApplicationDbContext DbContext)
        {
            _context = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
        }

        public async Task<BlogPost[]> GetPostsAsync(string admin)
        {
            Channel[] channels = await _context.Channels.Where(c => c.UserId == admin).ToArrayAsync();
            List<BlogPost> blogPosts = new List<BlogPost>();

            foreach (var item in channels)
            {
                BlogPost[] posts = await _context.BlogPosts.Where(b => b.ChannelId == item.Id).ToArrayAsync();
                foreach (var post in posts)
                {
                    blogPosts.Add(post);
                }
            }

            return blogPosts.ToArray();
        }
        public async Task<Comment[]> GetCommentsAsync(string admin)
        {
            Channel[] channels = await _context.Channels.Where(c=> c.UserId == admin).ToArrayAsync();

            List<Comment> comments = new List<Comment>();

            foreach(var item in channels)
            {
                BlogPost[] posts = await _context.BlogPosts.Where(b=> b.ChannelId == item.Id).ToArrayAsync();
                foreach(var post in posts)
                {
                    Comment[] commentsArray = await _context.comments.Where(ct => ct.PostId == post.Id).ToArrayAsync();
                    foreach(var comment in commentsArray)
                    {
                        comments.Add(comment);
                    }
                }
            }

            return comments.ToArray();
        }
    }
}