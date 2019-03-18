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
    public class UserManagerController:  Controller
    {
        private readonly IBlogItemService _BlogService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly SignInManager<IdentityUser> SignInManager;

        public UserManagerController(IBlogItemService BlogService,
            UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _BlogService = BlogService;
            _userManager = userManager;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser ([Bind("Email,Password")] UserModel user)
        {
            IdentityUser User = new IdentityUser(user.Email);
            User.Email = user.Email;
            User.NormalizedEmail = user.Email.ToUpper();
            
            _userManager.CreateAsync(User,user.Password);
            _userManager.AddToRoleAsync(User, "Contributor");

            return View("../Home/Index");
        }

        
        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> GetUserLists()
        {
            var userslist = await _userManager.Users.ToListAsync();

            List<UserModel> userslist2 = new List<UserModel>();

            foreach(var bob in userslist)
            {
                userslist2.Add(new UserModel { Email = bob.Email });
            }

            return View(userslist2);
        }
        

        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> BlockUserFromSite(string user)
        {
            IdentityUser RemovedUser = await _userManager.FindByEmailAsync(user);

            await _userManager.RemoveFromRoleAsync(RemovedUser, "Administrator");
            await _userManager.RemoveFromRoleAsync(RemovedUser, "Contributor");
            



            return View("../Home/Index");
        }



        [Authorize(Policy = MyIdentityData.BlogPolicy_Delete)]
        public async Task<IActionResult> UnblockUserFromSite(string user)
        {
            var RemovedUser = await _userManager.FindByEmailAsync(user);

            await _userManager.AddToRoleAsync(RemovedUser, "Contributor");

            return View("../Home/Index");
        }
    }
}
