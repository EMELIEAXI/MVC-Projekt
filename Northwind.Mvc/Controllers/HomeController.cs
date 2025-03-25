using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using System.Diagnostics;
using PraktiskaAppar;
using Northwind.EntityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindDatabaseContext db;
        public HomeController(ILogger<HomeController> logger,
            NorthwindDatabaseContext injectedContext)
        {
            _logger = logger;
            db = injectedContext;
        }

        public IActionResult Index()
        {
            HomeIndexViewModel model = new (

                VisitorCount: Random.Shared.Next(1, 1001),
                Categories: db.Categories.ToList(),
                Products: db.Products.ToList(),
                ProductICategory: db.Products.Include(p => p.Category).ToList()
                );

            return View(model);
        }
            
        public IActionResult ProductDetail(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Product ID is missing");
            }
            
            Product? model = db.Products.SingleOrDefault(p => p.ProductId == id);
            if (model == null)
            {
                return NotFound($"Product with ID of {id} not found.");
            }

            return View(model);
        }

        public IActionResult ViewCategory(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Category ID is missing");
            }

            var category = db.Categories
                .Include(c => c.Products) // Ladda relaterade produkter
                .FirstOrDefault(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound($"Category with ID {id} not found.");
            }

            return View(category); // Skicka kategorin och dess produkter till vyn
        }


        public IActionResult ModelBindning()
        {
            return View(); //en sida med formulär
        }

        [HttpPost]
        public IActionResult ModelBindning(Thing thing)
        {
            HomeModelBindningViewModel model = new(
                thing, 
                HasErrors: !ModelState.IsValid,
                ValidationsErrors: ModelState.Values
                    .SelectMany(state => state.Errors)
                    .Select(error => error.ErrorMessage)
                );
            return View(thing); //en sida som visar det användaren skickade
        }

        [Authorize(Roles ="Admin")]

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
