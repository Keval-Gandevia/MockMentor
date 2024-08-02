using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> AddAnswerAsync(Answer answer);
    }
}
