using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TriviaNight.Enum;
using TriviaNight.Extensions;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IApiService _apiService;

        public QuestionsController(
            ILogger<CategoriesController> logger, 
            IApiService apiService
            )
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Questions(int category, int amount, string difficulty)
        {
            QuestionRequestModel questionRequest = new()
            {
                Amount = amount,
                Category = category,
                Difficulty = difficulty
            };

            var questions = await _apiService.QuestionRequestAsync(questionRequest); // Récupération via l'API

            var score = InitializeScore(questions.Questions.Count);

            // Enregistrement en session du score
            HttpContext.Session.Set("Score", score);

            int i = 1;
            // Insertion des questions dans la mémoire
            foreach (var question in questions.Questions)
            {
                question.Id = i++;
            }

            // Enregistrement en session des questions
            HttpContext.Session.Set("Questions", questions);

            return RedirectToAction("Question");
        }

        public IActionResult Question() 
        {
            var questions = GetSessionQuestions();
            var score = GetSessionScore();

            // Récupération de la première question sans réponse afin de faciliter la navigation
            var question = questions.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault();

            // Dernière question
            if (question is null)
                return RedirectToAction("Score");

            ViewBag.Id = question.Id;

            return View(question);
        }

        [HttpPost]
        public IActionResult SaveAnswerAndScore([FromBody] string data)
        {
            var questions = GetSessionQuestions();
            var score = GetSessionScore();

            // On reçoit la donnée sous la forme <id>_correct ou <id>_incorrect pour récupérer l'id et connaitre le résultat
            if (data.Contains("_correct"))
            {
                score.Score++;
                HttpContext.Session.Set("Score", score);
            }

            int id = Int32.Parse(data.Split("_")[0]);
            var question = questions.Questions.Find(x => x.Id == id);
            if (question is null)
                throw new Exception("Aucune question trouvée pour cet ID.");

            question.Answered = true;

            // Màj session
            HttpContext.Session.Set("Questions", questions);

            return Ok();
            
        }

        public IActionResult Score()
        {
            var questions = GetSessionQuestions();
            var score = GetSessionScore();
            var questionCount = questions.Questions.Count;

            // Vérifier qu'une réponse a été apportée à chaque question, sinon on redirige vers la première question sans réponse
            var questionsLeft = questions.Questions.Count(x => !x.Answered);
            if (questionsLeft != 0) 
                return RedirectToAction("Question");

            var perf = score.Score > 0 ? score.Score / questionCount * 10 : 0; // Calcul de la performance

            // La performance permet de récupérer un GIF adapté au score
            switch (perf)
            {
                case var _ when perf <= 3:
                    score.ScoreResult = ScoreResultEnum.Bad;
                    break;
                case var _ when (perf > 3 && perf <= 6):
                    score.ScoreResult = ScoreResultEnum.Medium;
                    break;
                case var _ when (perf > 6 && perf <= 9):
                    score.ScoreResult = ScoreResultEnum.Good;
                    break;
                case var _ when perf == 10:
                    score.ScoreResult = ScoreResultEnum.Perfect;
                    break;
                default:
                    break;
            }

            return View(score);
        }

        private QuestionsList GetSessionQuestions()
        {
            var questions = HttpContext.Session.Get<QuestionsList>("Questions");

            if (questions is null || questions.Questions is null || questions.Questions.Count == 0)
                throw new Exception("Aucune question trouvée pour cette session.");

            return questions;
        }

        private ScoreModel GetSessionScore()
        {
            var score = HttpContext.Session.Get<ScoreModel>("Score");

            if (score is null)
                throw new Exception("Aucun score trouvé pour cette session.");

            return score;
        }

        private static ScoreModel InitializeScore(int questionCount)
        {
            return new ScoreModel()
            {
                Id = 1,
                Score = 0,
                QuestionCount = questionCount
            };
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
