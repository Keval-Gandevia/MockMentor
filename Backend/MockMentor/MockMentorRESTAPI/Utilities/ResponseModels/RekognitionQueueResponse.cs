namespace MockMentorRESTAPI.Utilities.ResponseModels
{
    public class RekognitionQueueResponse
    {
        public required int questionId { get; set; }
        public required int videoId { get; set; }
        public required string emotionValue { get; set; }
        public required MessageType messageType { get; set; }
    }
}
