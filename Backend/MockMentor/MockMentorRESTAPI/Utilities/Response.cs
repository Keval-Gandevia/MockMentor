using System.Net;

namespace MockMentorRESTAPI.Utilities
{
    public class Response
    {
        public required HttpStatusCode statusCode { get; set; }
        public string? message { get; set; }
        public object? payload { get; set; }
    }
}
