using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Greenit.Data;
using Greenit.Models;
using Greenit.Services;

namespace Greenit.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IChannelService _ChannelService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment hostingEnvironment;


        public ChannelsController(IChannelService ChannelService, UserManager<IdentityUser> userManager, 
            ApplicationDbContext context)
        {
            _ChannelService = ChannelService;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Channels
        public async Task<IActionResult> Index() { 
            var currentUser = await _userManager.GetUserAsync(User);
       
            var items = await _ChannelService.GetChannelsAsync();

            return View(items);
        
            //return View(await _context.Channels.ToListAsync());
        }

        // GET: Channels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels
                .Include(c=>c.posts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (channel == null)
            {
                return NotFound();
            }

            return View(channel);
        }

        // GET: Channels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Channels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Add)]
        public async Task<IActionResult> Create([Bind("Id,Name")] Channel channel)
        {
            if (ModelState.IsValid)
            {
                channel.UserId = User.Identity.Name;
                _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(channel.UserId), "ChannelAdmin");
                _context.Add(channel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(channel);
        }

        // GET: Channels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                return NotFound();
            }
            return View(channel);
        }

        // POST: Channels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Channel channel)
        {
            if (id != channel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelExists(channel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(channel);
        }



        private bool ChannelExists(int id)
        {
            return _context.Channels.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Add)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost([Bind("Id,ChannelId,Title,Body,Posted")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                blogPost.UserId = User.Identity.Name;
                blogPost.Posted = DateTime.Now;
                var channel = await _context.Channels.Where(p => p.Id == blogPost.ChannelId)
                                                .Include(p => p.posts)
                                                .FirstOrDefaultAsync();
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }
    }
}
