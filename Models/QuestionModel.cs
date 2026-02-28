using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class QuestionModel
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("difficulty")]
        public required string Difficulty { get; set; }

        [JsonPropertyName("category")]
        public required string Category { get; set; }

        [JsonPropertyName("question")]
        public required string Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public required string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public required List<String> IncorrectAnswers { get; set; }

        public bool Answered { get; set; }
    }

    public class QuestionsList
    {
        [JsonPropertyName("results")]
        public required List<QuestionModel> Questions { get; set; }
    }
}
