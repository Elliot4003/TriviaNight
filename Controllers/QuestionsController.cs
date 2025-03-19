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
        private readonly TriviaNightDbContext _context;

        public QuestionsController(ILogger<CategoriesController> logger, IApi api, TriviaNightDbContext context)
        {
            _logger = logger;
            _api = api;
            _context = context;
        }

        public IActionResult Questions(int category, int amount, string difficulty)
        {
            QuestionModel question = new QuestionModel();

            // Première récupération des questions
            if (_context.Questions != null)
            {
                QuestionRequestModel questionRequest = new QuestionRequestModel()
                {
                    Amount = amount,
                    Category = category,
                    Difficulty = difficulty
                };

                QuestionRequest(questionRequest); // Récupération via l'API
                ViewBag.Index = 0; // Initialisation du numéro des questions
            }

            ViewBag.Index += 1; // Incrémentation du numéro des questions
            int index = ViewBag.Index;

            if (index <= _context.Questions.Count()) 
            {
                // Récupération dans la mémoire
                question = _context.Questions.Find(index);

                return View(question);
            }

            // Retour aux catégories si les questions sont épuisées
            return RedirectToAction("Categories");

        }

        [HttpGet]
        private void QuestionRequest(QuestionRequestModel questionRequest)
        {
            QuestionsList questions = new QuestionsList();
            string request = "api.php?category=" + questionRequest.Category.ToString() + "&amount=" + questionRequest.Amount.ToString() + "&difficulty=" + questionRequest.Difficulty.ToLower();
            HttpResponseMessage response = _api.Client.GetAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var result = JsonSerializer.Deserialize<QuestionsList>(data);
                questions = result ?? new QuestionsList();

                int i = 1;
                // Insertion des questions dans la mémoire
                foreach (QuestionModel question in questions.Questions)
                {
                    question.Id = i++;
                    _context.Questions.Add(question);
                }

                _context.SaveChanges();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
