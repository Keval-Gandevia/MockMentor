namespace MockMentorRESTAPI.Utilities.ResponseModels
{
    public class GetFeedbackResponse
    {
        public required int feedbackId {  get; set; }
        public required string feedbackText { get; set; }
        public required int answerId { get; set; }
    }
}
