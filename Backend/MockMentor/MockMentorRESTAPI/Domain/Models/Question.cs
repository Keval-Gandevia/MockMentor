using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class Question
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int questionId { get; set; }
        [Required]
        public string? questionText { get; set; }
    }
}
