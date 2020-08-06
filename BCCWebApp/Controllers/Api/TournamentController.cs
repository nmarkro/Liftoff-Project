using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCCWebApp.Data;
using BCCWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BCCWebApp.Controllers.Api
{
    public class TournamentEntry
    {
        public string UserId { get; set; }
        public int DeckId { get; set; }
        public string UserDisplayName { get; set; }
        public string DeckNaviName { get; set; }
        public string DeckNaviCode { get; set; }
        
        public TournamentEntry(User user, Deck deck)
        {
            UserId = user.Id;
            DeckId = deck.Id;
            UserDisplayName = user.DisplayName;
            DeckNaviName = deck.NaviName;
            DeckNaviCode = deck.NaviCode;
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly BCCDbContext _context; 
        private Random rnd;

        public TournamentController(BCCDbContext context)
        {
            _context = context;
            rnd = new Random();
        }

        // GET: api/Tournament
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentEntry>>> GetTournament([FromQuery] int size = 16)
        {
            List<TournamentEntry> entries = new List<TournamentEntry>();

            // Add users that have entered the tournament and have a deck currently selected first
            List<User> enteredUsers = await _context.Users.Where(u => u.TorunamentRegistered && u.CurrentDeckId != null).ToListAsync();
            enteredUsers = enteredUsers.OrderBy(u => rnd.Next()).ToList();

            foreach(User u in enteredUsers)
            {
                if (entries.Count >= size)
                {
                    break;
                }
                Deck deck = _context.Decks.Find(u.CurrentDeckId);
                if (deck != null)
                {
                    entries.Add(new TournamentEntry(u, deck));
                }
            }

            // Add users that have entered the tournament and have a deck currently selected 2nd
            if (entries.Count < size)
            {
                List<User> usersWithDeck = await _context.Users.Where(u => u.CurrentDeckId != null).ToListAsync();
                usersWithDeck = usersWithDeck.OrderBy(x => rnd.Next()).ToList();
                foreach (User u in usersWithDeck)
                { 
                    if (entries.Find(e => e.UserId == u.Id) != null)
                    {
                        continue;
                    }
                    if (entries.Count >= size)
                    {
                        break;
                    }

                    Deck deck = _context.Decks.Find(u.CurrentDeckId);
                    if (deck != null)
                    {
                        entries.Add(new TournamentEntry(u, deck));
                    }
                }
            }

            // Add random decks w/ users last
            if (entries.Count < size)
            {
                List<Deck> decks = await _context.Decks.Where(d => d.UserId != null).ToListAsync();
                decks = decks.OrderBy(x => rnd.Next()).ToList();

                foreach (Deck d in decks)
                {
                    if(entries.Find(e => e.DeckId == d.Id) != null)
                    {
                        continue;
                    }
                    if (entries.Count >= size)
                    {
                        break;
                    }

                    User user = _context.Users.Find(d.UserId);
                    if (user != null)
                    {
                        if (entries.Find(e => e.UserId == user.Id) != null)
                        {
                            continue;
                        }
                        entries.Add(new TournamentEntry(user, d));
                    }
                }
            }

            return entries.OrderBy(x => rnd.Next()).Take(size).ToList();
        }
    }
}
