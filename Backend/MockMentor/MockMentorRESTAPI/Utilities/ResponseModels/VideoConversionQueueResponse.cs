namespace MockMentorRESTAPI.Utilities.ResponseModels
{
    public class VideoConversionQueueResponse
    {
        public required int questionId {  get; set; }
        public required string videoUrl { get; set; }
        public required MessageType messageType { get; set; }
    }
}
