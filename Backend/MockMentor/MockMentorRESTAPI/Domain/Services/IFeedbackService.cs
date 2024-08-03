using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IFeedbackService
    {
        Task<Response> AddFeedbackAsync(AddFeedbackRequest addFeedbackRequest);
        Task<Response> GetFeedbackAsync(int questionId);
    }
}
