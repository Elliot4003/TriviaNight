using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApiService _apiService;
        private readonly IQuestionsService _questionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IScoreService _scoreService;

        public CategoriesController(
            ILogger<CategoriesController> logger, 
            IApiService apiService, 
            IQuestionsService questionsService, 
            ICategoriesService categoriesService, 
            IScoreService scoreService)
        {
            _logger = logger;
            _apiService = apiService;
            _questionsService = questionsService;
            _categoriesService = categoriesService;
            _scoreService = scoreService;
        }

        [HttpGet]
        public IActionResult Categories()
        {
            _questionsService.DeleteQuestions(); // Suppression des potentielles questions
            _scoreService.DeleteScore(); // Suppression du potentiel score

            var categories = new CategoriesList();
            if (_categoriesService.GetCategoryCount() == 0) 
            {
                categories = _apiService.CategoryRequest(); // Première récupération des catégories
                _categoriesService.SaveCategories(categories);
            } 
            else
            {
                categories = _categoriesService.GetCategories(); // Récupération en mémoire
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