using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Repositories
{
    public interface IVideoAnalysisRepository
    {
        Task<VideoAnalysis> AddVideoAnalysisAsync(VideoAnalysis videoAnalysis);
        Task<VideoAnalysis> GetVideoAnalysisByVideoIdAsync(int videoId);
        Task<bool> UpdateVideoAnalysisAsync(VideoAnalysis videoAnalysis);
    }
}
