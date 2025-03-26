using Microsoft.EntityFrameworkCore;
using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class DbService : IDb
    {
        public DbService() 
        {
        }

        public void SaveQuestions(QuestionsList questions, TriviaNightDbContext context)
        {
            int i = 1;
            // Insertion des questions dans la mémoire
            foreach (QuestionModel question in questions.Questions)
            {
                question.Id = i++;
                context.Questions.Add(question);
            }

            context.SaveChanges();
        }

        public void DeleteQuestions(TriviaNightDbContext context)
        {
            context.Remove(context.Questions);
            context.SaveChanges();
        }
    }
}
