using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class QuestionModel
    {
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
    }

    public class QuestionsResponse
    {
        [JsonPropertyName("results")]
        public List<QuestionModel>? Questions { get; set; }
    }
}
