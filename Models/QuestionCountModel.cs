using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class QuestionCountModel
    {
        [JsonPropertyName("total_num_of_questions")]
        public int TotalNumOfQuestions { get; set; }

        [JsonPropertyName("total_num_of_pending_questions")]
        public int TotalNumOfPendingQuestions { get; set; }

        [JsonPropertyName("total_num_of_verified_questions")]
        public int TotalNumOfVerifiedQuestions { get; set; }

        [JsonPropertyName("total_num_of_rejected_questions")]
        public int TotalNumOfRejectedQuestions { get; set; }
    }   

    public class QuestionCountResponse
    {
        [JsonPropertyName("overall")]
        public QuestionCountModel? Overall { get; set; }

        [JsonPropertyName("categories")]
        public required Dictionary<string, QuestionCountModel> QuestionCount { get; set; }
    }
}
