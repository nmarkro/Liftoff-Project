using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCCWebApp.Data;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BCCWebApp.Controllers.Api
{
    [Authorize(AuthenticationSchemes = "Jwt")]
    [Route("api/[controller]")]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly BCCDbContext _context;
        private Random rnd;

        public DecksController(BCCDbContext context)
        {
            _context = context;
            rnd = new Random();
        }

        // GET: api/Decks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDecks([FromQuery] int[] id, [FromQuery] bool random = false, [FromQuery] int count = -1)
        {
            List<Deck> decks = await _context.Decks.ToListAsync();

            if (id.Length > 0)
            {
                decks = decks.FindAll(d => id.Contains(d.Id));
            }

            if (random)
            {
                decks = decks.OrderBy(x => rnd.Next()).ToList();
            }

            if (count > 0)
            {
                return decks.Take(count).ToList();
            }
            else
            {
                return decks;
            }
        }
    }
}
