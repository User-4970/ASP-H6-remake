using Microsoft.AspNetCore.Mvc;
using ASP_Core.Data;
using ASP_Core.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ASP_Core.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string author)
        {
            var query = _context.Posts.Include(p => p.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(p => p.Author.Contains(author));
                ViewBag.Search = author;
            }
            var posts = query.OrderByDescending(p => p.CreatedAt).ToList();
            return View(posts);
        }

        // GET: /Posts/Search
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // POST: /Posts/Search
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string author, string submit)
        {
            // Redirect to Index with author as query parameter
            return RedirectToAction(nameof(Index), new { author = author });
        }

        public IActionResult Details(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return View("NotFound");
            }
            return View(post);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Summary,Content")] Post model)
        {
            if (!ModelState.IsValid) return View(model);
            model.Author = User.Identity?.Name ?? "Anonymous";
            model.CreatedAt = System.DateTime.UtcNow;
            _context.Posts.Add(model);
            _context.SaveChanges();
            TempData["Message"] = "Пост створено";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null) return View("NotFound");
            return View(post);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Summary,Content")] Post model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            var post = _context.Posts.Find(id);
            if (post == null) return View("NotFound");
            post.Title = model.Title;
            post.Summary = model.Summary;
            post.Content = model.Content;
            _context.SaveChanges();
            TempData["Message"] = "Пост оновлено";
            return RedirectToAction(nameof(Details), new { id = post.Id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                TempData["Message"] = "Пост не знайдено";
                return RedirectToAction(nameof(Index));
            }
            // удаляем связанные комментарии
            _context.Comments.RemoveRange(post.Comments);
            _context.Posts.Remove(post);
            _context.SaveChanges();
            TempData["Message"] = "Пост видалено";
            return RedirectToAction(nameof(Index));
        }
    }
}
