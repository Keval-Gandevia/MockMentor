namespace MockMentorRESTAPI.Domain.Services
{
    public interface ISQSService
    {
        Task<bool> SendMessage(string queueUrl, string messageBody);
    }
}
