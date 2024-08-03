using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IAnswerRepository
    {
        Task<Answer> AddAnswerAsync(Answer answer);
        Task<Answer> GetAnswerAsync(int answerId);
        Task<Answer> GetAnswerByQuestionIdAsync(int questionId);
    }
}
