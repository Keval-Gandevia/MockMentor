using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> AddQuestionAsync(Question question);
    }
}
