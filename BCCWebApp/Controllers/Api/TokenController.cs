using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCCWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace BCCWebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly BCCDbContext context;
        private readonly IConfiguration configuration;

        public TokenController(BCCDbContext dbContext, IConfiguration configuration)
        {
            context = dbContext;
            this.configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token([FromForm] string payload)
        {
            if(payload == configuration["Authentication:BCC:Secret"])
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:Jwt:Secret"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    configuration["Authentication:Jwt:Issuer"],
                    configuration["Authentication:Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);

                return Ok(new
                {
                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires_in = DateTime.Now.AddHours(3),
                    token_type = "bearer"
                });
            }
            return Unauthorized();
        }
    }
}
