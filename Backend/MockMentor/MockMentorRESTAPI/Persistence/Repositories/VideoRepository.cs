using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class VideoRepository : BaseRepository, IVideoRepository
    {
        public VideoRepository(AppDbContext context) : base(context) { }

        public async Task<Video> AddVideosAsync(Video video)
        {
            await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();
            return video;
        }

        public async Task<Video> GetVideoByQuestionIdAsync(int questionId)
        {
            return await _context.Videos.FirstOrDefaultAsync(v => v.questionId == questionId);
        }
    }
}
