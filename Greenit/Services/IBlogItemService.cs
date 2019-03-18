using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greenit.Models;

namespace Greenit.Services
{
    public interface IBlogItemService
    {
        Task<BlogPost[]> GetBlogPostAsync();
        Task<bool> AddBlogPostAsync(BlogPost newpost);
        Task<Comment[]> GetCommentsAsync();
        Task<CommentStats[]> GetCommentCountByUserAsync();
        Task<PostStats[]> GetPostCountByUserAsync();
    }
}
