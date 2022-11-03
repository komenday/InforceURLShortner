using InforceURLShortner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace InforceURLShortner.Controllers
{
    public class HomeController : Controller
    {
        readonly InforceShortnerContext _context;
        public HomeController(InforceShortnerContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            return View(await _context.Urls.ToListAsync());
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullUrl)
        {
            if (_context.Urls.Where(u => u.FullURL == fullUrl).Count() == 0)
            {
                Url url = new Url
                {
                    FullURL = fullUrl,
                    ShortURL = UrlShortner.IdToShortURL(fullUrl.GetHashCode()),
                    AuthorLogin = User.Identity.Name,
                    CreationDate = DateTime.Now
                };
                _context.Add(url);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Create));
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var url = await _context.Urls.FirstOrDefaultAsync(m => m.Id == id);

            if (url == null)
                return NotFound();

            return View(url);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var url = await _context.Urls.FirstOrDefaultAsync(u => u.Id == id);

            if (url == null)
                return NotFound();

            if (User.IsInRole("admin") || User.Identity.Name == url.AuthorLogin)
                return View(url);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Url url = await _context.Urls.FindAsync(id);
            _context.Urls.Remove(url);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            string path = @"C:\Users\komen\source\repos\InforceURLShortner\InforceURLShortner\wwwroot\about.txt";
            string text = Environment.NewLine;

            using (StreamReader reader = new StreamReader(path))
            {
                text = await reader.ReadToEndAsync();
            }
            return View((object)text);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult EditAbout(string innerText)
        {
            return View((object)innerText);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RecordEditAbout(string edited)
        {
            string path = @"C:\Users\komen\source\repos\InforceURLShortner\InforceURLShortner\wwwroot\about.txt";
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(edited);
            }
            return RedirectToAction(nameof(About));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}