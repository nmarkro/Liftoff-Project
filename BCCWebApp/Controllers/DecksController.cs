using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCCWebApp.Data;
using BCCWebApp.Models;
using BCCWebApp.Scripts;
using BCCWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCCWebApp.Controllers
{
    public class DecksController : Controller
    {
        private BCCDbContext context;

        public DecksController(BCCDbContext dbContext)
        {
            context = dbContext;
        }

        [Authorize]
        [Route("/decks")]
        public IActionResult Index()
        {
            ViewBag.Decks = context.Decks.Where(d => d.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value).ToList();
            ViewBag.SelectedDeckId = context.Users.Where(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault().CurrentDeckId;

            return View();
        }

        [Authorize]
        [Route("/decks/add")]
        public IActionResult Add()
        {
            AddDeckViewModel addDeckViewModel = new AddDeckViewModel();

            return View(addDeckViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ProcessAddDeckForm(AddDeckViewModel addDeckViewModel)
        {
            if (ModelState.IsValid)
            {
                Deck newDeck = GenerateNewDeck(addDeckViewModel);
                context.Decks.Add(newDeck);
                context.SaveChanges();

                return Redirect("/Decks");
            }

            return View("Add", addDeckViewModel);
        }


        [Authorize]
        [HttpPut("/decks/SetChosenDeck")]
        public void SetChosenDeck(int chosenId)
        {
            User appUser = context.Users.Where(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
            appUser.CurrentDeckId = chosenId;
            context.SaveChanges();
        }

        private Deck GenerateNewDeck(AddDeckViewModel addDeckViewModel)
        {
            string naviName = User.Identity.Name.ToUpper();
            if (naviName.Length > 4)
            {
                string vowelless = new string(naviName.Where(c => !"AEIOUY".Contains(c)).ToArray());
                if (vowelless.Length > 1)
                {
                    naviName = vowelless;
                }
            }
            if (naviName.Length > 4)
            {
                naviName = naviName.Substring(0, 4);
            }

            int[] data = new int[]
            {
                addDeckViewModel.Operator,
                addDeckViewModel.ChipNavi,
                addDeckViewModel.Chip1a,
                addDeckViewModel.Chip1b,
                addDeckViewModel.Chip2a,
                addDeckViewModel.Chip2b,
                addDeckViewModel.Chip2c,
                addDeckViewModel.Chip3a,
                addDeckViewModel.Chip3b,
                addDeckViewModel.Chip3c,
                addDeckViewModel.Chip3d,
                addDeckViewModel.ChipR,
                addDeckViewModel.ChipL,
                BCCData.CodeTypes["Game Boy Advance"],
                0,
            };

            string naviCode = Util.PackNaviCode(naviName, data);
            naviCode = Regex.Replace(naviCode, ".{4}(?!$)", "$0-");
            

            Deck newDeck = new Deck
            {
                UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Name = addDeckViewModel.DeckName,
                NaviName = naviName,
                NaviCode = naviCode,
                Wins = 0,
                Battles = 0
            };

            return newDeck;
        }
    }
}
