namespace MockMentorRESTAPI.Utilities.ResponseModels
{
    public class AnswerQueueResponse
    {
        public required int questionId { get; set; }
        public required string answerText { get; set; }
        public required MessageType messageType { get; set; }
    }
}
