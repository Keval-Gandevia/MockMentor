using MockMentorRESTAPI.Utilities.RequestModels;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Domain.Models;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IVideoAnalysisService
    {
        Task<Response> AddVideoAnalysisAsync(AddVideoAnalysisRequest videoAnalysisRequest);
        Task<Response> GetVideoAnalysisByVideoIdAsync(int videoId);
        Task<Response> UpdateVideoAnalysisAsync(VideoAnalysis videoAnalysis);
    }
}
