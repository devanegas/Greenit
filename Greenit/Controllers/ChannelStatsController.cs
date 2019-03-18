using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Greenit.Data;
using Greenit.Models;
using Greenit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Greenit.Controllers
{
    public class ChannelStatsController : Controller
    {
        private readonly ChannelStatsService _channelStatsService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public ChannelStatsController(ChannelStatsService channelStatsService,
            UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _channelStatsService = channelStatsService;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            ChannelStatsViewModel channelStatsViewModel = new ChannelStatsViewModel();
            string ChannelOwner = User.Identity.Name;

            channelStatsViewModel.comments = await _channelStatsService.GetCommentsAsync(ChannelOwner);
            channelStatsViewModel.blogPosts = await _channelStatsService.GetPostsAsync(ChannelOwner);
            channelStatsViewModel.blogPostsSize = channelStatsViewModel.blogPosts.Length;
            channelStatsViewModel.commentsSize = channelStatsViewModel.comments.Length;

            return View(channelStatsViewModel);
        }

    }
}

