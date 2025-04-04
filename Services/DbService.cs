using Microsoft.EntityFrameworkCore;
using System.Linq;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class DbService : IDb
    {
        public DbService() 
        {
        }

        public void SaveCategories(CategoriesList categories, TriviaNightDbContext context)
        {
            // Insertion des questions dans la mémoire
            foreach(CategoryModel category in categories.Categories)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();
        }

        public void SaveQuestions(QuestionsList questions, TriviaNightDbContext context)
        {
            int i = 1;
            // Insertion des questions dans la mémoire
            foreach(QuestionModel question in questions.Questions)
            {
                question.Id = i++;
                context.Questions.Add(question);
            }

            context.SaveChanges();
        }

        public void InitializeScore(TriviaNightDbContext context)
        {
            ScoreModel score = new ScoreModel()
            {
                Id = 1,
                Score = 0
            };
            context.Score.Add(score);
            context.SaveChanges();
        }

        public void SaveScore(TriviaNightDbContext context)
        {
            ScoreModel score = context.Score.Find(1);
            score.Score++;
            context.SaveChanges();
        }

        public void SaveAnswer(int id, TriviaNightDbContext context)
        {
            QuestionModel question = context.Questions.Find(id);
            if (question != null)
            {
                question.Answered = true;
                context.SaveChanges();
            }
        }

        public void DeleteQuestions(TriviaNightDbContext context)
        {
            context.Questions.RemoveRange(context.Questions);
            context.SaveChanges();
        }

        public void DeleteScore(TriviaNightDbContext context)
        {
            context.Score.RemoveRange(context.Score);
            context.SaveChanges();
        }
    }
}
