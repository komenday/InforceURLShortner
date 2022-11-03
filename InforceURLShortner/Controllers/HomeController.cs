﻿using InforceURLShortner.Models;
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
                    AuthorLogin = User?.Identity.Name,
                    CreationDate = DateTime.Now
                };
                _context.Add(url);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
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

            if ((User?.IsInRole("admin") ?? false) || User?.Identity?.Name == url.AuthorLogin)
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
    }
}