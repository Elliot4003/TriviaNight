using TriviaNight.Models;

namespace TriviaNight.Interfaces
{
    public interface IScoreService
    {
        void DeleteScore();
        ScoreModel GetScore();
        void SaveScore();
    }
}