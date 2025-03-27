using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApi _api;
        private readonly IDb _db;
        private readonly TriviaNightDbContext _context;

        public QuestionsController(ILogger<CategoriesController> logger, IApi api, IDb db, TriviaNightDbContext context)
        {
            _logger = logger;
            _api = api;
            _db = db;
            _context = context;
        }

        public IActionResult Questions(int category, int amount, string difficulty)
        {
            QuestionRequestModel questionRequest = new QuestionRequestModel()
            {
                Amount = amount,
                Category = category,
                Difficulty = difficulty
            };

            QuestionsList questions = _api.QuestionRequest(questionRequest); // Récupération via l'API
            _db.SaveQuestions(questions, _context);

            return RedirectToAction("Question", new { id = 1 });
        }

        public IActionResult Question(int id) 
        {
            if (id <= _context.Questions.Count() && _context.Questions != null)
            {
                ViewBag.Id = id;
                // Récupération dans la mémoire 
                QuestionModel question = _context.Questions.Find(id);

                return View(question);
            }

            _db.DeleteQuestions(_context); // Fin des questions
            return RedirectToAction("Categories", "Categories"); // Retour aux catégories
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
