namespace MockMentorRESTAPI.Utilities.ResponseModels
{
    public class FeedbackQueueResponse
    {
        public required int answerId {  get; set; }
        public required string questionText { get; set; }
        public required string answerText { get; set; }
        public required string feedbackText { get; set; }
        public required MessageType messageType { get; set; }
    }
}
