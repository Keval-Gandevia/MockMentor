using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IQuestionService
    {
        Task<Response> AddQuestionAsync(Question question);
        Task<Response> GetQuestionByIdAsync(int questionId);
    }
}
