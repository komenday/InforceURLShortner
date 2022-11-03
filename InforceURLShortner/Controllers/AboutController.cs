using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InforceURLShortner.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> About()
        {
            string path = Path.GetFullPath("wwwroot/about.txt");
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
            string path = Path.GetFullPath("wwwroot/about.txt");
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(edited);
            }
            return RedirectToAction(nameof(About));
        }
    }
}
