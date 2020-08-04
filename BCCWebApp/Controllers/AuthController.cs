using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BCCWebApp.Data;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCCWebApp.Controllers
{
    public class AuthController : Controller
    {
        private BCCDbContext context;

        public AuthController(BCCDbContext dbContext)
        {
            context = dbContext;
        }

        [HttpGet("/signin")]
        public IActionResult SignIn(string returnUrl = "/signin-redirect")
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
            }
            return Redirect("/");
        }

        [HttpGet("/signin-redirect")]
        public async Task<IActionResult> OnSignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                User existingUser = context.Users.Find(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                if (existingUser != null)
                {
                    existingUser.DisplayName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                    existingUser.ProfileImageUrl = User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:profile_image_url").Value;
                } else
                {
                    User newUser = new User(
                        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                        User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:login").Value,
                        User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                        User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:profile_image_url").Value);
                    context.Users.Add(newUser);
                }
                await context.SaveChangesAsync();
            }

            return Redirect("/");
        }

        [Authorize]
        [Route("/signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
