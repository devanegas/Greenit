using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Greenit.Data;
using Greenit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogItemService _BlogService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IBlogItemService BlogService,
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
    }
}
