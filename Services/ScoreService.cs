using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class ScoreService : IScoreService
    {
        private readonly TriviaNightDbContext _context;

        public ScoreService(TriviaNightDbContext context)
        {
            _context = context;
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
        /// Supprime le score du contexte
        /// </summary>
        public void DeleteScore()
        {
            _context.Score.RemoveRange(_context.Score);
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
    }
}
