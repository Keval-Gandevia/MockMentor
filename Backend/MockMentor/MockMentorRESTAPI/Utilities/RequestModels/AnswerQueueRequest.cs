namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class AnswerQueueRequest
    {
        public required int questionId {  get; set; }
        public required string videoUrl { get; set; }
        public required MessageType messageType { get; set; }
    }
}
