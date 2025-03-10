using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IApi _api;

        public QuestionController(ILogger<CategoryController> logger, IApi api)
        {
            _logger = logger;
            _api = api;
        }

        [HttpGet]
        public IActionResult Questions(QuestionRequestModel questionRequest)
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            questions = QuestionRequest(questionRequest);

            return View();
        }

        private List<QuestionModel> QuestionRequest(QuestionRequestModel questionRequest)
        {
            List<QuestionModel> questions = new List<QuestionModel>();
            string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty + "&";
            HttpResponseMessage response = _api.client.GetAsync(_api.baseAddress + request).Result;

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
