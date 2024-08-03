using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IEmotionService
    {
        Task<Response> AddEmotionAsync(AddEmotionRequest addEmotionRequest);
        Task<Response> GetEmotionByVideoIdAsync(int videoId);
    }
}
