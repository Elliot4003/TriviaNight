using System.Text.Json.Serialization;

namespace TriviaNight.Models;

public class QuestionRequestModel
{
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("category")]
    public int Category { get; set; }

    [JsonPropertyName("difficulty")]
    public required string Difficulty { get; set; }
}
