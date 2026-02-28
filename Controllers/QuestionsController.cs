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
        private readonly IApiService _apiService;
        private readonly IQuestionsService _questionsService;
        private readonly ICategoriesService _categoriesService;
        private readonly IScoreService _scoreService;
        private QuestionsList _questions = new();

        public QuestionsController(
            ILogger<CategoriesController> logger, 
            IApiService apiService, 
            IQuestionsService questionsService, 
            ICategoriesService categoriesService, 
            IScoreService scoreService
            )
        {
            _logger = logger;
            _apiService = apiService;
            _questionsService = questionsService;
            _categoriesService = categoriesService;
            _scoreService = scoreService;
        }

        public IActionResult Questions(int category, int amount, string difficulty)
        {
            QuestionRequestModel questionRequest = new()
            {
                Amount = amount,
                Category = category,
                Difficulty = difficulty
            };

            var questions = _apiService.QuestionRequest(questionRequest); // Récupération via l'API
            _questionsService.SaveQuestions(questions); // Enregistrement dans le contexte

            return RedirectToAction("Question");
        }

        public IActionResult Question() 
        {
            if (_questions.Questions == null)
                _questions = _questionsService.GetQuestions();

            int questionCount = _questionsService.GetQuestionCount();

            if (_questions.Questions != null && questionCount != 0)
            {
                ViewBag.Score = _scoreService.GetScore().Score;
                ViewBag.QuestionCount = questionCount;

                // Récupération de la première question sans réponse afin de faciliter la navigation
                var question = _questions.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault() ?? new();
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
            if (data.Contains("_correct")) _scoreService.SaveScore(); 

            int id = Int32.Parse(data.Split("_")[0]);
            _questionsService.SaveAnswer(id);
            return Ok();
        }

        public IActionResult Score()
        {
            var scoreObj = _scoreService.GetScore() ?? new ScoreModel();

            scoreObj.QuestionCount = _questionsService.GetQuestionCount();

            // Vérifier qu'une réponse a été apportée à chaque question, sinon on redirige vers la première question sans réponse
            if (_questions.Questions != null)
            {
                int questionsLeft = _questions.Questions.Where(x => !x.Answered).Count();
                if (questionsLeft != 0 || scoreObj.QuestionCount == 0) return RedirectToAction("Question");
            }
            
            float score = scoreObj.Score;
            float questionCount = _questionsService.GetQuestionCount();

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
