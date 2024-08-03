using System.ComponentModel.DataAnnotations;

namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AddVideoAnalysisRequest
    {

        [Required]
        public int videoId { get; set; }
        public bool isFeedbackCompleted { get; set; }
        public bool isRekognitionCompleted { get; set; }
    }
}
