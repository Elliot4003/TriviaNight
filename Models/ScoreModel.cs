using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TriviaNight.Models
{
    public class ScoreModel
    {
        [Key]
        public int Id { get; set; } = 1;
        public int Score { get; set; }

    }
}
