using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Greenit.Data;
using Greenit.Models;
using Greenit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Controllers
{
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogItemService _blogItemService;
        private readonly IChannelService _channelService;
        private readonly UserManager<IdentityUser> _userManager;


        public StatsController(IChannelService channelService, IBlogItemService blogItemService,
           UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _blogItemService = blogItemService;
            _channelService = channelService;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IActionResult> Index()
        {
            StatsViewModel statsView = new StatsViewModel();
            //statsView.blogPosts 

            var blogs = await _blogItemService.GetBlogPostAsync();
            var channels = await _channelService.GetChannelsAsync();
            var comments = await _blogItemService.GetCommentsAsync();

            statsView.blogPostslSize = blogs.Length;
            statsView.channelsSize = channels.Length;
            statsView.commentsSize = comments.Length;
            statsView.indvComments = await _blogItemService.GetCommentCountByUserAsync();
            statsView.indvPosts = await _blogItemService.GetPostCountByUserAsync();
            statsView.mostRecentComment = await _context.comments.OrderByDescending(c => c.Posted).Take(1).ToArrayAsync();

            return View(statsView);
        }
    }
}
