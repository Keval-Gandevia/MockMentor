using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AddFeedbackRequest
    {
        [Required]
        public required string feedbackText { get; set; }

        [Required]
        public int answerId { get; set; }
    }
}
