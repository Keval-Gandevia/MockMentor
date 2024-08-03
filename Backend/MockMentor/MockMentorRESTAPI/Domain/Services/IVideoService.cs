using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IVideoService
    {
        Task<Response> AddVideoAsync(AddVideoRequest addVideoRequestvideo);
        Task<Response> GetVideoByQuestionIdAsync(int questionId);
    }
}
