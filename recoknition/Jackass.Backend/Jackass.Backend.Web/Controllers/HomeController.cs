using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Jackass.Backend.Web.Models;
using Microsoft.AspNetCore.Cors;

namespace Jackass.Backend.Web.Controllers
{
    [EnableCors("MyPolicy")]
    public sealed class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
