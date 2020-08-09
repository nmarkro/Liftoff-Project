using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using BCCWebApp.Data;
using System.Security.Claims;

namespace BCCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BCCDbContext context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BCCDbContext dbContext)
        {
            _logger = logger;
            context = dbContext;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                User user = context.Users.Find(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                ViewBag.TournamentEntered = user.TorunamentRegistered;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        [HttpPost("~/EnterTournament")]
        public async Task<IActionResult> Enter()
        {
            User user = await context.Users.FindAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            user.TorunamentRegistered = true;
            await context.SaveChangesAsync();
            return Json("Ok");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
