using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IDb
    {
        public void SaveCategories(CategoriesList categories, TriviaNightDbContext context);

        public void SaveQuestions(QuestionsList questions, TriviaNightDbContext context);

        public void InitializeScore(TriviaNightDbContext context);

        public void SaveScore(TriviaNightDbContext context);

        public void SaveAnswer(int id, TriviaNightDbContext context);

        public void DeleteQuestions(TriviaNightDbContext context);

        public void DeleteScore(TriviaNightDbContext context);
    }
}
