using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MockMentorRESTAPI.Domain.Models
{
    public class Video
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int videoId {  get; set; }
        [Required]
        public string videoUrl { get; set; }

        [ForeignKey("Question")]
        public int questionId { get; set; }
        public virtual Question Question { get; set; }


    }
}
