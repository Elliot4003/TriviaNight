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

        public void SaveCategories(CategoriesList categories)
        {
            if (categories.Categories != null) 
            {
                // Insertion des questions dans la mémoire
                foreach (CategoryModel category in categories.Categories)
                {
                    _context.Categories.Add(category);
                }

                _context.SaveChanges();
            }
        }

        public void SaveQuestions(QuestionsList questions)
        {
            if (questions.Questions != null)
            {
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

        public void InitializeScore()
        {
            ScoreModel score = new ScoreModel()
            {
                Id = 1,
                Score = 0
            };
            _context.Score.Add(score);
            _context.SaveChanges();
        }

        public void SaveScore()
        {
            ScoreModel score = _context.Score.Find(1) ?? new();
            if (score != null) 
            {
                _context.Score.Attach(score);
                score.Score++;
                _context.SaveChanges();
            }
        }

        public void SaveAnswer(int id)
        {
            QuestionModel question = _context.Questions.Find(id) ?? new();
            if (question != null)
            {
                question.Answered = true;
                _context.SaveChanges();
            }
        }

        public void DeleteQuestions()
        {
            _context.Questions.RemoveRange(_context.Questions);
            _context.SaveChanges();
        }

        public void DeleteScore()
        {
            _context.Score.RemoveRange(_context.Score);
            _context.SaveChanges();
        }

        public ScoreModel GetScore()
        {
            ScoreModel score = _context.Score.Find(1) ?? new();
            return score;
        }

        public int GetCategoryCount()
        {
            return _context.Categories.Count();
        }

        public CategoriesList GetCategories()
        {
            CategoriesList categories = new();
            categories.Categories = []; // Initialisation
            foreach (CategoryModel category in _context.Categories)
            {
                categories.Categories.Add(category);
            }
            return categories;
        }

        public int GetQuestionCount()
        {
            return _context.Questions.Count();
        }

        public QuestionsList GetQuestions() 
        {
            QuestionsList questions = new();
            questions.Questions = [];
            foreach (QuestionModel question in _context.Questions)
            {
                questions.Questions.Add(question);
            }
            return questions;
        }
    }
}
