using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TriviaNight.Enum;

namespace TriviaNight.Models
{
    public class ScoreModel
    {
        [Key]
        public int Id { get; set; } = 1;

        public int Score { get; set; }
        
        public ScoreResultEnum ScoreResult { get; set; }

        public int QuestionCount {  get; set; }
    }
}
