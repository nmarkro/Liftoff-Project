using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCCWebApp.Data;
using BCCWebApp.Models;

namespace BCCWebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BCCDbContext _context;

        public UsersController(BCCDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromQuery] string[] id, [FromQuery] bool entered = false)
        {
            List<User> users = await _context.Users.Where(u => u.TorunamentRegistered == entered).ToListAsync();

            if (id.Length > 0)
            {
                users = users.Where(u => id.Contains(u.Id)).ToList();
            }

            return users;
        }
    }
}
