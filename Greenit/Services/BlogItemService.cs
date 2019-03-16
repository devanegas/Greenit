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

    }
}
