using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IDb
    {
        public void SaveCategories(CategoriesList categories);

        public void SaveQuestions(QuestionsList questions);

        public void SaveScore();

        public void SaveAnswer(int id);

        public void DeleteQuestions();

        public void DeleteScore();

        public ScoreModel GetScore();

        public QuestionModel GetFirtQuestionNotAnswered();

        public int GetCategoryCount();

        public CategoriesList GetCategories();

        public int GetQuestionCount();

        public QuestionsList GetQuestions();
    }
}
