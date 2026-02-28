using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class CategoryQuestionCountModel
    {
        [JsonPropertyName("total_question_count")]
        public int TotalQuestionCount { get; set; }

        [JsonPropertyName("total_easy_question_count")]
        public int TotalEasyQuestionCount { get; set; }

        [JsonPropertyName("total_medium_question_count")]
        public int TotalMediumQuestionCount { get; set; }

        [JsonPropertyName("total_hard_question_count")]
        public int TotalHardQuestionCount { get; set; }
    }   

    public class CategoryQuestionCountResponse
    {
        [JsonPropertyName("category_id")]
        public int Id { get; set; }

        [JsonPropertyName("category_question_count")]
        public required CategoryQuestionCountModel CategoryQuestionCount { get; set; }
    }
}
