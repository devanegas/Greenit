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
    //[Authorize]
    public class BlogPostsController : Controller
    {
        private readonly IBlogItemService _BlogService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public BlogPostsController(IBlogItemService BlogService,
            UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _BlogService = BlogService;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        

    public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            //if (currentUser == null) return Challenge();

            //this was var items = await _BlogService.GetBlogPostAsync(currentUser);
            var items = await _BlogService.GetBlogPostAsync();

            return View(items);

            //var currentUser = await _userManger.GetUserAsync(User); 
        }
         
        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = MyIdentityData.BlogPolicy_Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            //blogPost.Posted = DateTime.Now;
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Edit)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Posted")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

                if (ModelState.IsValid)
                {
                    try
                    {
                        blogPost.Posted = DateTime.Now;
                        _context.Update(blogPost);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BlogPostExists(blogPost.Id))
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
                return View(blogPost);
            }

        // GET: BlogPosts/Delete/5
        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

                var blogPost = await _context.BlogPosts
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (blogPost == null)
                {
                    return NotFound();
                }

                return View(blogPost);
            }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize(Policy = MyIdentityData.BlogPolicy_Add)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([Bind("Id,UserId,PostId,Body,Posted")] Comment comment)
        {
            if (ModelState.IsValid)
            {

                comment.UserId = User.Identity.Name;
                comment.Posted = DateTime.Now;


                var Post = await _context.BlogPosts.Where(p => p.Id == comment.PostId)
                                                .Include(p => p.Comments)
                                                .FirstOrDefaultAsync();
                Post.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }



    }
}