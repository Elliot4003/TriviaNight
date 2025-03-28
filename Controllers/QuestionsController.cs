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
            QuestionRequestModel questionRequest = new()
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
                ViewBag.Score = _context.Score;
                ViewBag.QuestionCount = _context.Questions.Count();
                ViewBag.Id = id;
                QuestionModel question = _context.Questions.Find(id); // Récupération dans la mémoire 
                if (question.Answered || question.Id > _context.Questions.Where(x => !x.Answered).Select(x => x.Id).FirstOrDefault()) question = _context.Questions.Where(x => !x.Answered).FirstOrDefault(); // Si on est sur une question déjà répondue

                return View(question);
            }

            // Fin des questions
            _db.DeleteQuestions(_context);
            _db.DeleteScore(_context);

            return RedirectToAction("Categories", "Categories"); // Retour aux catégories
        }

        [HttpPost]
        public IActionResult SaveAnswerAndScore([FromBody] string data)
        {
            if (data.Contains("correct")) _db.SaveScore(_context);

            int id = Int32.Parse(data.Split("_")[0]);
            _db.SaveAnswer(id, _context);
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
