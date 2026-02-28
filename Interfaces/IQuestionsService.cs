using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IQuestionsService
    {
        void DeleteQuestions();
        QuestionModel GetFirtQuestionNotAnswered();
        int GetQuestionCount();
        QuestionsList GetQuestions();
        void SaveAnswer(int id);
        void SaveQuestions(QuestionsList questions);
    }
}