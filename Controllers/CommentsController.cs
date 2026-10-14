using Microsoft.AspNetCore.Mvc;
using ASP_Core.Data;
using ASP_Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP_Core.Controllers
{
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;
        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Comments/Create?postId=1
        [Authorize]
        [HttpGet]
        public IActionResult Create(int postId)
        {
            var post = _context.Posts.Find(postId);
            if (post == null) return NotFound();
            ViewBag.PostId = postId;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int postId, string text)
        {
            var post = _context.Posts.Find(postId);
            if (post == null) return NotFound();
            var comment = new Comment
            {
                PostId = postId,
                Text = text,
                Author = User.Identity?.Name ?? "Anonymous"
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();
            TempData["Message"] = "Коментар додано";
            return RedirectToAction("Details", "Posts", new { id = postId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                TempData["Message"] = "Коментар не знайдено";
                return RedirectToAction("Index", "Posts");
            }
            var postId = comment.PostId;
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            TempData["Message"] = "Коментар видалено";
            return RedirectToAction("Details", "Posts", new { id = postId });
        }
    }
}
