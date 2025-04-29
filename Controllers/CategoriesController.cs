using System.Diagnostics;
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

        public CategoriesController(ILogger<CategoriesController> logger, IApi api, IDb db)
        {
            _logger = logger;
            _api = api;
            _db = db;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            _db.DeleteQuestions(); // Suppression des potentielles questions
            _db.DeleteScore(); // Suppression du potentiel score

            var categories = new CategoriesList();
            if (_db.GetCategoryCount() == 0) 
            {
                categories = _api.CategoryRequest(); // Première récupération des catégories
                _db.SaveCategories(categories);
            } 
            else
            {
                categories = _db.GetCategories(); // Récupération en mémoire
            }

            return View(categories);
        }

        public IActionResult Contribute()
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