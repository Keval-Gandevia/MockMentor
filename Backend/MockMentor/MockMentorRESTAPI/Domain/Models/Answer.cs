using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class Answer
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int answerId { get; set; }
        [Required]
        public string answerText { get; set; }
        [ForeignKey("Question")]
        public int questionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual Feedback? feedback { get; set; }
    }
}
