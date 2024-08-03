using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IVideoRepository
    {
        Task<Video> AddVideosAsync(Video video);
        Task<Video> GetVideoByQuestionIdAsync(int questionId);
    }
}
