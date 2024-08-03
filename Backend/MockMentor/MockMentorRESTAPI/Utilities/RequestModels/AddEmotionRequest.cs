using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AddEmotionRequest
    {
        [Required]
        public int videoId { get; set; }
        [Required]
        public required string emotionValue { get; set; }
    }
}
