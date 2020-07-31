using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BCCWebApp.Data;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        [HttpPost("/signin")]
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
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string refreshToken = await HttpContext.GetTokenAsync("refresh_token");
                TwitchUser existingUser = context.TwitchUsers.Find(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                if (existingUser != null)
                {
                    existingUser.AccessToken = accessToken;
                    existingUser.DisplayName = User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:display_name").Value;
                    existingUser.ProfileImageUrl = User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:profile_image_url").Value;
                } else
                {
                    TwitchUser newTwitchUser = new TwitchUser
                    {
                        Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                        Login = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                        DisplayName = User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:display_name").Value,
                        ProfileImageUrl = User.Claims.FirstOrDefault(c => c.Type == "urn:twitch:profile_image_url").Value,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    };
                    User newUser = new User(newTwitchUser);
                    context.TwitchUsers.Add(newTwitchUser);
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
