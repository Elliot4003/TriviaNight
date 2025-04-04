using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApi _api;
        private readonly IDb _db;
        private readonly TriviaNightDbContext _context;

        public CategoriesController(ILogger<CategoriesController> logger, IApi api, IDb db, TriviaNightDbContext context)
        {
            _logger = logger;
            _api = api;
            _db = db;
            _context = context;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            _db.DeleteQuestions(_context); // Suppression des potentielles questions
            _db.DeleteScore(_context); // Suppression du potentiel score

            CategoriesList categories = new();
            if (_context.Categories.Count() == 0) 
            {
                categories = _api.CategoryRequest(); // Première récupération des catégories
                _db.SaveCategories(categories, _context);
            } 
            else
            {
                categories.Categories = []; // Initialisation
                foreach (CategoryModel category in _context.Categories) 
                {
                    categories.Categories.Add(category);
                }
            }

            return View(categories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}