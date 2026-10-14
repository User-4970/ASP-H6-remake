using Microsoft.AspNetCore.Mvc;
using ASP_Core.Models;
using System.Collections.Generic;
using System.Linq;


namespace ASP_Core.Controllers
{
    public class ProductController : Controller
    {
        private static readonly List<Product> _products = new()
        {
            new Product{ Id = 1, Name = "Банани", Price = 1.39m, Description = "За 1 кілограм" },
            new Product{ Id = 2, Name = "Кавуни", Price = 0.89m, Description = "За 1 кілограм" },
            new Product{ Id = 3, Name = "Дині", Price = 1.49m, Description = "За 1 кілограм" }
        };

        public IActionResult Index()
        {
            return View(_products);
        }

        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            ViewBag.Message = TempData["Message"] as string;
            return View(product);
        }

        public IActionResult Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return View("Index", _products);

            var results = _products.Where(p => p.Name.Contains(name, System.StringComparison.OrdinalIgnoreCase)).ToList();
            return View("Index", results);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Price,Description")] Product model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var nextId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            var product = new Product { Id = nextId, Name = model.Name, Price = model.Price, Description = model.Description };
            _products.Add(product);

            TempData["Message"] = "Товар успішно створено.";
            return RedirectToAction(nameof(Details), new { id = product.Id });
        }
    }
}
