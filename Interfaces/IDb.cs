using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IDb
    {
        public void SaveCategories(CategoriesList categories, TriviaNightDbContext context);

        public void SaveQuestions(QuestionsList questions, TriviaNightDbContext context);

        public void DeleteQuestions(TriviaNightDbContext context);
    }
}
