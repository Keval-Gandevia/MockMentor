using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class Feedback
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int feedbackId { get; set; }
        [Required]
        public string feedbackText { get; set; }

        [ForeignKey("Answer")]
        public int answerId { get; set; }
        public virtual Answer Answer { get; set; }
    }
}
