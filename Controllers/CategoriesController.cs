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

        public CategoriesController(ILogger<CategoriesController> logger, IApi api)
        {
            _logger = logger;
            _api = api;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            CategoriesList categories = _api.CategoryRequest(); // Récupération des catégories

            return View(categories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}