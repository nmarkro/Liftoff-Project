using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BCCWebApp.Data;
using BCCWebApp.Models;
using BCCWebApp.Scripts;
using BCCWebApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host.Mef;

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
            AddOrEditDeckViewModel addOrEditDeckViewModel = new AddOrEditDeckViewModel();

            return View(addOrEditDeckViewModel);
        }

        [Authorize]
        [Route("/decks/import")]
        public IActionResult Import()
        {
            ImportDeckViewModel importDeckViewModel = new ImportDeckViewModel();

            return View(importDeckViewModel);
        }

        [Authorize]
        [Route("/decks/edit/{id}")]
        public IActionResult Edit(int id)
        {
            Deck deck = context.Decks.Find(id);
            if (deck != null)
            {
                if (deck.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    AddOrEditDeckViewModel addOrEditDeckViewModel = new AddOrEditDeckViewModel(deck);
                    return View(addOrEditDeckViewModel);
                } else
                {
                    return BadRequest();
                }
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("/decks/delete/{id}")]
        public IActionResult Delete(int id)
        {
            Deck deck = context.Decks.Find(id);
            if (deck != null)
            {
                if (deck.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    context.Decks.Remove(deck);
                    context.SaveChanges();

                    return Redirect("/Decks");
                }
                else
                {
                    return BadRequest();
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessAddDeckForm(AddOrEditDeckViewModel addOrEditDeckViewModel)
        {
            if (ModelState.IsValid)
            {
                string naviCode = generateNaviCode(addOrEditDeckViewModel);
                Deck newDeck = new Deck
                {
                    UserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    Name = addOrEditDeckViewModel.DeckName,
                    NaviName = makeNaviName(User.Identity.Name),
                    NaviCode = naviCode,
                    Wins = 0,
                    Battles = 0
                };

                context.Decks.Add(newDeck);
                context.SaveChanges();

                return Redirect("/Decks");
            }

            return View("Add", addOrEditDeckViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessEditDeckForm(AddOrEditDeckViewModel addOrEditDeckViewModel)
        {
            if (ModelState.IsValid)
            {
                string naviCode = generateNaviCode(addOrEditDeckViewModel);

                Deck deck = context.Decks.Find(addOrEditDeckViewModel.DeckId);
                if (deck != null)
                {
                    if (deck.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                    {
                        deck.Name = addOrEditDeckViewModel.DeckName;
                        deck.NaviCode = naviCode;
                        context.SaveChanges();

                        return Redirect("/Decks");
                    } else
                    {
                        return BadRequest();
                    }
                } else
                {
                    return NotFound();
                }
            }

            return View("Edit", addOrEditDeckViewModel);
        }

        [Authorize]
        [HttpPut("/decks/SetChosenDeck")]
        public void SetChosenDeck(int chosenId)
        {
            Deck chosenDeck = context.Decks.Where(d => d.Id == chosenId).FirstOrDefault();
            if (chosenDeck != null)
            {
                if (chosenDeck.UserId == User.FindFirst(ClaimTypes.NameIdentifier).Value)
                {
                    User appUser = context.Users.Where(u => u.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault();
                    appUser.CurrentDeckId = chosenId;
                    context.SaveChanges();
                }
            }
        }

        private string makeNaviName(string name)
        {
            string naviName = name.ToUpper();
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

            return naviName;
        }

        private string generateNaviCode(AddOrEditDeckViewModel addOrEditDeckViewModel)
        {
            string naviName = makeNaviName(User.Identity.Name);

            int[] data = new int[]
            {
                addOrEditDeckViewModel.Operator,
                addOrEditDeckViewModel.ChipNavi,
                addOrEditDeckViewModel.Chip1a,
                addOrEditDeckViewModel.Chip1b,
                addOrEditDeckViewModel.Chip2a,
                addOrEditDeckViewModel.Chip2b,
                addOrEditDeckViewModel.Chip2c,
                addOrEditDeckViewModel.Chip3a,
                addOrEditDeckViewModel.Chip3b,
                addOrEditDeckViewModel.Chip3c,
                addOrEditDeckViewModel.Chip3d,
                addOrEditDeckViewModel.ChipR,
                addOrEditDeckViewModel.ChipL,
                BCCData.CodeTypes["Game Boy Advance"],  // Code type here doesn't matter, just default to GBA
                0,                                      // Checksum (will be calculated later)
            };

            string naviCode = Util.PackNaviCode(naviName, data);
            naviCode = Util.Decorate(naviCode);

            return naviCode;
        }
    }
}
