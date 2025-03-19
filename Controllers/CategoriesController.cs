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
            List<CategoryModel> categories = new List<CategoryModel>();
            HttpResponseMessage response = _api.Client.GetAsync("api_category.php").Result;
            CategoriesQuestionCountResponse categoryQuestionCount = CategoryCount();

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesResponse>(data);
                categories = result?.Categories ?? new List<CategoryModel>();
                foreach (CategoryModel category in categories)
                {
                    category.QuestionCount = categoryQuestionCount.CategoriesQuestionCount.Where(x => x.Key.ToString() == category.Id.ToString()).Select(x => x.Value).FirstOrDefault().TotalNumOfVerifiedQuestions;
                }
            }

            return View(categories);
        }

        [HttpGet]
        private CategoriesQuestionCountResponse CategoryCount()
        {
            CategoriesQuestionCountResponse categoryQuestionCount = new CategoriesQuestionCountResponse();
            HttpResponseMessage response = _api.Client.GetAsync("api_count_global.php").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<CategoriesQuestionCountResponse>(data);
                categoryQuestionCount = result ?? new CategoriesQuestionCountResponse();
            }

            return categoryQuestionCount;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}