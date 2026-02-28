using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TriviaNight.Extensions;
using TriviaNight.Interfaces;
using TriviaNight.Models;
using TriviaNight.Services;

namespace TriviaNight.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApiService _apiService;

        public CategoriesController(
            ILogger<CategoriesController> logger, 
            IApiService apiService
            )
        {
            _logger = logger;
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            CleanSession();

            // Initilisation
            if (!HttpContext.Session.Keys.Contains("Categories"))
            {
                var categoryRequestResult = await _apiService.CategoryRequestAsync();
                HttpContext.Session.Set("Categories", categoryRequestResult);
            }

            var categories = GetSessionCategories();

            return View(categories);
        }

        private void CleanSession()
        {
            // Nettoyage de la session
            HttpContext.Session.Remove("Questions");
            HttpContext.Session.Remove("Score");
        }

        private CategoriesList GetSessionCategories()
        {
            var categories = HttpContext.Session.Get<CategoriesList>("Categories");

            if (categories is null || categories.Categories is null || categories.Categories.Count == 0)
                throw new Exception("Aucune catégorie trouvée pour cette session.");

            return categories;
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