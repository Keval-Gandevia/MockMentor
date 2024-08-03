using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IAnswerService
    {
        Task<Response> AddAnswerAsync(AddAnswerRequest addAnswerRequest);
        Task<Response> GetAnswerByIdAsync(int answerId);
    }
}
