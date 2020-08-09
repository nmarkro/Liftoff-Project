using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCCWebApp.Data;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BCCWebApp.Controllers.Api
{
    [Authorize(AuthenticationSchemes = "Jwt")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BCCDbContext _context;
        private Random rnd;

        public UsersController(BCCDbContext context)
        {
            _context = context;
            rnd = new Random();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] string[] id, [FromQuery] bool random = false, [FromQuery] int count = -1, [FromQuery] bool entered = false)
        {
            List<User> users = await _context.Users.Where(u => u.TorunamentRegistered == entered).ToListAsync();

            if (id.Length > 0)
            {
                users = users.FindAll(u => id.Contains(u.Id));
            }

            if (random)
            {
                users = users.OrderBy(x => rnd.Next()).ToList();
            }

            if (count > 0) {
                return users.Take(count).ToList();
            } else
            { 
                return users;
            }
        }
    }
}
