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
            // Première récupération des questions
            if (_context.Questions.Count() == 0)
            {
                QuestionRequestModel questionRequest = new QuestionRequestModel()
                {
                    Amount = amount,
                    Category = category,
                    Difficulty = difficulty
                };

                QuestionsList questions = _api.QuestionRequest(questionRequest); // Récupération via l'API
                _db.SaveQuestions(questions, _context);
                ViewBag.Index = 0; // Initialisation du numéro des questions
            }

            ViewBag.Index += 1; // Incrémentation du numéro des questions

            if (ViewBag.Index <= _context.Questions.Count()) 
            {
                // Récupération dans la mémoire
                QuestionModel question = _context.Questions.Find(ViewBag.Index);

                return View(question);
            } 
            else
            {
                _db.DeleteQuestions(_context); // Fin des questions
                return RedirectToAction("Categories"); // Retour aux catégories
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
