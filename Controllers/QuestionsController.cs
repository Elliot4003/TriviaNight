using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TriviaNight.Enum;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApi _api;
        private readonly IDb _db;

        public QuestionsController(ILogger<CategoriesController> logger, IApi api, IDb db)
        {
            _logger = logger;
            _api = api;
            _db = db;
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
            _db.SaveQuestions(questions);

            return RedirectToAction("Question", new { id = 1 });
        }

        public IActionResult Question(int id) 
        {
            QuestionsList questions = _db.GetQuestions();
            if (id <= _db.GetQuestionCount() && questions.Questions != null)
            {
                if (id == 1) _db.InitializeScore();
                ViewBag.Score = _db.GetScore().Score;
                ViewBag.QuestionCount = _db.GetQuestionCount();
                QuestionModel question = questions.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault() ?? new(); // Première question non-répondue
                ViewBag.Id = question.Id;

                return View(question);
            }

            return RedirectToAction("Score"); // Ecran du score
        }

        [HttpPost]
        public IActionResult SaveAnswerAndScore([FromBody] string data)
        {
            if (data.Contains("_correct")) _db.SaveScore();

            int id = Int32.Parse(data.Split("_")[0]);
            _db.SaveAnswer(id);
            return Ok();
        }

        public IActionResult Score()
        {
            ScoreModel score = _db.GetScore() ?? new(); // Score unique avec un id à 1 (temporaire)

            switch (score.Score)
            {
                case var _ when score.Score <= 3:
                    score.ScoreResult = ScoreResultEnum.Bad;
                    break;
                case var _ when (score.Score > 3 && score.Score <= 6):
                    score.ScoreResult = ScoreResultEnum.Medium;
                    break;
                case var _ when (score.Score > 6 && score.Score <= 9):
                    score.ScoreResult = ScoreResultEnum.Good;
                    break;
                case var _ when score.Score == 10:
                    score.ScoreResult = ScoreResultEnum.Perfect;
                    break;
                default:
                    break;
            }

            score.QuestionCount = _db.GetQuestionCount();

            return View(score);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
