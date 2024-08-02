using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IFeedbackRepository
    {
        Task<Feedback> AddFeedbackAsync(Feedback feedback);
    }
}
