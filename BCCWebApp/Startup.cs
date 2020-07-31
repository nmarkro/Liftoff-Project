using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using BCCWebApp.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BCCWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private static string GetJsonData(JsonElement user, string key)
        {
            if (!user.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
            {
                return null;
            }

            return data.EnumerateArray().Select((p) => p.GetString(key)).FirstOrDefault();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<BCCDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "Twitch";
                })
                .AddCookie()
                .AddOAuth("Twitch", options =>
                {
                    options.ClientId = Configuration["Authentication:Twitch:ClientId"];
                    options.ClientSecret = Configuration["Authentication:Twitch:ClientSecret"];
                    options.CallbackPath = new PathString("/twitch-oauth");
                    options.AuthorizationEndpoint = "https://id.twitch.tv/oauth2/authorize";
                    options.TokenEndpoint = "https://id.twitch.tv/oauth2/token";
                    options.UserInformationEndpoint = "https://api.twitch.tv/helix/users";
                    options.SaveTokens = true;
                    options.ClaimActions.MapCustomJson(ClaimTypes.NameIdentifier, user => GetJsonData(user, "id"));
                    options.ClaimActions.MapCustomJson(ClaimTypes.Name, user => GetJsonData(user, "login"));
                    options.ClaimActions.MapCustomJson("urn:twitch:display_name", user => GetJsonData(user, "display_name"));
                    options.ClaimActions.MapCustomJson("urn:twitch:profile_image_url", user => GetJsonData(user, "profile_image_url"));
                    options.Events = new OAuthEvents
                    {
                        OnCreatingTicket = async context =>
                        {
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            request.Headers.Add("Client-ID", Configuration["Authentication:Twitch:ClientId"]);
                            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();
                            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                            context.RunClaimActions(json.RootElement);
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
