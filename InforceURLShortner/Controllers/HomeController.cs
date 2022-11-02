using InforceURLShortner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InforceURLShortner.Controllers
{
    public class HomeController : Controller
    {
        InforceShortnerContext _context;
        public HomeController(InforceShortnerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Urls.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullUrl)
        {
            Url url = new Url {
                FullURL = fullUrl,
                ShortURL = UrlShortner.IdToShortURL(fullUrl.GetHashCode()),
                AuthorLogin = User.Identity.Name,
                CreationDate = DateTime.Now
            };
            _context.Add(url);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}