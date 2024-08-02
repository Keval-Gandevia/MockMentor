using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AddVideoRequest
    {
        [Required]
        public required string videoUrl { get; set; }

        [Required]
        public int questionId { get; set; }
    }
}
