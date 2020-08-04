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
    public class DecksController : ControllerBase
    {
        private Random rnd;
        private readonly BCCDbContext _context;

        public DecksController(BCCDbContext context)
        {
            _context = context;
            rnd = new Random();
        }

        // GET: api/Decks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDecks([FromQuery] int[] id)
        {
            List<Deck> decks = await _context.Decks.ToListAsync();

            if (id.Length > 0)
            {
                decks = decks.Where(d => id.Contains(d.Id)).ToList();
            }

            return decks;
        }
    }
}
