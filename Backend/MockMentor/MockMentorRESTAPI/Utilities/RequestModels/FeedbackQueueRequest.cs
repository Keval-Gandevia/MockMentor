﻿namespace MockMentorRESTAPI.Utilities.RequestModels
{
    public class FeedbackQueueRequest
    {
        public required int answerId { get; set; }
        public required string questionText { get; set; }
        public required string answerText { get; set; }
        public required MessageType messageType { get; set; }
    }
}
