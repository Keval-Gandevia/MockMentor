using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Repositories;
using MockMentorRESTAPI.Persistence.Contexts;

namespace MockMentorRESTAPI.Persistence.Repositories
{
    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(AppDbContext context) : base(context) { }

        public async Task<Answer> AddAnswerAsync(Answer answer)
        {
            await _context.Answers.AddAsync(answer);
            await _context.SaveChangesAsync();
            return answer;
        }
    }
}
