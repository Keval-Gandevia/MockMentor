using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AddAnswerRequest
    {
        [Required]
        public required int questionId {  get; set; }
        [Required]
        public required string answerText { get; set; }
    }
}
