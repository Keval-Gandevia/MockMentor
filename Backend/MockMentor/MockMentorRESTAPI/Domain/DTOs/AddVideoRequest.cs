using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Domain.DTOs
{
    public class AddVideoRequest
    {
        [Required]
        public required string videoUrl { get; set; }

        [Required]
        public int questionId { get; set; }
    }
}
