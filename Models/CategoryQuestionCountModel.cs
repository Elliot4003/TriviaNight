using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class CategoryQuestionCountModel
    {
        [JsonPropertyName("total_num_of_questions")]
        public int? TotalNumOfQuestions { get; set; }

        [JsonPropertyName("total_num_of_pending_questions")]
        public int? TotalNumOfPendingQuestions { get; set; }

        [JsonPropertyName("total_num_of_verified_questions")]
        public int? TotalNumOfVerifiedQuestions { get; set; }

        [JsonPropertyName("total_num_of_rejected_questions")]
        public int? TotalNumOfRejectedQuestions { get; set; }
    }   

    public class CategoriesQuestionCountResponse
    {
        [JsonPropertyName("categories")]
        public List<Dictionary<int, CategoryQuestionCountModel>>? CategoriesQuestionCount { get; set; }
    }
}
