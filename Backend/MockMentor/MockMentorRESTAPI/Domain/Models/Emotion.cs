using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class Emotion
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emotionId { get; set; }
        [Required]
        public string emotionValue { get; set; }

        [ForeignKey("Video")]
        public int videoId { get; set; }
        public virtual Video Video { get; set; }
    }
}
