using BCCWebApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BCCWebApp.Data
{
    public class BCCDbContext : DbContext
    {
        public DbSet<TwitchUser> TwitchUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }

        public BCCDbContext(DbContextOptions<BCCDbContext> options) : base(options)
        {
        }
    }
}
