using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class VideoAnalysisRepository : BaseRepository, IVideoAnalysisRepository
    {
        public VideoAnalysisRepository(AppDbContext context) : base(context) { }
       
        public async Task<VideoAnalysis> AddVideoAnalysisAsync(VideoAnalysis videoAnalysis)
        {
            await _context.VideoAnalyses.AddAsync(videoAnalysis);
            await _context.SaveChangesAsync();
            return videoAnalysis;
        }

        public async Task<VideoAnalysis> GetVideoAnalysisByVideoIdAsync(int videoId)
        {
            return await _context.VideoAnalyses.FirstOrDefaultAsync(v => v.videoId == videoId);
        }

        public async Task<bool> UpdateVideoAnalysisAsync(VideoAnalysis videoAnalysis)
        {
            VideoAnalysis? updateVideoAnalysis = await _context.VideoAnalyses.FindAsync(videoAnalysis.videoAnalysisId);

            if(updateVideoAnalysis != null)
            {
                updateVideoAnalysis.isFeedbackCompleted = videoAnalysis.isFeedbackCompleted;
                updateVideoAnalysis.isRekognitionCompleted = videoAnalysis.isRekognitionCompleted;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
