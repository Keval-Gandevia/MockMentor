using Microsoft.EntityFrameworkCore;
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

        public async Task<Answer> GetAnswerAsync(int answerId)
        {
            return await _context.Answers.FirstOrDefaultAsync(a => a.answerId == answerId);
        }

        public async Task<Answer> GetAnswerByQuestionIdAsync(int questionId)
        {
            return await _context.Answers.FirstOrDefaultAsync(a => a.questionId == questionId);
        }
    }
}
