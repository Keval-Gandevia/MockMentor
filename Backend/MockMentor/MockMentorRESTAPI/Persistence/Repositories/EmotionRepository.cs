using Microsoft.EntityFrameworkCore;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class EmotionRepository : BaseRepository, IEmotionRepository
    {
        public EmotionRepository(AppDbContext context) : base(context) { }

        public async Task<Emotion> AddEmotionAsync(Emotion emotion)
        {
            await _context.Emotions.AddAsync(emotion);
            await _context.SaveChangesAsync();
            return emotion;
        }

        public async Task<Emotion> GetEmotionByVideoIdAsync(int videoId)
        {
            return await _context.Emotions.FirstOrDefaultAsync(e => e.videoId == videoId);
        }
    }
}
