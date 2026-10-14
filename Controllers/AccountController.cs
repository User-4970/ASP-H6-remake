using Microsoft.AspNetCore.Mvc;
using ASP_Core.Data;
using ASP_Core.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ASP_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Username and password required");
                return View();
            }
            if (_context.Users.Any(u => u.UserName == username))
            {
                ModelState.AddModelError("", "User already exists");
                return View();
            }
            var user = new AppUser { UserName = username, PasswordHash = Hash(password) };
            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Login(string username, string password)
        {
            var hash = Hash(password);
            var user = _context.Users.FirstOrDefault(u => u.UserName == username && u.PasswordHash == hash);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            return RedirectToAction("Index", "Posts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }

        private static string Hash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
