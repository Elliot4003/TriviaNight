using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApi _api;

        public QuestionsController(ILogger<CategoriesController> logger, IApi api)
        {
            _logger = logger;
            _api = api;
        }

        public IActionResult Questions(int category, int amount, string difficulty)
        {
            QuestionRequestModel questionRequest = new QuestionRequestModel()
            {
                Amount = amount,
                Category = category,
                Difficulty = difficulty
            };

            List<QuestionModel> questions = new List<QuestionModel>();
            questions = QuestionRequest(questionRequest);

            return View(questions);
        }

        [HttpGet]
        private List<QuestionModel> QuestionRequest(QuestionRequestModel questionRequest)
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty.ToLower();
            HttpResponseMessage response = _api.Client.GetAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionsResponse>(data);
                questions = result?.Questions ?? new List<QuestionModel>();
            }

            return questions;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
