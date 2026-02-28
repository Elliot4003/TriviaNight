using TriviaNight.Interfaces;
using TriviaNight.Models;

namespace TriviaNight.Services
{
    public class QuestionsService : IQuestionsService
    {
        private readonly TriviaNightDbContext _context;

        public QuestionsService(TriviaNightDbContext context)
        {
            _context = context;
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

        public QuestionModel GetFirtQuestionNotAnswered()
        {
            return _context.Questions.Where(x => !x.Answered).OrderBy(x => x.Id).FirstOrDefault() ?? new QuestionModel();
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
