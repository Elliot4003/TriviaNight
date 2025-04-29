using Microsoft.EntityFrameworkCore;
using System.Linq;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class DbService : IDb
    {
        private readonly TriviaNightDbContext _context;

        public DbService(TriviaNightDbContext context) 
        {
            _context = context;
        }

        /// <summary>
        /// Enregistre les catégories dans le contexte au premier appel de l'API OTDB
        /// </summary>
        /// <param name="categories"></param>
        public void SaveCategories(CategoriesList categories)
        {
            if (categories.Categories != null) 
            {
                // Insertion des questions dans la mémoire
                foreach (var category in categories.Categories)
                {
                    _context.Categories.Add(category);
                }

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Enregistre les questions dans le contexte au premier appel de l'API OTDB
        /// </summary>
        /// <param name="questions"></param>
        public void SaveQuestions(QuestionsList questions)
        {
            if (questions.Questions != null)
            {
                int i = 1;
                // Insertion des questions dans la mémoire
                foreach (var question in questions.Questions)
                {
                    question.Id = i++;
                    _context.Questions.Add(question);
                }

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Enregistre le score dans le contexte après la réponse à une question
        /// </summary>
        public void SaveScore()
        {
            var score = _context.Score.Find(1) ?? new ScoreModel();
            if (score != null) 
            {
                _context.Score.Attach(score);
                score.Score++;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Marque une réponse comme "répondue"
        /// </summary>
        /// <param name="id"></param>
        public void SaveAnswer(int id)
        {
            var question = _context.Questions.Find(id) ?? new QuestionModel();
            if (question != null)
            {
                question.Answered = true;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Supprime les questions du contexte
        /// </summary>
        public void DeleteQuestions()
        {
            _context.Questions.RemoveRange(_context.Questions);
            _context.SaveChanges();
        }

        /// <summary>
        /// Supprime le score du contexte
        /// </summary>
        public void DeleteScore()
        {
            _context.Score.RemoveRange(_context.Score);
            _context.SaveChanges();
        }

        /// <summary>
        /// Initialise le score pour une session de questions
        /// </summary>
        private void InitializeScore()
        {
            var score = new ScoreModel()
            {
                Id = 1,
                Score = 0
            };
            _context.Score.Add(score);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retourne le score
        /// </summary>
        /// <returns>Le score</returns>
        public ScoreModel GetScore()
        {
            if (_context.Score.Find(1) == null) InitializeScore();
            var score = _context.Score.Find(1) ?? new();
            return score;
        }

        public QuestionModel GetFirtQuestionNotAnswered()
        {
            return _context.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault() ?? new QuestionModel();
        }

        /// <summary>
        /// Retourne le nombre de catégories dans le contexte
        /// </summary>
        /// <returns>Le nombre de catégories</returns>
        public int GetCategoryCount()
        {
            return _context.Categories.Count();
        }

        /// <summary>
        /// Retourne les catégories présentes dans le contexte
        /// </summary>
        /// <returns>Les catégories</returns>
        public CategoriesList GetCategories()
        {
            var categories = new CategoriesList();
            categories.Categories = []; // Initialisation
            foreach (var category in _context.Categories)
            {
                categories.Categories.Add(category);
            }
            return categories;
        }

        /// <summary>
        /// Retourne le nombre de questions dans le contexte
        /// </summary>
        /// <returns></returns>
        public int GetQuestionCount()
        {
            return _context.Questions.Count();
        }

        /// <summary>
        /// Retourne les questions présentes dans le contexte
        /// </summary>
        /// <returns>Les questions</returns>
        public QuestionsList GetQuestions() 
        {
            var questions = new QuestionsList();
            questions.Questions = [];
            foreach (var question in _context.Questions)
            {
                questions.Questions.Add(question);
            }
            return questions;
        }
    }
}
