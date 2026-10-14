using Microsoft.AspNetCore.Mvc;
using ASP_Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace ASP_Core.Controllers
{
    public class GamesController : Controller
    {
        private static readonly List<Game> _games = new()
        {
            new Game{ Id = 1, Title = "The Witcher 3", Genre = "RPG", Rating = 9.5, IsCompleted = true },
            new Game{ Id = 2, Title = "Hades", Genre = "Roguelike", Rating = 9.0, IsCompleted = false },
            new Game{ Id = 3, Title = "Stardew Valley", Genre = "Simulation", Rating = 8.7, IsCompleted = true }
        };

        public IActionResult Index()
        {
            return View(_games);
        }

        public IActionResult Details(int id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                ViewBag.Message = "Гру не знайдено.";
                return View("NotFound");
            }
            return View(game);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Genre,Rating,IsCompleted")] Game model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nextId = _games.Any() ? _games.Max(g => g.Id) + 1 : 1;
            model.Id = nextId;
            _games.Add(model);
            TempData["Message"] = "Гру успішно додано.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var game = _games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                TempData["Message"] = "Гру не знайдено.";
                return RedirectToAction(nameof(Index));
            }

            _games.Remove(game);
            TempData["Message"] = "Гру видалено.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Completed()
        {
            var list = _games.Where(g => g.IsCompleted).ToList();
            return View(list);
        }
    }
}
