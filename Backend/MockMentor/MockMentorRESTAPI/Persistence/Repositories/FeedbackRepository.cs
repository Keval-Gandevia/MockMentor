using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context) { }

        public async Task<Feedback> AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
