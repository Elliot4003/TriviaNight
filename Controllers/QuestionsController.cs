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
            _db.SaveQuestions(questions); // Enregistrement dans le contexte

            return RedirectToAction("Question");
        }

        public IActionResult Question() 
        {
            QuestionsList questions = _db.GetQuestions();
            int questionCount = _db.GetQuestionCount();

            if (questions.Questions != null && questionCount != 0)
            {
                ViewBag.Score = _db.GetScore().Score;
                ViewBag.QuestionCount = questionCount;

                // Récupération de la première question sans réponse afin de faciliter la navigation
                QuestionModel question = questions.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault() ?? new();
                ViewBag.Id = question.Id;

                if (question.Id == 0) return RedirectToAction("Score"); // Dernière question

                return View(question);
            } 

            return RedirectToAction("Categories", "Categories");
        }

        [HttpPost]
        public IActionResult SaveAnswerAndScore([FromBody] string data)
        {
            // On reçoit la donnée sous la forme <id>_correct ou <id>_incorrect pour récupérer l'id et connaitre le résultat
            if (data.Contains("_correct")) _db.SaveScore(); 

            int id = Int32.Parse(data.Split("_")[0]);
            _db.SaveAnswer(id);
            return Ok();
        }

        public IActionResult Score()
        {
            QuestionsList questions = _db.GetQuestions();
            ScoreModel scoreObj = _db.GetScore() ?? new();

            scoreObj.QuestionCount = _db.GetQuestionCount();

            // Vérifier qu'une réponse a été apportée à chaque question, sinon on redirige vers la première question sans réponse
            if (questions.Questions != null)
            {
                int questionsLeft = questions.Questions.Where(x => !x.Answered).Count();
                if (questionsLeft != 0 || scoreObj.QuestionCount == 0) return RedirectToAction("Question"); // a modifier : si dernière question -> id à 0 -> redirection infinie
            }
            
            float score = scoreObj.Score;
            float questionCount = _db.GetQuestionCount();

            float perf = score > 0 ? score / questionCount * 10 : 0; // Calcul de la performance

            // La performance permet de récupérer un GIF adapté au score
            switch (perf)
            {
                case var _ when perf <= 3:
                    scoreObj.ScoreResult = ScoreResultEnum.Bad;
                    break;
                case var _ when (perf > 3 && perf <= 6):
                    scoreObj.ScoreResult = ScoreResultEnum.Medium;
                    break;
                case var _ when (perf > 6 && perf <= 9):
                    scoreObj.ScoreResult = ScoreResultEnum.Good;
                    break;
                case var _ when perf == 10:
                    scoreObj.ScoreResult = ScoreResultEnum.Perfect;
                    break;
                default:
                    break;
            }

            return View(scoreObj);
        }

        public IActionResult Categories()
        {
            return RedirectToAction("Categories", "Categories");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
