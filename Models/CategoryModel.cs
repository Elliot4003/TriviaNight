using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class CategoryModel
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        public int QuestionCount { get; set; }
    }

    public class CategoriesList
    {
        [JsonPropertyName("trivia_categories")]
        public List<CategoryModel>? Categories { get; set; }
    }
}