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
        public string? Type { get; set; }

        [JsonPropertyName("difficulty")]
        public string? Difficulty { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("question")]
        public string? Question { get; set; }

        [JsonPropertyName("correct_answer")]
        public string? CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public List<String>? IncorrectAnswers { get; set; }

        public bool Answered { get; set; }
    }

    public class QuestionsList
    {
        [JsonPropertyName("results")]
        public List<QuestionModel>? Questions { get; set; }
    }
}
