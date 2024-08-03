using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IEmotionRepository
    {
        Task<Emotion> AddEmotionAsync(Emotion emotion);
        Task<Emotion> GetEmotionByVideoIdAsync(int videoId);
    }
}
