using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IQuestionRepository
    {
        Task AddQuestionAsync(Question question);
    }
}
