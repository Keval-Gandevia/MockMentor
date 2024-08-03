using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class VideoAnalysis
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int videoAnalysisId { get; set; }
        public bool isFeedbackCompleted { get; set; } = false;
        public bool isRekognitionCompleted { get; set; } = false;

        [ForeignKey("Video")]
        public int videoId { get; set; }
        public virtual Video Video { get; set; }
    }
}
