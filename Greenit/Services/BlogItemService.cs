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
    public class BlogItemService : IBlogItemService
    {
        private readonly ApplicationDbContext _context;


        public BlogItemService(ApplicationDbContext DbContext)
        {
            _context = DbContext ?? throw new ArgumentNullException(nameof(DbContext));
        }

        public async Task<BlogPost[]> GetBlogPostAsync()
        {
            return await _context.BlogPosts
                .Include(c => c.Comments)
                .ToArrayAsync();
               
        }

        public async Task<bool> AddBlogPostAsync(BlogPost newBlogPost)
        {
            _context.BlogPosts.Add(newBlogPost);
            var saveResult = await _context.SaveChangesAsync();
            return (saveResult == 1);
        }

        public async Task<Comment[]> GetCommentsAsync()
        {
            return await _context.comments
                .ToArrayAsync();
        }

        public async Task<CommentStats[]> GetCommentCountByUserAsync()
        {
            return await _context.comments.GroupBy(p => p.UserId).Select(g => new CommentStats{ userid = g.Key, count = g.Count() })
                .ToArrayAsync();
        }
        public async Task<PostStats[]> GetPostCountByUserAsync()
        {
            return await _context.BlogPosts.GroupBy(p => p.UserId).Select(g => new PostStats { userid = g.Key, count = g.Count() })
                .ToArrayAsync();
        }
    }
}
